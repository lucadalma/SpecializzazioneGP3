using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Classe EnemyStateMachine: gestisce la macchina a stati del nemico e le proprietà generali
public class EnemyStateMachine : StateMachine
{
    //--- Parametri per la fuga ---
    [field: SerializeField] public float RetreatMinDistance { get; private set; } = 4f;   // se il player è più vicino di così, il nemico scappa
    [field: SerializeField] public float RetreatMaxDistance { get; private set; } = 8f;   // distanza obiettivo di “comfort”
    [field: SerializeField] public float RetreatRepathInterval { get; private set; } = 0.5f; // intervallo per ricalcolare la destinazione
    [field: SerializeField] public float RetreatCooldown { get; private set; } = 6f;       // cooldown tra fughe
    public float LastRetreatTime { get; set; } = -Mathf.Infinity;

    //--- Componenti del nemico ---
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    //--- Parametri di combattimento ---
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackKnockback { get; private set; }

    //Riferimento al player
    public Health Player { get; private set; }

    //Strategia attuale del nemico (Aggressive, Defensive, Balanced)
    public EnemyBehaviourSO CurrentStrategy { get; private set; }

    //--- Parametri di pattuglia ---
    [field: SerializeField] public Transform[] PatrolPoints { get; private set; }
    [field: SerializeField] public bool PatrolLoop { get; private set; } = true;
    [field: SerializeField] public float PatrolWaitTime { get; private set; } = 1.25f;
    [field: SerializeField] public float PatrolSpeedMultiplier { get; private set; } = 0.6f;
    public int PatrolIndex { get; set; } = 0;
    public int PatrolDirection { get; set; } = 1; // 1 forward, -1 backward (per ping-pong)
    public float LastAtPointTime { get; set; } = -Mathf.Infinity;

    [SerializeField] public GameObject attackLogo;

    //Funzione chiamata all’avvio
    private void Start()
    {
        //Trova il player nella scena tramite tag
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        //Disabilita aggiornamento automatico del NavMeshAgent (sarà gestito manualmente)
        Agent.updatePosition = false;
        Agent.updateRotation = false;

        //Inizia nello stato Idle
        SwitchState(new EnemyIdleState(this));
    }

    //Gestione eventi Health
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

    //Quando il nemico prende danno passa allo stato Impact
    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    //Quando il nemico muore passa allo stato Dead
    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    //Aggiorna la strategia dell’IA adattiva
    public void SetStrategy(EnemyBehaviourSO newStrategy)
    {
        CurrentStrategy = newStrategy;
    }

    //Gizmo per visualizzare il raggio di inseguimento nel Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}
