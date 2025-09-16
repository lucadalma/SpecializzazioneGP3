using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe EnemyChasingState: stato in cui il nemico insegue il player
public class EnemyChasingState : EnemyBaseState
{
    //Hash per il blend tree della locomozione
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    //Hash per il parametro Speed dell'Animator
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    //Durata della transizione tra animazioni
    private const float CrossFadeDuration = 0.1f;

    //Tempo di damping per aggiornare i parametri dell'Animator
    private const float AnimatorDampTime = 0.1f;

    //Costruttore dello stato
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    //Funzione chiamata quando il nemico entra nello stato
    public override void Enter()
    {
        //Avvia il blend tree della locomozione con crossfade
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame durante lo stato
    public override void Tick(float deltaTime)
    {
        //Se il player esce dal raggio di inseguimento passa allo stato Idle
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        //Se il player è entro il raggio di attacco passa allo stato Attacking
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        //Muove il nemico verso il player
        MoveToPlayer(deltaTime);

        //Fa guardare il nemico verso il player
        FacePlayer();

        //Aggiorna il parametro Speed dell'Animator per la locomozione
        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    //Funzione chiamata quando il nemico esce dallo stato
    public override void Exit()
    {
        //Resetta il percorso del NavMeshAgent e azzera la velocità
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    //Funzione per muovere il nemico verso il player
    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            //Imposta la destinazione del NavMeshAgent
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            //Muove il CharacterController secondo la velocità desiderata dal NavMeshAgent
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        //Sincronizza la velocità dell'Agent con quella del CharacterController
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    //Controlla se il player è entro il raggio di attacco
    private bool IsInAttackRange()
    {
        if (stateMachine.Player.IsDead) return false;

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
