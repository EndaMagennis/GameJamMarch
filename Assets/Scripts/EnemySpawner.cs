using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    Vector3 SpawnPosition;
    float XBound = 15f;
    float ZBound = 15f;
    public GameObject mapfloor;
    public AudioSource enemySound;
    public AudioClip enemyClip;
    ThirdPersonController tpc;
    // Start is called before the first frame update
    void Start()
    {
        tpc = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
        Invoke("CreateEnemy", 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    
    public void CreateEnemy()
    {
        Instantiate(enemyPrefab, new Vector3(Random.Range(XBound, -XBound), 0, Random.Range(ZBound, -ZBound)), enemyPrefab.transform.rotation);
        enemySound.PlayOneShot(enemyClip);
        tpc.DisplayMessage("New Enemy on the Map");
    }
   
}
