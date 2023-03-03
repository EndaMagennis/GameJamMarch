using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpawner : MonoBehaviour
{
    public GameObject goalPrefab;
    float XBound = 30f;
    float ZBound = 30f;
    public GameObject mapfloor;

    public AudioClip ping;
    public AudioSource gaolSound;
    ThirdPersonController tpc;
    // Start is called before the first frame update
    void Start()
    {
        tpc = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
        InvokeRepeating("CreateEnemy", 5.0f, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateEnemy()
    {
        Instantiate(goalPrefab, new Vector3(Random.Range(XBound, -XBound), 1.5f, Random.Range(ZBound, -ZBound)), goalPrefab.transform.rotation);
        gaolSound.PlayOneShot(ping);
        tpc.DisplayMessage("New Coin on the Map");
    }
}
