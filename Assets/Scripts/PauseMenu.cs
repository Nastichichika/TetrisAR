using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPauseCustom = false;

    [SerializeField]private GameObject menuPause;

    public void Awake() {
        menuPause.SetActive(false);
    }
    public void PauseGame() {
        menuPause.SetActive(true);
        Time.timeScale = 0f;
        isPauseCustom = true;
    }

    public void ResumeGame()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1f; 
        isPauseCustom = false;
    }

    public void RestartGame()
    {
        ARManager.instance.gameStart = false;
        GameManager.instance.SetGameIsStart();
        isPauseCustom = false;
        ARSession[] ARSessionScript = FindObjectsOfType<ARSession>();
        for (var i = 0; i < ARSessionScript.Length; ++i)
        {
            ARSessionScript[i].Reset();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void CustomToMenu()
    {
        ARManager.instance.gameStart = false;
        GameManager.instance.SetGameIsStart();
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
