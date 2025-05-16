using Unity.Netcode;
using UnityEngine;

public class LocalPlayerCamera : NetworkBehaviour
{
    [SerializeField] GameObject camObj; // assign your child Camera

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            camObj.SetActive(true);
    }
}
