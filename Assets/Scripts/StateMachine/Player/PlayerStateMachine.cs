using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerStateMachine: gestisce tutti gli stati del giocatore
public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public PlayerActionTracker ActionTracker { get; private set; } // Tracker azioni giocatore
    [field: SerializeField] public InputReader InputReader { get; private set; } // Input da tastiera/controller
    [field: SerializeField] public CharacterController Controller { get; private set; } // Controller del personaggio
    [field: SerializeField] public Animator Animator { get; private set; } // Animator
    [field: SerializeField] public Targeter Targeter { get; private set; } // Sistema di lock-on
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; } // Gestione forze esterne
    [field: SerializeField] public WeaponDamage Weapon { get; private set; } // Gestione armi
    [field: SerializeField] public Health Health { get; private set; } // Sistema salute
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; } // Sistema ragdoll

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; } // Velocità movimento in free look
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; } // Velocità movimento in targeting
    [field: SerializeField] public float RotationDamping { get; private set; } // Damping rotazione
    [field: SerializeField] public float DodgeDuration { get; private set; } // Durata del dodge
    [field: SerializeField] public float DodgeLenght { get; private set; } // Lunghezza del dodge
    [field: SerializeField] public float JumpForce { get; private set; } // Forza del salto
    [field: SerializeField] public Attack[] Attacks { get; private set; } // Lista attacchi giocatore

    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity; // Timer per gestire cooldown dodge
    public Transform MainCameraTransform { get; private set; } // Riferimento alla camera principale
    public bool IsBlocking { get; set; } // Stato di blocco del giocatore

    private void Start()
    {
        // Blocca il cursore al centro dello schermo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Prende la trasformazione della camera principale
        MainCameraTransform = Camera.main.transform;

        // Imposta stato iniziale del giocatore
        SwitchState(new PlayerFreeLookState(this));
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

    //Gestione danno ricevuto
    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    //Gestione morte giocatore
    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }
}
