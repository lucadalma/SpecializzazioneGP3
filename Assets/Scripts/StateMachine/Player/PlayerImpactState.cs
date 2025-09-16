using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerImpactState: stato in cui il giocatore subisce un impatto e viene temporaneamente fermato
public class PlayerImpactState : PlayerBaseState
{
    //Hash per l'animazione di impatto
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    //Durata della transizione dell'animazione
    private const float CrossFadeDuration = 0.1f;

    //Durata dello stato in secondi
    private float duration = 1;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata all'ingresso nello stato
    public override void Enter()
    {
        //Avvia animazione di impatto
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Permette un leggero movimento del personaggio
        Move(deltaTime);

        //Riduce il timer dello stato
        duration -= deltaTime;

        //Se il timer termina, torna allo stato di locomotion o targeting
        if (duration <= 0)
        {
            ReturnToLocomotion();
        }
    }

    //Funzione chiamata all'uscita dallo stato
    public override void Exit()
    {
        //Nessuna azione particolare al termine dello stato
    }
}
