using Unity.Netcode;
using UnityEngine;
using TMPro;

public class LobbyManager : NetworkBehaviour
{
    public TMP_Text player1StatusText;
    public TMP_Text player2StatusText;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client connected: " + clientId);
        UpdateLobbyUIClientRpc(NetworkManager.ConnectedClientsList.Count);
    }

    void OnClientDisconnected(ulong clientId)
    {
        Debug.Log("Client disconnected: " + clientId);
        UpdateLobbyUIClientRpc(NetworkManager.ConnectedClientsList.Count);
    }

    [ClientRpc]
    void UpdateLobbyUIClientRpc(int clientCount)
    {
        if (clientCount >= 1)
        {
            player1StatusText.text = "Player 1: Ready";
        }
        else
        {
            player1StatusText.text = "Player 1: Not connected";
        }

        if (clientCount >= 2)
        {
            player2StatusText.text = "Player 2: Connected";
        }
        else
        {
            player2StatusText.text = "Player 2: Waiting...";
        }
    }
}
