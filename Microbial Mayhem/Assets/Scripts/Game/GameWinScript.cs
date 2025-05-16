using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinScript : MonoBehaviour
{
    public ScalingBehaviour scalingBehaviour;
    public GameObject GameWinPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(scalingBehaviour.GetInfectionPercentage() >= 100)
        {
            GameWinPanel.SetActive(true);
        }
    }
}
