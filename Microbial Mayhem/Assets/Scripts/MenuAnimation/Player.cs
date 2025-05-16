using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayerMovementRoutine());
    }

    IEnumerator PlayerMovementRoutine()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Move to the right for 5 seconds
        float moveSpeed = 500f; // Adjust speed as needed
        float moveTime = 5f;
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + Vector3.right * 4400f; // Example movement to the right

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for 3 seconds again
        yield return new WaitForSeconds(5f);

        // Move to the left for 5 seconds
        elapsedTime = 0f;
        initialPosition = transform.position;
        targetPosition = initialPosition - Vector3.right * 5500f; // Example movement to the left

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the player object after finishing movements
        Destroy(gameObject);
    }
}
