using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject[] walls;
    public int wallCount;
    public int infectionCount = 0;

    public static event Action OnInfection;


    // Start is called before the first frame update
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall"); 
        wallCount = walls.Length;
        Debug.Log("Total Walls: " + wallCount);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void GotInfection()
    {  
        infectionCount++;

        OnInfection?.Invoke();
    }
}
