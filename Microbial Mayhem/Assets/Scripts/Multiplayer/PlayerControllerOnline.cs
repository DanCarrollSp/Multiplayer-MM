using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerOnline : NetworkBehaviour
{
    [Header("General Settings")]
    public bool isActive = true;
    public bool useAcceleration = true;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Pull Forces")]
    public float pullDampening = 5f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;

    // input and velocity are driven on the server
    private Vector2 input;
    private Vector2 velocity;
    private List<Vector2> pullForces = new List<Vector2>();

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // Only the server simulates physics; clients just get transform updates
        if (!IsServer)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (!IsOwner || !isActive) return;

        // Read WASD/gamepad input
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Send it up to the server each frame
        SendInputServerRpc(moveInput);
    }

    [ServerRpc]
    private void SendInputServerRpc(Vector2 movementInput)
    {
        input = movementInput;
    }

    void FixedUpdate()
    {
        // Only the server actually moves the Rigidbody
        if (!IsServer || !isActive) return;

        MoveCharacter();
    }

    private void MoveCharacter()
    {
        // Acceleration/deceleration
        Vector2 targetVelocity = input * speed;
        if (useAcceleration)
        {
            if (input.sqrMagnitude > 0)
                velocity = Vector2.MoveTowards(velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            else
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            velocity = targetVelocity;
        }

        ApplyPullForces();
        rb.velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
    }

    private void ApplyPullForces()
    {
        for (int i = pullForces.Count - 1; i >= 0; i--)
        {
            velocity += pullForces[i];
            pullForces[i] = Vector2.Lerp(pullForces[i], Vector2.zero, pullDampening * Time.fixedDeltaTime);
            if (pullForces[i].sqrMagnitude < 0.01f)
                pullForces.RemoveAt(i);
        }
    }

    // If other systems need to add a pull force, send that up via RPC too
    public void AddPullForce(Vector2 force)
    {
        if (!IsOwner) return;
        AddPullForceServerRpc(force);
    }

    [ServerRpc]
    private void AddPullForceServerRpc(Vector2 force)
    {
        pullForces.Add(force);
    }

    public void PullTowards(Vector2 position, float pullStrength = 1f)
    {
        if (!IsOwner) return;
        Vector2 offset = position - (Vector2)transform.position;
        if (offset.sqrMagnitude == 0f) return;
        Vector2 direction = offset.normalized;
        AddPullForce(direction * pullStrength);
    }
}
