using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    private PlayerController playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //playerMovement.SetInput(moveInput);
    }
}