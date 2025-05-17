// SizeManager.cs
using UnityEngine;

public class SizeManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float size = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        size = transform.localScale.x;
    }

    public void IncreaseSpriteSize(float amount)
    {
        Vector3 s = transform.localScale;
        s.x += amount;
        s.y += amount;
        transform.localScale = s;
        size = s.x;
    }

    public void DecreaseSpriteSize(float amount)
    {
        Vector3 s = transform.localScale;
        s.x -= amount;
        s.y -= amount;
        transform.localScale = s;
        size = s.x;
    }

    public void SetValues(float newSize)
    {
        Vector3 s = transform.localScale;
        s.x = newSize;
        s.y = newSize;
        transform.localScale = s;
        size = newSize;
    }
}
