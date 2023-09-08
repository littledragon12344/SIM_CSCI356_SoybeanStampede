using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameStateContoller : MonoBehaviour
{
    public float Timer = 0; //in game timer
    private bool EndOfGame; //game state
    public int Kills;       //kills

    //gamestate
    public GameObject pauseMenu;
    public GameObject GameOverScreen;
    private static bool Pause;     //game state 
    public TextMeshProUGUI FinalScore;

    void Start()
    {
        pauseMenu.SetActive(false);
        //Note to Gun 
        //if (PausedMenu.pause==true) return;// so that it doesnt shoot when paused
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu.activeInHierarchy || GameOverScreen.activeInHierarchy)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = !Pause; //false become true
            if (Pause)
                PauseGame(); // pause game
            else
                ResumGame(); // resume game
        }
    }

    public void ScoreUpdate()
    {
        Kills++;// Plus score
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

    public void GameOver()
    {
        pauseMenu.SetActive(false);
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;//pause the game
        Pause = true;

        FinalScore.text = "You died,Final Score :" + Kills.ToString();
    }


    public void Survived()
    {
        pauseMenu.SetActive(false);
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;//pause the game
        Pause = true;

        FinalScore.text = "Game Cleared,Final Score :" + Kills.ToString();
    }
}
