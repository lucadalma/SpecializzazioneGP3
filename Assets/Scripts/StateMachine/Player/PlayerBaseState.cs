using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe astratta PlayerBaseState: base per tutti gli stati del giocatore
public abstract class PlayerBaseState : State
{
    //Riferimento alla macchina a stati del giocatore
    protected PlayerStateMachine stateMachine;

    //Costruttore: assegna la macchina a stati
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //Funzione di movimento generica senza input diretto
    protected void Move(float deltatime)
    {
        Move(Vector3.zero, deltatime);
    }

    //Funzione di movimento con vettore di motion
    protected void Move(Vector3 motion, float deltatime)
    {
        //Applica il movimento considerando anche eventuali forze esterne
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltatime);
    }

    //Funzione per far guardare il giocatore verso il target attuale
    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) return;

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    //Funzione per tornare allo stato di locomotion dopo un'azione
    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget != null)
        {
            //Se c'è un target passa allo stato targeting
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            //Altrimenti passa allo stato free look
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
