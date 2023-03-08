using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;
    public int playerScore;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject deathScreen;

    [SerializeField] GameObject playerInfoUI;
    [SerializeField] GameObject worldInfoUI;
    [SerializeField] GameObject highScoreUI;
    TextMeshProUGUI playerInfo;
    TextMeshProUGUI worldInfo;
    TextMeshProUGUI highScoreInfo;
    Scene currentScene;

    List<GameObject> checkpoints;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        checkpoints = GameEnvironment.Singleton.Checkpoints;
        GameEnvironment.Singleton.LoadHighScore();
    }

    private void Start()
    {
        Scene level = SceneManager.GetActiveScene();
        if (level.name == "Main Menu")
            MainMenu.SetActive(true);
        PauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        playerInfo = playerInfoUI.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        worldInfo = worldInfoUI.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        highScoreInfo = highScoreUI.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && Time.timeScale != 0)
        {
            PauseGame();
        }
        UpdatePlayerInfo(SceneManager.GetActiveScene());

        CheckHighScore();
        SetHighScore();
    }

    public void StartGame()
    {
        GameEnvironment.Singleton.ResetPlayerValues();
        Time.timeScale = 1;
        deathScreen.SetActive(false);
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitToMain()
    {   
        GameEnvironment.Singleton.SaveHighScore(GameEnvironment.Singleton.HighScore);
        GameEnvironment.Singleton.ResetPlayerValues();
        PauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisplayMessage("");
        SceneManager.LoadScene(0);
        MainMenu.SetActive(true );
    }
    
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    
    public void DeathScreen()
    {
        GameEnvironment.Singleton.SaveHighScore(GameEnvironment.Singleton.HighScore);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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


    public void UpdatePlayerInfo(Scene level)
    {
        if(level.name == "Game")
            playerInfo.text = "Player Health: " + GameEnvironment.Singleton.playerHealth + "\n" + "Player Score: " + GameEnvironment.Singleton.PlayerPoints + "\n";
        else
        {
            playerInfo.text = "";
        }
    }
    public void CheckHighScore()
    {
        if (GameEnvironment.Singleton.PlayerPoints > GameEnvironment.Singleton.HighScore)
        {
            GameEnvironment.Singleton.HighScore = GameEnvironment.Singleton.PlayerPoints;

            GameEnvironment.Singleton.SaveHighScore(GameEnvironment.Singleton.HighScore);
        }
    }

    public void SetHighScore()
    {
        if (GameEnvironment.Singleton.HighScore == 0)
        {
            highScoreInfo.text = "";
        }
        else
        {
            highScoreInfo.text = "High Score: " + GameEnvironment.Singleton.HighScore.ToString();
        }
    }

    public void ResetHighScore()
    {
        GameEnvironment.Singleton.SaveHighScore(0);
        GameEnvironment.Singleton.LoadHighScore();
    }

    public void DisplayMessage(string message)
    {
        worldInfo.text = message;
        StartCoroutine("EraseMessage");
    }

    IEnumerator EraseMessage()
    {
        yield return new WaitForSeconds(2);
        worldInfo.text = "";
    }
}
