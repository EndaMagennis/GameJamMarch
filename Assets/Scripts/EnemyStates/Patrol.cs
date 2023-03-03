using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentIndex = -1;
    

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        state = STATE.PATROL;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {       
        float lastDistance = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDistance)
            {
                currentIndex = i - 1; //update ++ the currentIndex, so this setting previous then updates and sets to closest
                lastDistance = distance;
            }
        }
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        if (IsPlayerBehind())
        {
            nextState = new Flee(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }


        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

