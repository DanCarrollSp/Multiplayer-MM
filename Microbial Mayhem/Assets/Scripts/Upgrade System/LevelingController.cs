using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelingController : MonoBehaviour
{
    public int levelsUsed = 0;
    private int level = 1;
    private bool upgradeAvailalble = false;

    public Button SpeedButton;
    public Button InfectionRateButton;

    public GameObject InfectionRateObject;

    public int MovementSpeedLevel = 0;
    public TextMeshProUGUI movementSpeedLevelAmount;
    public PlayerController playerController;

    public int InfectionRateLevel = 0;
    public TextMeshProUGUI infectionRateLevelAmount;
    private GameObject[] Walls;

    public TextMeshProUGUI levelText;
    public GameObject upgradeAvailableGameObject;
    public TextMeshProUGUI UpgradesAvailableAmountText;

    public int InfectionRadiusLevel = 0;
    public TextMeshProUGUI infectionRadiusLevelAmount;
    public Button InfectionRadiusButton;
    public Transform InfectionRadiusTransfrom;

    public AnalyticsManager AnalyticsManager;

    // Start is called before the first frame update
    void Start()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfUpgradeAvailable();
        UpgradesAvailableAmountText.text = (level - levelsUsed).ToString();
    }

    public void LevelUpdate(int newLevel)
    {
        level = newLevel;
        levelText.text = level.ToString();
        
    }

    void CheckIfUpgradeAvailable()
    {
        if (level > levelsUsed)
        {
            SpeedButton.enabled = true;
            InfectionRateButton.enabled = true;
            InfectionRadiusButton.enabled = true;
            upgradeAvailalble = true;
            upgradeAvailableGameObject.SetActive(true);
            
        }
        else
        {
            SpeedButton.enabled = false;
            InfectionRateButton.enabled = false;
            InfectionRadiusButton.enabled = false;
            upgradeAvailalble = false;
            upgradeAvailableGameObject.SetActive(false);
        }
    }

    //public void AddMovementSpeedLevel()
    //{
    //    if(level > levelsUsed && MovementSpeedLevel <= 10)
    //    {
    //        MovementSpeedLevel++;
    //        levelsUsed++;
    //        movementSpeedLevelAmount.text = MovementSpeedLevel.ToString() + "/10";

    //        playerController.speed += 1;
    //        playerController.maxSpeed += 1;
    //    }
    //}
    //public void AddInfectionRateLevel()
    //{
    //    if(level > levelsUsed && InfectionRateLevel <= 10)
    //    {
    //        InfectionRateLevel++;
    //        levelsUsed++;
    //        infectionRateLevelAmount.text = InfectionRateLevel.ToString() + "/10";

    //        foreach(GameObject wall in Walls)
    //        {
    //            wall.GetComponent<BodyWallController>().changeColorAmount++;
    //        }
    //    }
    //}
    //public void AddInfectionRadiusLevel()
    //{
    //    if (level > levelsUsed && InfectionRateLevel <= 10)
    //    {
    //        InfectionRadiusLevel++;
    //        levelsUsed++;
    //        infectionRadiusLevelAmount.text = InfectionRadiusLevel.ToString() + "/10";
    //        Vector3 scale = InfectionRadiusTransfrom.localScale;
    //        scale.x = scale.x + 0.1f;
    //        scale.y = scale.y + 0.1f;
    //        InfectionRadiusTransfrom.localScale = scale;
    //    }

    //}
    public void AddInfectionRadiusUpgrade()
    {
        InfectionRadiusLevel++;
        Vector3 scale = InfectionRadiusTransfrom.localScale;
        scale.x = scale.x + 0.1f;
        scale.y = scale.y + 0.1f;
        InfectionRadiusTransfrom.localScale = scale;
        AnalyticsManager.SendDataPayload();
    }
    public void AddInfectionRateUpgrade()
    {
        InfectionRateLevel++;
        foreach (GameObject wall in Walls)
        {
            wall.GetComponent<BodyWallController>().changeColorAmount++;
        }
        AnalyticsManager.SendDataPayload();
    }
    public void AddMovementSpeedUpgrade()
    {
        MovementSpeedLevel++;
        playerController.speed += 1;
        playerController.maxSpeed += 1;
        AnalyticsManager.SendDataPayload();
    }
}
