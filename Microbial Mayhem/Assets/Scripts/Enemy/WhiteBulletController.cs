// WhiteBulletController.cs
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class WhiteBulletController : NetworkBehaviour
{
    public float lifeTime = 10f;
    private NetworkObject netObj;

    private void Awake() => netObj = GetComponent<NetworkObject>();

    private void Start()
    {
        if (IsServer)
            Invoke(nameof(DestroySelf), lifeTime);
    }

    private void DestroySelf()
    {
        if (IsServer && netObj.IsSpawned)
            netObj.Despawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        var playerController = other.GetComponent<PlayerControllerOnline>()
                            ?? other.GetComponentInParent<PlayerControllerOnline>();
        if (playerController != null)
        {
            Debug.Log($"[Server] Bullet hit {playerController.name} (client {playerController.OwnerClientId})");

            // Shrink the *server’s* copy of that player:
            playerController.ShrinkOnServerRpc(
              0.1f,
              new ServerRpcParams
              {
                  // not strictly needed here, but left for clarity
                  //Send = new ServerRpcSendParams { TargetClientIds = new[] { playerController.OwnerClientId } }
              }
            );

            netObj.Despawn();
        }
        else if (other.CompareTag("Wall"))
        {
            netObj.Despawn();
        }
    }
}
