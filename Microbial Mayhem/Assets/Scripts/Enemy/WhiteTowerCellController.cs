using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class WhiteTowerCellController : MonoBehaviour
{
    public List<GameObject> cannons = new List<GameObject>();
    public GameObject bullet;


    public float shootInterval = 3f; // Time between shots
    private float shootTimer = 0f;

    public float rotationSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootCannons();
            shootTimer = 0f;
        }
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    void shootCannons()
    {
        foreach (var cannon in cannons)
        {
            GameObject newBullet = Instantiate(bullet, cannon.transform.position, Quaternion.identity);

            // Get direction from cannon to shoot outward
            Vector2 direction = cannon.transform.up; // cannon faces up by default in Unity

            // Apply velocity to the bullet
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 10f;
            }
        }
    }
}
