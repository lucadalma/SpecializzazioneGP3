using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe EnemyUnblockableAttackState: stato in cui il nemico esegue un attacco imparabile
public class EnemyUnblockableAttackState : EnemyBaseState
{
    //Durata totale dell'attacco
    private float duration = 2f;
    private float timer;

    //Riferimento all'attacco (qui non usato direttamente)
    private Attack attack;

    //Costruttore dello stato
    public EnemyUnblockableAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata quando il nemico entra nello stato
    public override void Enter()
    {
        //Mostra l'icona di attacco speciale
        stateMachine.attackLogo.SetActive(true);

        //Inizializza il timer
        timer = duration;

        //Avvia animazione speciale di colpo imparabile
        stateMachine.Animator.CrossFadeInFixedTime("UnblockableAttack", 0.1f);

        //Attiva l’arma come colpo imparabile
        stateMachine.Weapon.SetUnblockableAttack(20, 15f);
    }

    //Funzione chiamata ogni frame durante lo stato
    public override void Tick(float deltaTime)
    {
        //Il nemico guarda sempre il player
        FacePlayer();

        //Riduce il timer
        timer -= deltaTime;

        //Quando il timer finisce reset arma e passa allo stato Chasing
        if (timer <= 0f)
        {
            stateMachine.Weapon.ResetUnblockable();
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    //Funzione chiamata quando il nemico esce dallo stato
    public override void Exit()
    {
        //Nasconde l'icona di attacco e resetta la flag unblockable per sicurezza
        stateMachine.attackLogo.SetActive(false);
        stateMachine.Weapon.ResetUnblockable();
    }
}
