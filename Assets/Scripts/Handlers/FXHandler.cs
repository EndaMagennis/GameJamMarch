using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXHandler : MonoBehaviour
{
    public static FXHandler Instance;
    public ParticleSystem pSys;
    public Image cooldownRadial;
    public Image sprintBar;
    public float shiftCooldownTime = 10f;
    public bool shiftCooldownEnabled = false;
    public float sprintCooldownTime = 6.0f;
    public bool sprintCooldownEnabled = false;

    public bool canSprint = true;

    ThirdPersonController tpc;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        pSys = GetComponentInChildren<ParticleSystem>();
        pSys.Pause();
        cooldownRadial.enabled = false;
        sprintBar.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsShiftCooldownEnabled())
        {
            ApplyShiftCooldown();
        }
        if(sprintCooldownEnabled)
        {
            DrainSprintBar();
        }
        else
        {
            RefillSprintBar();
        }
    }

    public void EmitParticles(Transform gO)
    {
        pSys.transform.position = gO.position;
        pSys.Play();
    }
    

    public void StopEmitParticles()
    {
        pSys.Stop();
    }

    public void ApplyShiftCooldown()
    {
        if(shiftCooldownTime > 0.1f)
        {
            cooldownRadial.enabled=true;
            shiftCooldownTime -= Time.deltaTime;
            cooldownRadial.fillAmount = shiftCooldownTime /10;
        }
        else
        {
            cooldownRadial.enabled=false;
            shiftCooldownEnabled = false;
            RefillCooldownTimer();
        }
    }
    public void DrainSprintBar()
    {
        if (sprintCooldownTime > 0.1f)
        {
            sprintBar.enabled = true;
            sprintCooldownTime -= Time.deltaTime;
            sprintBar.fillAmount = sprintCooldownTime / 6;  
            canSprint = true;
        }
        else
        {
            sprintBar.enabled = false;
            sprintCooldownEnabled = false;
            canSprint = false;
        }
    }
    
    public void RefillSprintBar()
    {
        if(sprintCooldownTime < 6.0f)
        {
            sprintBar.enabled=true;
            sprintCooldownTime += Time.deltaTime;
            sprintBar.fillAmount = sprintCooldownTime / 6;
            if(sprintCooldownTime < 0.5f)
                canSprint = false;
        }
        else
        {
            sprintBar.enabled =false;
            sprintCooldownEnabled = false;
            canSprint = true;
        }
    }

    void RefillCooldownTimer()
    {
        shiftCooldownTime = 10.0f;
    }

    public bool IsShiftCooldownEnabled()
    {
        if (shiftCooldownEnabled)
        {
            return true;
        }
        else { return false; }  
    }

    public bool IsSprintCoolDownEnabled()
    {
        if (sprintCooldownEnabled)
        {
            return true;
        }
        return false;
    }
}
