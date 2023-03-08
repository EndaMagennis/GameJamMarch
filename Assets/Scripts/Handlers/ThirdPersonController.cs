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
    public GameObject deathScreen;
    public Camera GameCamera;
    public float playerSpeed = 5.0f;
    private float JumpForce = 1.0f;

    private CharacterController m_Controller;
    private Animator m_Animator;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    EnemySpawner m_EnemySpawner;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_Controller = gameObject.GetComponent<CharacterController>();
        m_Animator = gameObject.GetComponentInChildren<Animator>();
        m_EnemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    void Update()
    {
        groundedPlayer = m_Controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && FXHandler.Instance.canSprint)
        {
            FXHandler.Instance.sprintCooldownEnabled = true;
            playerSpeed = 8.33f;
        }
        else
        {
            FXHandler.Instance.sprintCooldownEnabled = false;
            playerSpeed = 5.0f;
        }


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
            AudioHandler.Instance.masterAudio.PlayOneShot(AudioHandler.Instance.playerJump, 0.6f);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        m_Controller.Move(playerVelocity * Time.deltaTime);

        if (GameEnvironment.Singleton.playerHealth <= 0)
        {
            UIHandler.Instance.DeathScreen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            if (GameEnvironment.Singleton.playerHealth < 100)
                GameEnvironment.Singleton.ModifyHealth(5);

            GameEnvironment.Singleton.ModifyScore(100);
            this.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            m_EnemySpawner.CreateEnemy();
            Destroy(other.gameObject);
        }
        if (other.tag == "DeathCollider")
        {
            UIHandler.Instance.DeathScreen();
            Destroy(gameObject);
        }
    }
}
