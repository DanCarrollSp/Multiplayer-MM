using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivacyPolicy : MonoBehaviour
{
    public GameObject PrivacyPolicyPanel;
    public List<GameObject> MainMenuObjects;
    private const string PrivacyPolicyKey = "PrivacyPolicyAccepted";

    void Start()
    {
        int accepted = PlayerPrefs.GetInt(PrivacyPolicyKey, 0);
        Debug.Log("PrivacyPolicyAccepted: " + accepted);

        if (accepted == 0)
        {
            PlayerPrefs.SetInt(PrivacyPolicyKey, 0);
            PlayerPrefs.Save();

            PrivacyPolicyPanel.SetActive(true);
            foreach (GameObject obj in MainMenuObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            PrivacyPolicyPanel.SetActive(false);
            foreach (GameObject obj in MainMenuObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    public void OnAccept()
    {
        PlayerPrefs.SetInt(PrivacyPolicyKey, 1);
        PlayerPrefs.Save();
        PrivacyPolicyPanel.SetActive(false);
        foreach (GameObject obj in MainMenuObjects)
        {
            obj.SetActive(true);
        }
    }

    public void OnDecline()
    {
        Application.Quit();
    }

    public void OnViewPolicy()
    {
        PlayerPrefs.DeleteKey(PrivacyPolicyKey);
        Application.OpenURL("https://docs.google.com/document/d/1TqSHbdJGpW_rr8g1pbLU7qgahWvTn6rHET_AO88wP1M/edit?usp=sharing");
    }
}
