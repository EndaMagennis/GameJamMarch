using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    float rotSpeed = 2.0f;
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, GameObject _playerObject)
        : base(_npc, _agent, _anim, _player, _playerObject)
    {
        state = STATE.ATTACK;
    }

    public override void Enter()
    {
        anim.SetTrigger("isAttacking");
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                            Quaternion.LookRotation(direction),
                                            Time.deltaTime * rotSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isAttacking");
        base.Exit();
    }
}
