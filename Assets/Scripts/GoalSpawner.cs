using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpawner : MonoBehaviour
{
    public GameObject goalPrefab;
    float XBound = 30f;
    float ZBound = 30f;
    public GameObject mapfloor;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateEnemy", 10.0f, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateEnemy()
    {
        Vector3 spawnPos = new Vector3(Random.Range(XBound, -XBound), 1.5f, Random.Range(ZBound, -ZBound));
        if (IsFreeSpace(spawnPos))
        {
            Instantiate(goalPrefab, spawnPos, goalPrefab.transform.rotation);
            AudioHandler.Instance.masterAudio.PlayOneShot(AudioHandler.Instance.coinPing);
            UIHandler.Instance.DisplayMessage("New Coin on the Map");
        }
        else
        {
            CreateEnemy();
        }
        
    }

    bool IsFreeSpace(Vector3 point)
    {
        var hitCollider = Physics.OverlapSphere(point, 1);
        if(hitCollider.Length > 0)
        {
            return false;
        }
        return true;
    }
}
