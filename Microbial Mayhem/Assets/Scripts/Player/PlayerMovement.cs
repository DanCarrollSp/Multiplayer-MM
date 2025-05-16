using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 velocity;

    [Header("General Settings")]
    public bool isActive = true;
    public bool useAcceleration = true; // Toggle between smooth & instant movement

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
    }

    public void SetInput(Vector2 moveInput)
    {
        input = moveInput;
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            MoveCharacter();
        }
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
            velocity = targetVelocity; // Instant movement
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
        pullForces.RemoveAll(force => force.sqrMagnitude < 0.01f); // Removes all forces that are too small
    }

    public void AddPullForce(Vector2 force)
    {
        pullForces.Add(force);
    }

    public void PullTowards(Vector2 position, float pullStrength = 0.1f)
    {
        Vector2 direction = position;
        AddPullForce(direction * pullStrength);
    }
}
