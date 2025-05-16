using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (sceneName == "Menu" || sceneName == SceneManager.GetActiveScene().name)
        {
            DontDestroyDataScript.Instance.gamesPlayed++;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}