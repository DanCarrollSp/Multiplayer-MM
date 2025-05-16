using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float size = 1.0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        size = transform.localScale.x;
    }

    public void IncreaseSpriteSize(float t_increase)
    {
        if (spriteRenderer != null)
        {
            float sizeIncrement = t_increase;

            Vector3 scale = transform.localScale;
            scale.x += sizeIncrement;
            scale.y += sizeIncrement;

            transform.localScale = scale;
            size = transform.localScale.x;
        }
    }

    public void DecreaseSpriteSize(float t_decrease)
    {
        if (spriteRenderer != null)
        {
            float sizeIncrement = t_decrease;

            Vector3 scale = transform.localScale; 
            scale.x -= sizeIncrement; 
            scale.y -= sizeIncrement; 

            transform.localScale = scale;
            size = transform.localScale.x;
        }
    }

    public void SetValues(float size)
    {
        Vector3 scale = transform.localScale;
        scale.x = size;
        scale.y = size;

        transform.localScale = scale;
    }
}
