using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScalingBehaviour : MonoBehaviour
{
    private GameController gameController; // Reference to GameController
    public GameObject percentageText;
    private TextMeshProUGUI percentage;
    private GameObject gameControllerObject;
    public RectTransform imageTransform; // Reference to the UI Image's RectTransform
    public float minWidth = 0f;  // Minimum width (0)
    public float maxWidth = 500f; // Maximum width (500)
    private float widthPerWall = 0;
    private bool scaleFound = false;
    private int currentInfection = 0;
    bool showTowers = false;
    public float infectionPercentage = 0;

    void Start()
    {
        // Find GameController GameObject using its tag
        gameControllerObject = GameObject.FindWithTag("GameController");
        percentage = percentageText.GetComponent<TextMeshProUGUI>();

        if (imageTransform == null)
        {
            Debug.LogError("Image RectTransform is not assigned!");
        }
    }

    private void Update()
    {
        if (!scaleFound)
            FindScale();

        if (gameController != null && currentInfection != gameController.infectionCount)
        {
            float newWidth = widthPerWall * gameController.infectionCount;
            imageTransform.sizeDelta = new Vector2(newWidth, imageTransform.sizeDelta.y);

            currentInfection = gameController.infectionCount;
            infectionPercentage = (newWidth / maxWidth) * 100;
            if (infectionPercentage >= 25 && !showTowers)
            {
                showTowers=true;
                EventManager.Instance.RaiseEvent("ShowTowerCells", 1);
            }
            percentage.text = "Infection Percentage: " + infectionPercentage.ToString("F2") + "%";
        }
    }

    // Function to calculate width per wall
    float CalculateWidthPerWall(int numberOfWalls, float minWidth, float maxWidth)
    {
        return (maxWidth - minWidth) / numberOfWalls;
    }

    void FindScale()
    {
        if (gameControllerObject != null)
        {
            // Access the GameController script attached to the GameController GameObject
            gameController = gameControllerObject.GetComponent<GameController>();

            if (gameController != null)
            {
                int numberOfWalls = gameController.wallCount; // Access wall count from GameController
                if (numberOfWalls > 0)
                {
                    scaleFound = true;

                    // Calculate how much 1 wall contributes to width
                    widthPerWall = CalculateWidthPerWall(numberOfWalls, minWidth, maxWidth);

                    // Output the result
                    Debug.Log("Width per wall: " + widthPerWall);
                }
            }
        }
    }
    public float GetInfectionPercentage()
    {
        return infectionPercentage;
    }
}
