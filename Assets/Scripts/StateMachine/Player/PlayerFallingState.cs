using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerFallingState: stato in cui il giocatore sta cadendo
public class PlayerFallingState : PlayerBaseState
{
    //Hash dell'animazione di caduta
    private readonly int FallHash = Animator.StringToHash("Fall");

    //Momentum orizzontale durante la caduta
    private Vector3 momentum;

    //Durata della transizione verso l'animazione di caduta
    private const float CrossFadeDuration = 0.1f;

    //Costruttore
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    //Funzione chiamata quando il giocatore entra nello stato
    public override void Enter()
    {
        //Salva la velocità corrente del controller, ignorando la componente verticale
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        //Avvia l'animazione di caduta
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Applica il movimento orizzontale durante la caduta
        Move(momentum, deltaTime);

        //Se il giocatore tocca terra, torna allo stato appropriato
        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        //Guarda verso il target corrente se presente
        FaceTarget();
    }

    //Funzione chiamata quando il giocatore esce dallo stato
    public override void Exit()
    {

    }
}
