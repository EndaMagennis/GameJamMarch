using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }
    private List<GameObject> copiables = new List<GameObject>();
    public List<GameObject> Copiables { get { return copiables; } }
    private int playerHealthMax = 100;
    public int playerHealth { get { return playerHealthMax; } }
    private int playerPoints = 0;
    public int PlayerPoints { get { return playerPoints; } }
    public int HighScore = 0;
    public bool playerHasShifted = false;

    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoints"));
                instance.Copiables.AddRange(GameObject.FindGameObjectsWithTag("Copiable"));
            }
            return instance;
        }
    }

    public void ModifyHealth(int amount)
    {
        playerHealthMax += amount;
    }

    public void ModifyScore(int amount)
    {
        playerPoints += amount;
    }

    public void ResetPlayerValues()
    {
        playerHealthMax = 100;
        playerPoints = 0;
    }

    [System.Serializable]
    class SaveData
    {
        public int saveScore;
    }

    public void SaveHighScore(int _highScore)
    {
        SaveData data = new SaveData();
        data.saveScore = _highScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScore = data.saveScore;
        }
    }
}
