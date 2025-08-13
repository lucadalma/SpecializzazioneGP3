using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockingState : EnemyBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Block");

    private const float CrossFadeDuration = 0.1f;

    private float blockDuration = 1.5f;

    private float elapsedTime = 0f;

    public EnemyBlockingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        elapsedTime += deltaTime;
        if (elapsedTime >= blockDuration)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }

        FacePlayer();
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

}
