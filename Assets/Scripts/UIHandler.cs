using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public int playerScore;
    public GameObject MainMenu;


    private void Awake()
    {
        MainMenu.SetActive(true);
    }
    
    public void StartGame()
    {
        Time.timeScale = 1;
        MainMenu.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
