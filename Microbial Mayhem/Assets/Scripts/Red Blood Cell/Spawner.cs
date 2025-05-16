using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Spawner : MonoBehaviour
{
    public GameObject entity;
    public bool canSpawn = true; // Toggle spawning on/off
    public float spawnInterval = 5f; // Time between spawns

    // Paths
    public Transform[] pathNodes;

    // Red Blood Values
    public float MAX_SIZE = 1f; // Default 1
    public float MAX_SPEED = 100; // Default 100

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (canSpawn)
            {
                SpawnEntity();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEntity()
    {
        Transform spawnPoint = this.transform;
        if (entity != null && spawnPoint != null)
        {
            GameObject newEntity = Instantiate(entity, spawnPoint.position, spawnPoint.rotation);
   
            float randomSize = Random.Range(MAX_SIZE * 0.3f, MAX_SIZE);
            float randomSpeed = (1.0f - randomSize) * MAX_SPEED;
            randomSpeed = randomSpeed > (MAX_SPEED * 0.3f) ? randomSpeed : MAX_SPEED * 0.3f;

            newEntity.GetComponent<RedCellController>().SetPath(pathNodes, randomSpeed);
            newEntity.GetComponent<SizeManager>().SetValues(randomSize);
        }
    }
}
