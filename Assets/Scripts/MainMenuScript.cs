using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void Exitgame()
    {
        Application.Quit();
    }
    public void playgame()
    {
        PlayerController.getInstance().PlayGame();
    }
    void Start()
    {
        


    }
    public void PauseGame()
    {
        PlayerController.getInstance().pausegame();
       
    }
    public void ResumeGame()
    {
        PlayerController.getInstance().resumeGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
