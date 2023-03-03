using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : State
{
    GameObject safeZone;

    public Flee(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        state = STATE.FLEE;
        safeZone = GameObject.FindGameObjectWithTag("Safe");
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        agent.isStopped = false;
        agent.speed = 6;
        agent.SetDestination(safeZone.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 2.0f)
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

