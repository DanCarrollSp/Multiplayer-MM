using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionRadius : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Player.transform.position;
    }
}
