using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
