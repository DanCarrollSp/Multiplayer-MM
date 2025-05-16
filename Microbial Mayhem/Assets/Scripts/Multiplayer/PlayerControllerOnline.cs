using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System.Globalization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerOnline : NetworkBehaviour
{
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 velocity;
    private PlayerInput playerInput;

    [Header("General Settings")]
    public bool isActive = true;
    public bool useAcceleration = true;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Pull Forces")]
    private List<Vector2> pullForces = new List<Vector2>();
    public float pullDampening = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Only read input for the owning client
        if (!IsOwner || !isActive) return;

        input = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (!IsOwner || !isActive) return;

        MoveCharacter();
    }

    private void MoveCharacter()
    {
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
        for (int i = 0; i < pullForces.Count; i++)
        {
            velocity += pullForces[i];
            pullForces[i] = Vector2.Lerp(pullForces[i], Vector2.zero, pullDampening * Time.fixedDeltaTime);
        }
        pullForces.RemoveAll(force => force.sqrMagnitude < 0.01f);
    }

    public void AddPullForce(Vector2 force)
    {
        if (!IsOwner) return;
        pullForces.Add(force);
    }

    public void PullTowards(Vector2 position, float pullStrength = 1f)
    {
        if (!IsOwner) return;

        Vector2 playerPosition = transform.position;
        Vector2 offset = position - playerPosition;

        if (offset.sqrMagnitude == 0f) return;

        Vector2 direction = offset.normalized;
        AddPullForce(direction * pullStrength);
    }
}
