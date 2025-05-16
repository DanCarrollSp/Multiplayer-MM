using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCellController : MonoBehaviour
{
    private SeekBehaviour seekBehaviour = new SeekBehaviour();
    private Transform[] currentPath;

    private float speed = 3f;
    private int currentNode = 0;
    private float deviationRange = 3f;
    private float deviationX;
    private float deviationZ;

    void Start()
    {
        deviationX = Random.Range(-deviationRange, deviationRange);
        deviationZ = Random.Range(-deviationRange, deviationRange);
    }

    private void FixedUpdate()
    {
        if (currentNode < currentPath.Length)
        {
            // Calculate direction to the next path point
            Vector3 targetPosition = currentPath[currentNode].position;

            // Apply the deviation to the target position
            targetPosition.x += deviationX;
            targetPosition.z += deviationZ;

            // Move towards the target position
            Vector3 movement = seekBehaviour.Seek(speed, transform.position, targetPosition);
            transform.position += movement * Time.deltaTime;

            // Check if we have reached the current path point
            float arrivalTolerance = deviationRange + 1;
            if (Vector3.Dot((targetPosition - transform.position).normalized, movement.normalized) < 0)
            {
                currentNode++;
            }
        }
    }

    public void SetPath(Transform[] path, float newSpeed)
    {
        currentPath = path;
        speed = newSpeed;
    }
}
