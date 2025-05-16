using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro

public class NextLevelBigToe : MonoBehaviour
{
    private GameController gameController;
    private TextMeshProUGUI nextLevelText;
    private bool levelTriggered = false; // Prevent multiple triggers

    void Start()
    {
        gameController = FindObjectOfType<GameController>(); // Find GameController in the scene

        GameObject nextLevelObj = GameObject.FindGameObjectWithTag("NextLevel");
        if (nextLevelObj != null)
        {
            nextLevelText = nextLevelObj.GetComponent<TextMeshProUGUI>();
            nextLevelText.enabled = false; // Ensure text is initially hidden
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'NextLevel' found!");
        }
    }

    void Update()
    {
        // Only trigger when infectionCount >= wallCount and it hasn't been triggered yet
        if ((!levelTriggered && gameController != null && gameController.infectionCount >= gameController.wallCount) || Input.GetKeyDown(KeyCode.G))
        {
            levelTriggered = true; // Prevent multiple triggers
            StartCoroutine(ShowTextAndLoadScene());
        }
    }

    IEnumerator ShowTextAndLoadScene()
    {
        if (nextLevelText != null)
        {
            nextLevelText.enabled = true; // Show text only now
        }

        yield return new WaitForSeconds(3f); // Wait for 5 seconds

        SceneManager.LoadScene("BigToe"); // Load new scene
    }
}
