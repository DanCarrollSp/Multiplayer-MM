using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class BodyWallController : NetworkBehaviour
{
    private GameController gameController;
    GameObject gameControllerObject;

    public byte changeColorAmount = 6;
    float timeToIncreaseInfection = 0;
    private ulong infectingClientId = ulong.MaxValue; // Invalid by default

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
                byte r = currentColor.r;
                byte g = currentColor.g;
                byte b = currentColor.b;

                Debug.Log(infectingClientId + " Update");
                if (infectingClientId == 0)
                {
                    // Remove blue first
                    if (b > 0)
                    {
                        b = b > changeColorAmount ? (byte)(b - changeColorAmount) : (byte)0;
                    }
                    else
                    {
                        // Once blue is gone, apply green infection
                        r = r > changeColorAmount ? (byte)(r - changeColorAmount) : (byte)0;
                        g = g < 255 - changeColorAmount ? (byte)(g + changeColorAmount) : (byte)255;
                    }
                }
                else if (infectingClientId == 1)
                {
                    // Remove green first
                    if (g > 0)
                    {
                        g = g > changeColorAmount ? (byte)(g - changeColorAmount) : (byte)0;
                    }
                    else
                    {
                        // Once green is gone, apply blue infection
                        r = r > changeColorAmount ? (byte)(r - changeColorAmount) : (byte)0;
                        b = b < 255 - changeColorAmount ? (byte)(b + changeColorAmount) : (byte)255;
                    }
                }

                Color32 newColor = new Color32(r, g, b, 255);
                syncedColor.Value = newColor;

                // Infection complete when full saturation of active color
                if (((infectingClientId == 0 && g == 255) || (infectingClientId == 1 && b == 255)) && !isInfected.Value)
                {
                    isInfected.Value = true;
                    gameController.GotInfection();
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

            NetworkObject networkObject = other.GetComponentInParent<NetworkObject>();
            if (networkObject != null)
            {
                infectingClientId = networkObject.OwnerClientId;
                Debug.Log(infectingClientId + " Enter");
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("InfectionRadius"))
        {
            isCollidingWithPlayer = false;
            Debug.Log(infectingClientId + " Exit");
            infectingClientId = ulong.MaxValue; // Reset
        }
    }

}
