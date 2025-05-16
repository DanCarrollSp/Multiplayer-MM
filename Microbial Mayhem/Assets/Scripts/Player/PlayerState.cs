using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool isAlive = true;
    public GameObject RestartPanel;
    public void RestartGame()
    {
        RestartPanel.SetActive(true);
    }
}
