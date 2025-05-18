// WhiteTowerCellController.cs
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WhiteTowerCellController : NetworkBehaviour
{
    [Header("References")]
    public List<GameObject> cannons = new List<GameObject>();
    public GameObject bulletPrefab; // ? make sure this is the prefab!

    [Header("Timing")]
    public float shootInterval = 3f;
    private float shootTimer = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 20f;

    private void Update()
    {
        // Always spin on every client for visuals
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Only the server actually spawns bullets
        if (!IsServer) return;

        shootTimer += Time.deltaTime;
        if (shootTimer < shootInterval) return;
        shootTimer = 0f;

        foreach (var cannon in cannons)
        {
            var newBullet = Instantiate(
                bulletPrefab,
                cannon.transform.position,
                cannon.transform.rotation
            );

            newBullet.GetComponent<NetworkObject>().Spawn();

            // Give it velocity; movement will sync via NetworkTransform
            if (newBullet.TryGetComponent<Rigidbody2D>(out var rb))
                rb.velocity = cannon.transform.up * 10f;
        }
    }
}
