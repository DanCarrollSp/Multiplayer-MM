using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BodyWallController : NetworkBehaviour
{
    private GameController gameController;
    GameObject gameControllerObject;

    public byte changeColorAmount = 6;
    float timeToIncreaseInfection = 0;

    private SpriteRenderer sprite;
    private bool isCollidingWithPlayer = false;

    private NetworkVariable<Color32> syncedColor = new NetworkVariable<Color32>(
        new Color32(255, 0, 0, 255),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private NetworkVariable<bool> isInfected = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        sprite = GetComponent<SpriteRenderer>();

        // Apply current color in case this is a client joining later
        sprite.color = syncedColor.Value;

        // Subscribe to updates so color changes in real time
        syncedColor.OnValueChanged += (oldColor, newColor) =>
        {
            sprite.color = newColor;
            Debug.Log($"[Client] Updated wall color to {newColor}");
        };
    }


    void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    void Update()
    {
        if (!IsServer) return; // Only server processes the infection

        if (isCollidingWithPlayer && !isInfected.Value)
        {
            timeToIncreaseInfection += Time.deltaTime;

            if (timeToIncreaseInfection > 0.0175f)
            {
                Color32 currentColor = syncedColor.Value;

                byte r = currentColor.r > changeColorAmount ? (byte)(currentColor.r - changeColorAmount) : (byte)0;
                byte g = currentColor.g < 255 - changeColorAmount ? (byte)(currentColor.g + changeColorAmount) : (byte)255;

                Color32 newColor = new Color32(r, g, 0, 255);
                syncedColor.Value = newColor;

                if (g == 255 && !isInfected.Value)
                {
                    isInfected.Value = true;
                    gameController.GotInfection(); // Still a local call — ensure GC is network-safe if needed
                }

                timeToIncreaseInfection = 0;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("InfectionRadius") && !isInfected.Value)
        {
            isCollidingWithPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("InfectionRadius"))
        {
            isCollidingWithPlayer = false;
        }
    }
}
