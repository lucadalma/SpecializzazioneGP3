using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerDeadState: stato in cui il giocatore è morto
public class PlayerDeadState : PlayerBaseState
{
    //Costruttore
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    //Funzione chiamata quando il giocatore entra nello stato
    public override void Enter()
    {
        //Attiva il ragdoll del giocatore
        stateMachine.Ragdoll.ToggleRagdoll(true);

        //Disattiva l'arma del giocatore
        stateMachine.Weapon.gameObject.SetActive(false);
    }

    //Funzione chiamata ogni frame (nessuna logica durante la morte)
    public override void Tick(float deltaTime)
    {

    }

    //Funzione chiamata quando il giocatore esce dallo stato (nessuna logica)
    public override void Exit()
    {

    }
}
