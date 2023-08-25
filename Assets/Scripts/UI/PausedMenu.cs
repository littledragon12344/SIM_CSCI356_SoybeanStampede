using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private static bool Pause;     //game state 

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        //Note to Gun 
        //if (PausedMenu.pause==true) return;// so that it doesnt shoot when paused
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = !Pause; //false become true
            if (Pause)
                PauseGame(); // pause game
            else
                ResumGame(); // resume game
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;//pause the game
        Pause = true;
    }

    public void ResumGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;//resume the game
        Pause = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Test()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
