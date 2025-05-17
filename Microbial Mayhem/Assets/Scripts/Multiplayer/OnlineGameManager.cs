using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OnlineGameManager : NetworkBehaviour
{
    public int totalWalls = 0;
    private int player0Walls = 0;
    private int player1Walls = 0;

    private UIManager uiManager;

    void Start()
    {
        totalWalls = GameObject.FindGameObjectsWithTag("Wall").Length; 
        uiManager = FindObjectOfType<UIManager>();
    }

    public void GotInfection(ulong clientId)
    {
        if (!IsServer) return;

        if (clientId == 0) player0Walls++;
        else if (clientId == 1) player1Walls++;

        // Update both clients
        uiManager.UpdateInfectionPercentagesClientRpc(player0Walls, player1Walls, totalWalls);
    }
}
