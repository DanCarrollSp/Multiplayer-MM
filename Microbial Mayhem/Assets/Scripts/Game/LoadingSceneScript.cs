using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneScript : MonoBehaviour
{

    public TMP_Text LoadingSceneTextBox;
    public TMP_Text NextSceneTextBox;
    public NextLevel NextLevelScript;
    private int dotCount = 0;
    private const int maxDots = 20;

    void Start()
    {
        StartCoroutine(AddDots());
        NextSceneTextBox.text = NextLevelScript.nextScene;
    }

    IEnumerator AddDots()
    {
        while (dotCount < maxDots)
        {
            LoadingSceneTextBox.text += ".";
            dotCount++;
            yield return new WaitForSeconds(0.10f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dotCount == maxDots)
        {
            SceneManager.LoadScene(NextLevelScript.nextScene);
        }

    }
}
