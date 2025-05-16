using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCellTowerSpawner : MonoBehaviour
{
    public List<GameObject> towerCells = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.Subscribe("ShowTowerCells", data => ShowTowerCells(data));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTowerCells(int data)
    {
        Debug.Log("Tower Cell");
        bool show = data != 0; // Convert int to bool
        foreach (GameObject towerCell in towerCells)
        {
            towerCell.SetActive(show);
        }
    }

}
