using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class XPScalingBehaviour : MonoBehaviour
{
    private GameObject UIController;
    private GameController gameController; // Reference to GameController
    private GameObject gameControllerObject;
    public RectTransform imageTransform; // Reference to the UI Image's RectTransform
    public float minWidth = 0f;  // Minimum width (0)
    public float maxWidth = 400f; // Maximum width (500)
    private float widthPerRedBloodCell = 0;
    private bool scaleFound = false;
    private int redBloodCell = 0;
    private int maxRedBloodCellAmount = 3;
    private int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        UIController = GameObject.FindWithTag("UIController");
        gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (redBloodCell >= maxRedBloodCellAmount)
        {
            maxRedBloodCellAmount = GetMaxRedBloodCellAmount();
            level++;
            imageTransform.sizeDelta = new Vector2(minWidth, imageTransform.sizeDelta.y);
            redBloodCell = 0;
            scaleFound = false;
            UIController.GetComponent<LevelingController>().LevelUpdate(level);
            Debug.Log("RedBloodCell Reset");
        }

        if (!scaleFound)
           { FindScale();
            Debug.Log("Redblood Cell Scale Reset");
        }
 
    }

    float CalculateWidthPerRedBloodCell(int redBloodCellMax, float minWidth, float maxWidth)
    {
        return (maxWidth - minWidth) / redBloodCellMax;
    }

    void FindScale()
    {
        if (gameControllerObject != null)
        {
            // Access the GameController script attached to the GameController GameObject
            gameController = gameControllerObject.GetComponent<GameController>();

            if (gameController != null)
            {
                   scaleFound = true;

                   // Calculate how much 1 wall contributes to width
                   widthPerRedBloodCell = CalculateWidthPerRedBloodCell(maxRedBloodCellAmount, minWidth, maxWidth);

                   // Output the result
                   Debug.Log("Width per redbloodcell: " + widthPerRedBloodCell);
            }
        }
    }

    int GetMaxRedBloodCellAmount()
    {
        return Mathf.RoundToInt(maxRedBloodCellAmount * (1 + (level * 0.2f)));
    }

    public void AddRedBloodCell()
    {
        EventManager.Instance.RaiseEvent("AddExperience", 500);
        redBloodCell++;
        imageTransform.sizeDelta += new Vector2(widthPerRedBloodCell, 0);
    }
}
