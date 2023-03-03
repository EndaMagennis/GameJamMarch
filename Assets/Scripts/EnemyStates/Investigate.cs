using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static State;
using UnityEngine.AI;

public class Investigate : State
{
    Transform searchObject;

    public Investigate(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        state = STATE.INVESTIGATE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIlde");
        agent.isStopped = false;
        float lastDistance = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDistance)
            {
                lastDistance = distance;
            }
        }
        base.Enter();
    }

    public override void Update()
    {  
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 3000) < 10)// 10% chance of entering patrol
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");//ensures any queued 'isIlde' animations are cleared to begi next animation cleanly
        base.Exit();
    }
}
