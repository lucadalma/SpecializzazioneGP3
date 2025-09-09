using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnblockableAttackState : EnemyBaseState
{ 
    private float duration = 2f; // durata dell'attacco
    private float timer;

    private Attack attack;

    public EnemyUnblockableAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        timer = duration;

        // Attiva animazione speciale (colpo imparabile)
        stateMachine.Animator.CrossFadeInFixedTime("UnblockableAttack", 0.1f);

        // Attiva la spada con flag "unblockable"
        stateMachine.Weapon.SetUnblockableAttack(20, 15f);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();

        timer -= deltaTime;
        if (timer <= 0f)
        {
            stateMachine.Weapon.ResetUnblockable();
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Weapon.ResetUnblockable(); // reset per sicurezza
    }
}
