using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCustom : MonoBehaviour
{
    public void StartTheGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitFromGame()
    {
        Application.Quit();
    }
}
