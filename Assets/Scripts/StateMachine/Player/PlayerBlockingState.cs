using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerBlockingState: stato in cui il giocatore sta bloccando
public class PlayerBlockingState : PlayerBaseState
{
    //Hash dell'animazione di blocco
    private readonly int BlockHash = Animator.StringToHash("Block");

    //Durata della transizione verso l'animazione di blocco
    private const float CrossFadeDuration = 0.1f;

    //Costruttore
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata quando il giocatore entra nello stato
    public override void Enter()
    {
        //Il giocatore diventa invulnerabile durante il blocco
        stateMachine.Health.SetInvulnerable(true);

        //Avvia l'animazione di blocco
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);

        //Registra il blocco nel tracker del giocatore (per IA adattiva)
        stateMachine.ActionTracker?.RegisterBlock();

        //Segna il player come in stato di blocco
        stateMachine.IsBlocking = true;
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Permette al giocatore di muoversi mentre blocca
        Move(deltaTime);

        //Se il giocatore smette di bloccare torna allo stato appropriato
        if (!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }

        //Se non c'è più un target passa allo stato free look
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
    }

    //Funzione chiamata quando il giocatore esce dallo stato
    public override void Exit()
    {
        //Rimuove l'invulnerabilità e disattiva lo stato di blocco
        stateMachine.Health.SetInvulnerable(false);
        stateMachine.IsBlocking = false;
    }
}
