using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }
    private List<GameObject> copiables = new List<GameObject>();
    public List<GameObject> Copiables { get { return copiables; } }
    private int playerHealthMax = 100;
    public int playerHealth { get { return playerHealthMax; } }
    private int playerPoints;
    public int PlayerPoints { get { return playerHealthMax; } }

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
}
