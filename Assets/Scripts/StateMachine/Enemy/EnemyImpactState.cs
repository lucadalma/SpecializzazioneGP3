using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe EnemyImpactState: stato in cui il nemico subisce un impatto/danno
public class EnemyImpactState : EnemyBaseState
{
    //Hash per l'animazione di impatto
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    //Durata della transizione tra animazioni
    private const float CrossFadeDuration = 0.1f;

    //Durata totale dello stato di impatto
    private float duration = 1f;

    //Costruttore dello stato
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata quando il nemico entra nello stato
    public override void Enter()
    {
        //Avvia l'animazione di impatto con crossfade
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame durante lo stato
    public override void Tick(float deltaTime)
    {
        //Permette al nemico di muoversi anche mentre subisce l'impatto
        Move(deltaTime);

        //Riduce il timer dello stato
        duration -= deltaTime;

        //Se il timer scade, passa allo stato Idle
        if (duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    //Funzione chiamata quando il nemico esce dallo stato (qui non fa nulla)
    public override void Exit()
    {

    }
}
