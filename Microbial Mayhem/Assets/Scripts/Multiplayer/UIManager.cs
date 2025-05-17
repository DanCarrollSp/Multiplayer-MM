using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIManager : NetworkBehaviour
{
    public TMP_Text player1Text;
    public TMP_Text player2Text;

    private void Start()
    {
        UpdateInfectionPercentagesClientRpc(0, 0, 16);
    }
    // Called by the server
    [ClientRpc]
    public void UpdateInfectionPercentagesClientRpc(int player1Walls, int player2Walls, int totalWalls)
    {
        float percent1 = totalWalls > 0 ? (player1Walls * 100f / totalWalls) : 0;
        float percent2 = totalWalls > 0 ? (player2Walls * 100f / totalWalls) : 0;

        player1Text.text = $"Player 1: {percent1:F1}%";
        player2Text.text = $"Player 2: {percent2:F1}%";
    }
}
