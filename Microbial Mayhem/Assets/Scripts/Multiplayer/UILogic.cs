using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UILogic : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public TMPro.TMP_InputField ipInputField; // Optional

    public GameObject hostObject;
    public GameObject clientObject;
    public GameObject inputFieldObject;
    public GameObject player1Status;
    public GameObject player2Status;

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        hostObject.SetActive(false);
        clientObject.SetActive(false);
        inputFieldObject.SetActive(false);
        player1Status.SetActive(true);
        player2Status.SetActive(true);
        Debug.Log("Host started.");
    }

    void StartClient()
    {
        string ip = ipInputField != null ? ipInputField.text : "127.0.0.1";

        // Set the IP address if using UnityTransport
        Unity.Netcode.Transports.UTP.UnityTransport transport =
            (Unity.Netcode.Transports.UTP.UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.SetConnectionData(ip, 7777); // Port can be customized

        NetworkManager.Singleton.StartClient();
        hostObject.SetActive(false);
        clientObject.SetActive(false);
        inputFieldObject.SetActive(false);
        player1Status.SetActive(true);
        player2Status.SetActive(true);
        Debug.Log("Client connecting to: " + ip);
    }
}
