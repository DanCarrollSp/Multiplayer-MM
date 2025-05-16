using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDesorbing : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<SizeManager>().DecreaseSpriteSize(); // Decrease player's sprite size
            //Destroy(gameObject); // Destroy the White Cell
        }
    }
}
