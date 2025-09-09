using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public float RetreatMinDistance { get; private set; } = 4f;   // se il player è più vicino di così, scappo
    [field: SerializeField] public float RetreatMaxDistance { get; private set; } = 8f;   // obiettivo di “comfort”
    [field: SerializeField] public float RetreatRepathInterval { get; private set; } = 0.5f; // ogni quanto ricalcolo la destinazione
    [field: SerializeField] public float RetreatCooldown { get; private set; } = 6f;
    public float LastRetreatTime { get; set; } = -Mathf.Infinity;

    [field: SerializeField] public CharacterController Controller { get; private set; }

    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    
    [field: SerializeField] public Health Health { get; private set; }

    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public float PlayerChasingRange { get; private set; }

    [field: SerializeField] public float AttackRange { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackKnockback { get; private set; }

    public Health Player { get; private set; }

    public EnemyAdaptiveAI.EnemyBehaviour Strategy { get; private set; } = EnemyAdaptiveAI.EnemyBehaviour.Balanced;

    private void Start()
    {


        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage() 
    {
        SwitchState(new EnemyImpactState(this));
    }
    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }
    public void SetStrategy(EnemyAdaptiveAI.EnemyBehaviour newStyle)
    {
        Strategy = newStyle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}
