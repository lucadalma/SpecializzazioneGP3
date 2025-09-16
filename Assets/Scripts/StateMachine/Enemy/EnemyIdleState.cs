using UnityEngine;

//Classe EnemyIdleState: stato in cui il nemico è fermo o esegue il pattugliamento
public class EnemyIdleState : EnemyBaseState
{
    //Hash per il blend tree della camminata
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Walking");
    //Hash per il parametro Speed dell'Animator
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    //Durata della transizione tra animazioni
    private const float CrossFadeDuration = 0.1f;
    //Tempo di damping per aggiornare i parametri dell'Animator
    private const float AnimatorDampTime = 0.1f;

    //Costruttore dello stato
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata quando il nemico entra nello stato
    public override void Enter()
    {
        //Avvia il blend tree della camminata
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);

        //Se c'è un NavMeshAgent, imposta lo stopping distance a 0
        if (stateMachine.Agent != null)
        {
            stateMachine.Agent.stoppingDistance = 0f;
        }
    }

    //Funzione chiamata ogni frame durante lo stato
    public override void Tick(float deltaTime)
    {
        //Se il player è entro il raggio di inseguimento passa allo stato Chasing
        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        //Se non ci sono punti di pattuglia fermo, aggiorna animazione
        if (stateMachine.PatrolPoints == null || stateMachine.PatrolPoints.Length == 0)
        {
            stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
            Move(deltaTime);
            return;
        }

        //Seleziona il punto di pattuglia corrente
        Transform target = stateMachine.PatrolPoints[Mathf.Clamp(stateMachine.PatrolIndex, 0, stateMachine.PatrolPoints.Length - 1)];
        if (target == null)
        {
            stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }

        //Calcola la distanza al punto di pattuglia
        Vector3 to = target.position - stateMachine.transform.position;
        float sqr = to.sqrMagnitude;
        bool atPoint = sqr <= 0.25f * 0.25f; //Raggio di tolleranza 0.25 unità

        if (atPoint)
        {
            //Se il nemico ha aspettato abbastanza passa al prossimo punto
            if (Time.time - stateMachine.LastAtPointTime > stateMachine.PatrolWaitTime)
            {
                stateMachine.LastAtPointTime = Time.time;
                AdvancePatrolIndex();
            }
            stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
            Move(deltaTime);
            return;
        }

        //Se c'è un NavMeshAgent muove il nemico verso il punto di pattuglia
        if (stateMachine.Agent != null)
        {
            stateMachine.Agent.destination = target.position;
            Vector3 desired = stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed * stateMachine.PatrolSpeedMultiplier;
            Move(desired, deltaTime);
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }
        else //Altrimenti muove senza NavMeshAgent
        {
            Vector3 dir = to.normalized * stateMachine.MovementSpeed * stateMachine.PatrolSpeedMultiplier;
            Move(dir, deltaTime);
        }

        //Rotazione verso il punto di pattuglia
        Vector3 flat = new Vector3(to.x, 0f, to.z);
        if (flat.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(flat);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, look, 0.2f);
        }

        //Aggiorna il parametro Speed dell'Animator
        stateMachine.Animator.SetFloat(SpeedHash, 1f * stateMachine.PatrolSpeedMultiplier, AnimatorDampTime, deltaTime);
    }

    //Avanza all'indice successivo della pattuglia
    private void AdvancePatrolIndex()
    {
        if (stateMachine.PatrolLoop)
        {
            //Loop circolare
            stateMachine.PatrolIndex = (stateMachine.PatrolIndex + 1) % stateMachine.PatrolPoints.Length;
        }
        else
        {
            //Avanza e inverte direzione se raggiunge fine array
            int next = stateMachine.PatrolIndex + stateMachine.PatrolDirection;
            if (next < 0 || next >= stateMachine.PatrolPoints.Length)
            {
                stateMachine.PatrolDirection *= -1;
                next = Mathf.Clamp(stateMachine.PatrolIndex + stateMachine.PatrolDirection, 0, stateMachine.PatrolPoints.Length - 1);
            }
            stateMachine.PatrolIndex = next;
        }
    }

    //Funzione chiamata quando il nemico esce dallo stato (qui non fa nulla)
    public override void Exit() { }
}
