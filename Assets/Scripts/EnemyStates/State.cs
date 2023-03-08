using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, INVESTIGATE, ATTACK, FLEE
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE state;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;
    protected GameObject playerObject;

    float visDistance = 15.0f;
    float visAngle = 50.0f;
    float attackDistance = 1.0f;
    

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, GameObject _playerObject)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
        playerObject = _playerObject;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT;}

    public State Process()
    {
        if(stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);

            if (direction.magnitude < visDistance && angle < visAngle && !GameEnvironment.Singleton.playerHasShifted)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsPlayerBehind()
    {
        if (player != null)
        {
            Vector3 direction = npc.transform.position - player.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            if (direction.magnitude < 2.0f && angle < 30.0f && !GameEnvironment.Singleton.playerHasShifted)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < attackDistance)
        {
            return true;
        }
        return false;
    }
}
