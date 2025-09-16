using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerJumpingState: stato in cui il giocatore salta
public class PlayerJumpingState : PlayerBaseState
{
    //Hash per l'animazione di salto
    private readonly int JumpHash = Animator.StringToHash("Jump");

    //Momento iniziale del salto (movimento orizzontale mantenuto)
    private Vector3 momentum;

    //Durata della transizione dell'animazione
    private const float CrossFadeDuration = 0.1f;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    //Funzione chiamata all'ingresso nello stato
    public override void Enter()
    {
        //Applica forza verticale per il salto
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        //Mantiene la velocità orizzontale al momento del salto
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        //Avvia animazione di salto
        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Muove il giocatore lungo la direzione orizzontale
        Move(momentum, deltaTime);

        //Se la velocità verticale diventa negativa, passa allo stato di caduta
        if (stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        //Il personaggio guarda verso il target se presente
        FaceTarget();
    }

    //Funzione chiamata all'uscita dallo stato
    public override void Exit()
    {
        //Nessuna azione particolare all'uscita
    }
}
