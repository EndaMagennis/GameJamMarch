using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ThirdPersonController : MonoBehaviour
{
    public GameObject PauseMenu;
    public AudioSource playerSound;
    public AudioClip foot;
    public AudioClip jumpSound;
    public GameObject deathScreen;
    public Camera GameCamera;
    public float playerSpeed = 5.0f;
    private float JumpForce = 1.0f;
    int playerHealth;
    int playerPoints;
    
    private CharacterController m_Controller;
    private Animator m_Animator;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    EnemySpawner m_EnemySpawner;
    public GameObject playerInfoUI;
    public GameObject worldInfoUI;
    TextMeshProUGUI playerInfo;
    TextMeshProUGUI worldInfo;



    private void Awake()
    {
        deathScreen.SetActive(false);
        PauseMenu.SetActive(false);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_Controller = gameObject.GetComponent<CharacterController>();
        m_Animator = gameObject.GetComponentInChildren<Animator>();
        m_EnemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        playerHealth = GameEnvironment.Singleton.playerHealth;
        playerPoints = GameEnvironment.Singleton.PlayerPoints;
        playerInfo = playerInfoUI.gameObject.GetComponent<TextMeshProUGUI>();
        worldInfo = worldInfoUI.gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        groundedPlayer = m_Controller.isGrounded;
        
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 8.33f;
        }
        else
            playerSpeed = 5.0f;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //trasnform input into camera space
        var forward = GameCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        var right = Vector3.Cross(Vector3.up, forward);
        
        Vector3 move = forward * input.z + right * input.x;
        move.y = 0;
        
        m_Controller.Move(move * Time.deltaTime * playerSpeed);

        m_Animator.SetFloat("MovementX", input.x);
        m_Animator.SetFloat("MovementZ", input.z);
        

        if (input != Vector3.zero)
        {
            gameObject.transform.forward = forward;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(JumpForce * -3.0f * gravityValue);
            m_Animator.SetTrigger("Jump");
            playerSound.PlayOneShot(jumpSound, 0.6f);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        m_Controller.Move(playerVelocity * Time.deltaTime);

        if(playerHealth <= 0)
        {
            DeathScreen();
        }

        UpdatePlayerInfo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            playerPoints += 100;
            playerHealth += 5;
            m_EnemySpawner.CreateEnemy();
            Destroy(other.gameObject);
        }
        else if (other.tag == "Enemy")
        {
            playerHealth -= 25;
        }
    }
    
    void UpdatePlayerInfo()
    {
        playerInfo.text = "Player Health: " + playerHealth.ToString() + "\n" + "Player Score: " + playerPoints.ToString() +"\n";
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


    public void DeathScreen()
    {
        Time.timeScale = 0;
        deathScreen.SetActive(true);
    }
}
