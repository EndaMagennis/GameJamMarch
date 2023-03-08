using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance;
    public AudioSource masterAudio;
    public AudioSource music;
    public AudioClip menuMusic;
    public AudioClip mainGameMusic;
    public AudioClip playerJump;
    public AudioClip playerShapeshift;
    public AudioClip enemyAttack;
    public AudioClip enemySpawn;
    public AudioClip enemyNear;
    public AudioClip coinPing;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
    }

    private void Start()
    {
        PlaySceneMusic(SceneManager.GetActiveScene());
    }
    public void PlaySceneMusic(Scene level)
    {
       if(level.name == "Main Menu")
       {
            music.Stop();   
            music.clip = menuMusic;
            music.Play();
       }
       else if(level.name == "Game")
        {
            music.Stop();
            music.clip = mainGameMusic;
            music.Play();
        }
    }

}
