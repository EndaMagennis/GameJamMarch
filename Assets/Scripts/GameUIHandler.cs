using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIHandler : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject deathScreen;

    private void Awake()
    {
        PauseMenu.SetActive(false);
        deathScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && Time.timeScale != 0)
        {
            PauseGame();
        }
    }
    public void QuitToMain()
    {
        PauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void DeathScreen()
    {
        Time.timeScale = 0;
        deathScreen.SetActive(true);
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void StartGame()
    {
        deathScreen.SetActive(false);
        PauseMenu.SetActive(false);
        SceneManager.LoadScene(1);
    }

}
