using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    public GameObject defaultPlayer;
    public TextMeshPro playerInstructions;
    public List<GameObject> doors = new List<GameObject>();
    SkinnedMeshRenderer toRemove;
    public CinemachineFreeLook mainCamera;
    ThirdPersonController tpc;
    int ShiftMessage = 0;
    int DoorMessage = 0;

    RaycastHit hit;
    float rayDistance = 6.0f;

    void Start()
    {
        toRemove = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        defaultPlayer = this.gameObject;
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.yellow, rayDistance);
        doors.AddRange(GameObject.FindGameObjectsWithTag("Door"));
        tpc = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();

    }

    void Update()
    {
        ChangeShape();
    }

    void ChangeShape()
    {
        if(Physics.Raycast(this.transform.position, this.transform.forward , out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Copiable"))
            {
                if (ShiftMessage < 1)
                { 
                    tpc.DisplayMessage("Click LMB to ShapeShift");
                    ShiftMessage++;
                }
                else
                {
                    tpc.DisplayMessage("(())");
                }
                if (Input.GetMouseButton(0))
                {
                    GameObject newPlayer = hit.collider.gameObject;
                    newPlayer.AddComponent<SkinnedMeshRenderer>();
                    CreateNewPlayer(newPlayer.GetComponent<SkinnedMeshRenderer>());
                }
            }

            if (hit.collider.CompareTag("Door"))
            {
                if (DoorMessage < 1)
                {
                    tpc.DisplayMessage("Click LMB to ShapeShift");
                    DoorMessage++;
                }
                tpc.DisplayMessage("Press 'E'");
                GameObject door = hit.collider.gameObject;
                if (Input.GetKey(KeyCode.E))
                {
                    OpenDoor(door);
                }
            }
        }
    }

    void CreateNewPlayer(SkinnedMeshRenderer player)
    { 
        toRemove.sharedMesh = null;
        toRemove.sharedMesh = player.sharedMesh;
        StartCoroutine("ReturnPlayer");

    }

    IEnumerator ReturnPlayer()
    {
        yield return new WaitForSeconds(5);
        toRemove.sharedMesh = GameObject.Find("DefaultMesh").GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;    
    }

    public void OpenDoor(GameObject door)
    {
        door.SetActive(false);
    }
}
