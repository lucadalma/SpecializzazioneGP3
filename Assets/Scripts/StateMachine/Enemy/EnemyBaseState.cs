using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltatime)
    {
        Move(Vector3.zero, deltatime);
    }

    protected void Move(Vector3 motion, float deltatime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltatime);
    }

    protected void FacePlayer() 
    {
        if (stateMachine.Player == null) return;

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange() 
    {
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }
}
