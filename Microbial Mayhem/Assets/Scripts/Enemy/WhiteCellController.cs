using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCellController : MonoBehaviour
{
    private GameObject Player;
    Rigidbody2D body;
    SeekBehaviour seekBehaviour;
    float detectionRadious = 2.5f;
    float speed = 150;
    float detectionDuration = 0;
    bool playerDetected = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        seekBehaviour = new SeekBehaviour();
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerDetected == false)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
            if (distanceToPlayer <= detectionRadious)
                playerDetected = true;
        }

        if (playerDetected == true)
        {
            Vector2 movement = seekBehaviour.Seek(speed, transform.position, Player.transform.position);
            body.MovePosition((Vector2)transform.position + movement * Time.deltaTime);
            detectionDuration += Time.deltaTime;
            if (detectionDuration > 5)
            {
                playerDetected = false;
                detectionDuration = 0;
            }
            //Debug.Log(detectionDuration);
        }

    }
}
