using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class CustomPlayerSpawner : NetworkBehaviour
{
    public GameObject player0Prefab;
    public GameObject player1Prefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            NetworkManager.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        // Wait for scene to load before spawning players
        if (SceneManager.GetActiveScene().name == "Multiplayer")
        {
            SpawnPlayerForClient(clientId);
        }
        else
        {
            // Optional: wait for scene load, then spawn
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.name == "Multiplayer")
                {
                    SpawnPlayerForClient(clientId);
                }
            };
        }
    }

    private Vector3 GetSpawnPosition(ulong clientId)
    {
        return clientId == 0 ? new Vector3(-3f, 0f, 0f) : new Vector3(3f, 0f, 0f);
    }

    private void SpawnPlayerForClient(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            GameObject prefab = clientId == 0 ? player0Prefab : player1Prefab;
            Vector3 spawnPos = clientId == 0 ? new Vector3(-3, 0, 0) : new Vector3(3, 0, 0);

            GameObject player = Instantiate(prefab, spawnPos, Quaternion.identity);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }

    private void OnDestroy()
    {
        if (IsServer)
            NetworkManager.OnClientConnectedCallback -= HandleClientConnected;
    }
}
