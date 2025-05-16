using UnityEngine;
using TMPro;

public class TipScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private float timer = 0f;
    public float duration = 8f;         // Total animation time in seconds

    public float minFontSize = 33f;
    public float maxFontSize = 43f;
    public float oscillationSpeed = 2f; // Adjust this to control how fast it goes from 33 to 43

    void Start()
    {
        //textMeshPro = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        // Increase timer by the time elapsed since last frame
        timer += Time.deltaTime;

        if (timer < duration * oscillationSpeed)
        {
            // Mathf.PingPong returns a value that oscillates between 0 and (maxFontSize - minFontSize)
            float newSize = minFontSize + Mathf.PingPong(timer * oscillationSpeed * (maxFontSize - minFontSize), maxFontSize - minFontSize);
            textMeshPro.fontSize = newSize;
        }
        else
        {
            // After 8 seconds, disable the GameObject so it disappears from the screen
            textMeshPro.gameObject.SetActive(false);
        }
    }
}
