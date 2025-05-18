using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class NextLevel : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nextLevelText;
    public GameObject loadingScenePanel;

    [Header("Scene")]
    public string nextScene;

    bool levelTriggered = false;
    string pendingResult = "";

    void Awake()
    {
        if (nextLevelText == null)
        {
            var obj = GameObject.FindGameObjectWithTag("NextLevel");
            if (obj != null) nextLevelText = obj.GetComponent<TextMeshProUGUI>();
        }
        nextLevelText.enabled = false;
        loadingScenePanel.SetActive(false);
    }

    // Called by the ClientRpc, now with text
    public void TriggerNextLevelUI(string resultText)
    {
        if (levelTriggered) return;
        levelTriggered = true;

        pendingResult = resultText;
        nextLevelText.text = pendingResult;
        nextLevelText.enabled = true;

        StartCoroutine(ShowLoadingAndLoadScene());
    }

    IEnumerator ShowLoadingAndLoadScene()
    {
        yield return new WaitForSeconds(2f);
        loadingScenePanel.SetActive(true);
        yield return new WaitForSeconds(2f);

        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                nextScene,
                UnityEngine.SceneManagement.LoadSceneMode.Single
            );
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TriggerNextLevelUI("DEBUG: Forced Win");
    }
}
