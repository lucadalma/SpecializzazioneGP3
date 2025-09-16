using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe EnemyDeadState: stato in cui il nemico è morto
public class EnemyDeadState : EnemyBaseState
{
    //Costruttore dello stato
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    //Funzione chiamata quando il nemico entra nello stato
    public override void Enter()
    {
        //Attiva il ragdoll del nemico
        stateMachine.Ragdoll.ToggleRagdoll(true);

        //Disattiva l'arma del nemico
        stateMachine.Weapon.gameObject.SetActive(false);

        //Distrugge il gameObject del target associato
        GameObject.Destroy(stateMachine.Target);
    }

    //Funzione chiamata ogni frame durante lo stato (qui non fa nulla)
    public override void Tick(float deltaTime)
    {

    }

    //Funzione chiamata quando il nemico esce dallo stato (qui non fa nulla)
    public override void Exit()
    {

    }
}
