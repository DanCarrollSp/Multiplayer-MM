using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehaviour
{
    public Vector2 Seek(float speed,Vector2 objectPosition,Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - objectPosition).normalized;

        // Scale by speed
        return direction * speed * Time.deltaTime;
    }
}
