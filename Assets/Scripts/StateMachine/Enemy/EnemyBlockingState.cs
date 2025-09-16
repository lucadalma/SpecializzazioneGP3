using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe EnemyBlockingState: stato in cui il nemico blocca gli attacchi
public class EnemyBlockingState : EnemyBaseState
{
    //Hash per l'animazione di blocco
    private readonly int BlockHash = Animator.StringToHash("Block");

    //Durata della transizione tra animazioni
    private const float CrossFadeDuration = 0.1f;

    //Durata totale del blocco
    private float blockDuration = 1.5f;

    //Timer per tenere traccia del tempo passato nello stato
    private float elapsedTime = 0f;

    //Costruttore dello stato
    public EnemyBlockingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata quando il nemico entra nello stato
    public override void Enter()
    {
        //Rende il nemico invulnerabile mentre blocca
        stateMachine.Health.SetInvulnerable(true);

        //Avvia l'animazione di blocco con crossfade
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame durante lo stato
    public override void Tick(float deltaTime)
    {
        //Permette al nemico di muoversi (anche se in blocco)
        Move(deltaTime);

        //Aggiorna il timer
        elapsedTime += deltaTime;

        //Se il tempo di blocco è scaduto, passa allo stato Idle
        if (elapsedTime >= blockDuration)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }

        //Fa guardare il nemico verso il player
        FacePlayer();
    }

    //Funzione chiamata quando il nemico esce dallo stato
    public override void Exit()
    {
        //Rimuove l'invulnerabilità
        stateMachine.Health.SetInvulnerable(false);
    }
}
