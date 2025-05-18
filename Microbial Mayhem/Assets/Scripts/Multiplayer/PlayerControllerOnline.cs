// PlayerControllerOnline.cs
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(NetworkObject))]
public class PlayerControllerOnline : NetworkBehaviour
{
    public bool isActive = true;
    public bool useAcceleration = true;

    public float speed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    public float pullDampening = 5f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 input;
    private Vector2 velocity;
    private List<Vector2> pullForces = new List<Vector2>();

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // server simulates physics; clients just get transform updates
        if (!IsServer)
            rb.isKinematic = true;
    }

    private void Update()
    {
        if (!IsOwner || !isActive) return;
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        SendInputServerRpc(moveInput);
    }

    [ServerRpc]
    private void SendInputServerRpc(Vector2 movementInput)
    {
        input = movementInput;
    }

    private void FixedUpdate()
    {
        if (!IsServer || !isActive) return;
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        Vector2 targetVel = input * speed;
        if (useAcceleration)
            velocity = (input.sqrMagnitude > 0)
                ? Vector2.MoveTowards(velocity, targetVel, acceleration * Time.fixedDeltaTime)
                : Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        else
            velocity = targetVel;

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
        AddPullForce(offset.normalized * pullStrength);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShrinkOnServerRpc(float amount, ServerRpcParams rpcParams = default)
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Max(0f, s.x - amount);
        s.y = Mathf.Max(0f, s.y - amount);
        transform.localScale = s;
    }
}
