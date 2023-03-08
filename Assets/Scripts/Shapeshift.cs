using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    SkinnedMeshRenderer playerSMRenderer;
    MeshRenderer playerRenderer;
    MeshFilter playerMeshFilter;
    Mesh playerMesh;
    bool hasDisplayed = false;
    bool hasShifted;
    bool canShift = true;

    RaycastHit hit;
    float rayDistance = 6.0f;

    void Start()
    {
        //reference to player's meshrenderer
        playerRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        playerSMRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        playerMeshFilter = gameObject.GetComponentInChildren<MeshFilter>();
        playerMesh = GetComponent<Mesh>();
        playerSMRenderer.enabled = true;
        playerRenderer.enabled = false;
    }

    void Update()
    {
        ChangeShape();
    }

    void ChangeShape()
    {
        //sending a raycast from the player in a forward direction
        if(Physics.Raycast(this.transform.position, this.transform.forward , out hit, rayDistance))
        {
            //checking if the gameObject that the ray hits can be copied and if the player can change shape
            if (hit.collider.CompareTag("Copiable") && canShift)
            {
                FXHandler.Instance.EmitParticles(hit.collider.transform);//calling a function from my FXHadler Class to emit particles from copiable GOs
                if (!hasDisplayed)//cheking if player has been 'taught' how to shapeshift yet
                {
                    //if not, shows them the instruction once
                    UIHandler.Instance.DisplayMessage("LMB to ShapeShift");
                    hasDisplayed = true;    
                }
                if (Input.GetMouseButton(0) && !GameEnvironment.Singleton.playerHasShifted)// if player clicks while looking at copiable GO, and hasn't shifted yet
                {
                    CreateNewPlayer(hit.collider.gameObject.GetComponent<MeshRenderer>(), hit.collider.GetComponent<MeshFilter>().mesh, hit.collider.gameObject.GetComponent<MeshFilter>());
                    
                    GameEnvironment.Singleton.playerHasShifted = true;//ensuring player can't shift again until cooldown
                    canShift = false;//don't think this is being used atm?
                    FXHandler.Instance.shiftCooldownEnabled = true;// sets the bool in FXHandler class to true, begining a cooldown UI
                    StartCoroutine("ShiftCoolDown");// resets hasShifted to false after 10 seconds 
                }
            }
            else if(hit.collider.tag != "Copiable")
            { 
                FXHandler.Instance.StopEmitParticles();//stopping the particles emitting as soon as player is looking at anything else
            }

            if (hit.collider.CompareTag("Door"))//checking if the GO is a door
            {
                UIHandler.Instance.DisplayMessage("Press 'E'");//Telling the player what to do about it
                GameObject door = hit.collider.gameObject;//creating a GO variable of door
                if (Input.GetKey(KeyCode.E))
                {
                    OpenDoor(door);//calling the function to open the door(setting isActive to false, because doors are actually quite annoying )
                }
            }
        }
        
    }

    void CreateNewPlayer(MeshRenderer newPlayerMeshRenderer, Mesh newPlayerMesh, MeshFilter newPlayerMeshFilter)
    {
        playerSMRenderer.enabled = false;
        playerMesh = newPlayerMesh;
        playerMesh.bindposes = newPlayerMesh.bindposes;
        playerRenderer.material = newPlayerMeshRenderer.material;
        playerMeshFilter.mesh = newPlayerMeshFilter.mesh;
        playerRenderer.enabled = true;
        
        StartCoroutine("ReturnPlayer");
    }


    IEnumerator ShiftCoolDown()
    {
        yield return new WaitForSeconds(10);//waits for 10 seconds
        GameEnvironment.Singleton.playerHasShifted = false;//resets hasShifed to false
        canShift = true;//still not sure I need this
    }

    IEnumerator ReturnPlayer()
    {
        yield return new WaitForSeconds(5);//waits for 5 seconds
        //sets the players mesh back to normal, using a copy of the mesh hidden under the map
        //this was a workaround after many attempts to hold a reference to the mesh in a GameEnvironment handler and many other gruelling failures
        playerSMRenderer.sharedMesh = GameObject.Find("DefaultMesh").GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        playerRenderer.enabled = false;
        playerSMRenderer.enabled = true;
    }

    public void OpenDoor(GameObject door)
    {
        door.SetActive(false);//yeah, doors are very time-consuming
    }
}
