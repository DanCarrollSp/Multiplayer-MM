using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class OnlineGameManager : NetworkBehaviour
{
    public float showDelay = 3f;

    public string nextSceneName;

    private int totalWalls;
    private int player0Walls;
    private int player1Walls;
    private bool isWaitingForLoad = false;

    void Start()
    {
        if (!IsServer) return;
        InitSceneState();
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;
    }

    private void InitSceneState()
    {
        totalWalls = GameObject.FindGameObjectsWithTag("Wall").Length;
        player0Walls = player1Walls = 0;
        // send an initial reset to UI
        var ui = FindObjectOfType<UIManager>();
        ui.UpdateInfectionPercentagesClientRpc(player0Walls, player1Walls, totalWalls);
        isWaitingForLoad = false;
    }

    public void GotInfection(ulong clientId)
    {
        if (!IsServer) return;

        if (clientId == 0) player0Walls++;
        else if (clientId == 1) player1Walls++;

        // push update to UI
        var ui = FindObjectOfType<UIManager>();
        ui.UpdateInfectionPercentagesClientRpc(player0Walls, player1Walls, totalWalls);

        if (player0Walls + player1Walls >= totalWalls && !isWaitingForLoad)
        {
            isWaitingForLoad = true;

            // 1) determine winner text
            string result;
            if (player0Walls > player1Walls) result = "Player 1 Wins!";
            else if (player1Walls > player0Walls) result = "Player 2 Wins!";
            else result = "Draw!";

            // 2) tell all clients to show it
            ShowNextLevelClientRpc(result);

            // 3) host will despawn and load next level after delay
            StartCoroutine(DelayLoadNextScene());
        }
    }

    [ClientRpc]
    void ShowNextLevelClientRpc(string resultText)
    {
        var nl = FindObjectOfType<NextLevel>();
        if (nl != null)
            nl.TriggerNextLevelUI(resultText);
    }

    IEnumerator DelayLoadNextScene()
    {
        // despawn old players
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var oldObj = client.PlayerObject;
            if (oldObj != null)
                oldObj.Despawn(true);
        }

        yield return new WaitForSeconds(showDelay);

        // host initiates the scene load
        NetworkManager.Singleton.SceneManager.LoadScene(
            nextSceneName,
            LoadSceneMode.Single
        );
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode mode)
    {
        if (!IsServer || sceneName != nextSceneName) return;

        // spawn fresh player for this client
        var prefab = NetworkManager.Singleton.NetworkConfig.PlayerPrefab;
        var go = Instantiate(prefab);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        // re-init on first local callback
        if (clientId == NetworkManager.Singleton.LocalClientId)
            InitSceneState();
    }
}
