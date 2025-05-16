using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyWallController : MonoBehaviour
{
    private GameController gameController; // Reference to GameController
    GameObject gameControllerObject;

    int greenAmount = 0;
    public byte changeColorAmount = 6;
    float timeToIncreaseInfection = 0;
    SpriteRenderer sprite;
    bool isInfected = false;
    bool isCollidingWithPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(255, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollidingWithPlayer) // Only change color while colliding with the player
        {
            timeToIncreaseInfection += Time.deltaTime;

            if (timeToIncreaseInfection > 0.0175f) // Change color at intervals
            {
                Color32 newColor = sprite.color;

                if (newColor.r - changeColorAmount >= 0)
                    newColor.r -= changeColorAmount;
                else
                {
                    newColor.r = 0;
                }
                if (newColor.g + changeColorAmount <= 255)
                    newColor.g += changeColorAmount;
                else
                {
                    newColor.g = 255;
                }

                if (newColor.g == 255 && !isInfected)
                {
                    isInfected = true;
                    gameController.GotInfection();
                }

                sprite.color = newColor;
                timeToIncreaseInfection = 0; // Reset timer
            }
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("InfectionRadius") && !isInfected)
        {
            isCollidingWithPlayer = true;
            Debug.Log("Infection Col");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("InfectionRadius"))
        {
            isCollidingWithPlayer = false;
        }
    }
}