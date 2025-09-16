using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe astratta EnemyBaseState: base per tutti gli stati del nemico
public abstract class EnemyBaseState : State
{
    //Riferimento alla macchina a stati del nemico
    protected EnemyStateMachine stateMachine;

    //Costruttore che assegna la state machine
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //Funzione di movimento generica senza input
    protected void Move(float deltatime)
    {
        Move(Vector3.zero, deltatime);
    }

    //Funzione di movimento con vettore di movimento
    protected void Move(Vector3 motion, float deltatime)
    {
        //Somma il movimento desiderato e quello ricevuto dal ForceReceiver (es. knockback) e applica al CharacterController
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltatime);
    }

    //Funzione per far guardare il nemico verso il player
    protected void FacePlayer()
    {
        if (stateMachine.Player == null) return;

        //Calcola la direzione verso il player
        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0; //Ignora l'altezza per non inclinare il nemico

        //Aggiorna la rotazione del nemico
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    //Controlla se il player è entro il raggio di inseguimento
    protected bool IsInChaseRange()
    {
        //Se il player è morto, non è in range
        if (stateMachine.Player.IsDead) return false;

        //Calcola la distanza al quadrato per evitare sqrt (ottimizzazione)
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        //Confronta con il raggio di inseguimento al quadrato
        return playerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }
}
