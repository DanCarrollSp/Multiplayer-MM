using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroyDataScript : MonoBehaviour
{
    public static DontDestroyDataScript Instance;
    public int gamesPlayed = 1;

    public bool BTesting = false;
    void Awake()
    {
        //BTesting = Random.value < 0.5f;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe to scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);//Prevents duplicates
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    private void OnDestroy()
    {
        // Unsubscribe when object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
    }
}
