using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void GoToGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Ai_Spawn_test");//Currently it goes to ai spawn test
    }
    public void HowToMenu()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
   
