using UnityEngine;

public class EnemyPositioningState : EnemyBaseState
{
    private Transform target;
    private float idealDistance = 5f;
    private float distanceThreshold = 0.5f;

    public EnemyPositioningState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        target = stateMachine.Player.transform;
        stateMachine.Agent.stoppingDistance = idealDistance;
        stateMachine.Agent.isStopped = false;
    }

    public override void Tick(float deltaTime)
    {
        if (target == null)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        float distance = Vector3.Distance(stateMachine.transform.position, target.position);

        if (distance > idealDistance + distanceThreshold)
        {
            stateMachine.Agent.destination = target.position;
        }
        else if (distance < idealDistance - distanceThreshold)
        {
            Vector3 directionAway = (stateMachine.transform.position - target.position).normalized;
            Vector3 newPos = stateMachine.transform.position + directionAway * 2f;
            stateMachine.Agent.destination = newPos;
        }
        else
        {
            stateMachine.Agent.ResetPath();

            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
        }

        Move(deltaTime);
    }

    public override void Exit()
    {
        if (stateMachine.Agent.isActiveAndEnabled && stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.ResetPath();
        }
    }
}
