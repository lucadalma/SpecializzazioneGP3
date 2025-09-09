using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{

    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float TransitionDuration = 0.1f;


    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        float blockChance = 0f;
        float superAttackChance = 0f;


        switch (stateMachine.Strategy)
        {
            case EnemyAdaptiveAI.EnemyBehaviour.Defensive:
                blockChance = 0.6f;
                superAttackChance = 0.2f;
                break;
            case EnemyAdaptiveAI.EnemyBehaviour.Balanced:
                blockChance = 0.3f;
                superAttackChance = 0.3f;
                break;
            case EnemyAdaptiveAI.EnemyBehaviour.Aggressive:
                blockChance = 0.1f;
                superAttackChance = 0.4f;
                break;
        }

        if (Random.value < blockChance)
        {
            stateMachine.SwitchState(new EnemyBlockingState(stateMachine));
            return;
        }
        else
        {
            if (Random.value < superAttackChance)
            {
                stateMachine.SwitchState(new EnemyUnblockableAttackState(stateMachine));
                return;
            }
            stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);

        }

    }

    public override void Tick(float deltaTime)
    {

        if (GetNormalizeTime(stateMachine.Animator) >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }

        FacePlayer();

    }

    public override void Exit()
    {

    }

}
