using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    public Transform playerPosition;
    bool foundPlayer = false;

    void Start()
    {
        centrePoint = GameObject.FindGameObjectWithTag("WalkablePlane").gameObject.transform;
        playerPosition = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
        if (distanceToPlayer < 5) foundPlayer = true;
        else foundPlayer = false;

        if (!foundPlayer &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;

            if (RandomPoint(centrePoint.position, range, out point))
            {
                agent.SetDestination(point);
            }
        }
        else if (foundPlayer)
        {
            agent.SetDestination(playerPosition.position);
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


}