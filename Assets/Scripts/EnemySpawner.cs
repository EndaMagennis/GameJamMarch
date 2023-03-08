using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    float XBound = 15f;
    float ZBound = 15f;
    public GameObject mapfloor;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateEnemy", 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    
    public void CreateEnemy()
    {
        Vector3 spawnPos = new Vector3(Random.Range(XBound, -XBound), 1.5f, Random.Range(ZBound, -ZBound));
        if (IsFreeSpace(spawnPos))
        {
            Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
            AudioHandler.Instance.masterAudio.PlayOneShot(AudioHandler.Instance.enemySpawn);
            UIHandler.Instance.DisplayMessage("New Enemy on the Map");
        }
        else
        {
            CreateEnemy();
        }
    }
        

    bool IsFreeSpace(Vector3 point)
    {
        var hitCollider = Physics.OverlapSphere(point, 1);
        if (hitCollider.Length > 0)
        {
            return false;
        }
        return true;
    }

}
