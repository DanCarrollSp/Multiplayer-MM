using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class AnalyticsManager : MonoBehaviour
{
    public Button speed;
    public Button infectionRate;
    public Button infectionRadius;
    public Button CollapseButton;
    public Button ExpandButton;

    public LevelingController levelingController;

    public Button restart;
    public Button menu;

    private GameObject gameControllerObject;
    public ScalingBehaviour scaling;

    class Payload
    {
        public string key;
        public string deviceID;
        public string date;
        public string sceneName;
        public int currentGame;
        public float timeElapsed;
        public int upgradeAmount;
        public int speedUpgrade;
        public int infectionRateUpgrade;
        public int infectionRadiusUpgrade;
        public int UpgradeSystemExpandedOrCollapsed;
        public int UpgradeSystemInteractions;
        public int InfectionPercentage;
        public bool BTesting;
    }

    private int UpgradeSystemExpandedOrCollapsed = 0;
    private int UpgradeSystemInteractions = 0;
    private float timeElapsed = 0;
    private float sendTimer = 0;
    
    private void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameController");
        GameController.OnInfection += SendDataPayload;
        //Upgrade buttons
        if (speed != null)
        {
            speed.onClick.AddListener(() =>
            {
                UpgradeSystemInteractions++;
                SendDataPayload();
                Debug.Log("Speed button pressed!");
            });
        }
        if (infectionRate != null)
        {
            infectionRate.onClick.AddListener(() =>
            {
                UpgradeSystemInteractions++;
                SendDataPayload();
                Debug.Log("Infection Rate button pressed!");
            });
        }
        if(infectionRadius != null)
        {
            infectionRadius.onClick.AddListener(() =>
            {
                UpgradeSystemInteractions++;
                SendDataPayload();
                Debug.Log("Infection Radius button pressed!");
            });

        }

        if (CollapseButton != null)
        {
            CollapseButton.onClick.AddListener(() =>
            {
                UpgradeSystemInteractions++;
                UpgradeSystemExpandedOrCollapsed++;
                SendDataPayload();
            });
        }

        if (ExpandButton != null)
        {
            ExpandButton.onClick.AddListener(() =>
            {
                UpgradeSystemInteractions++;
                UpgradeSystemExpandedOrCollapsed++;
                SendDataPayload();
            });
        }

        //Menu and Restart buttons
        if (restart != null)
        {
            restart.onClick.AddListener(() =>
            {
                SendDataPayload();
                Debug.Log("Restart button pressed!");
            });
        }
        if (menu != null)
        {
            menu.onClick.AddListener(() =>
            {
                SendDataPayload();
                Debug.Log("Menu button pressed!");
            });
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        sendTimer += Time.deltaTime;

        // Send data every 15 seconds
        if (sendTimer >= 15f)
        {
            SendDataPayload();
            sendTimer = 0f; // Reset the timer after sending data
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendDataPayload();
        }
    }


    public void SendDataPayload()
    {
        Payload payload = new Payload
        {
            key = "my_unique_key",
            deviceID = SystemInfo.deviceUniqueIdentifier,
            date = DateTime.Now.ToString(),
            sceneName = SceneManager.GetActiveScene().name,
            currentGame = DontDestroyDataScript.Instance.gamesPlayed,
            timeElapsed = timeElapsed,
            upgradeAmount = levelingController.levelsUsed,
            speedUpgrade = levelingController.MovementSpeedLevel,
            infectionRateUpgrade = levelingController.InfectionRateLevel,
            infectionRadiusUpgrade = levelingController.InfectionRadiusLevel,
            UpgradeSystemExpandedOrCollapsed = UpgradeSystemExpandedOrCollapsed,
            UpgradeSystemInteractions = UpgradeSystemInteractions,
            InfectionPercentage = (int)scaling.GetInfectionPercentage(),
            BTesting = DontDestroyDataScript.Instance.BTesting
        };

        string jsonData = JsonUtility.ToJson(payload);
        Debug.Log("Sending JSON data: " + jsonData);

        StartCoroutine(PostData(jsonData));
    }

    IEnumerator PostData(string jsonData)
    {
        string url = "https://compucore.itcarlow.ie/Microbial_Mayhem_Analytics/upload_data"; // UPDATED URL
        //string url = "http://localhost:5000/upload_data";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbPOST; // Explicitly setting POST method

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data successfully sent to server: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error sending data to server. HTTP Status: " + request.responseCode);
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }
    private void OnDestroy()
    {
        GameController.OnInfection -= SendDataPayload;
    }
}
