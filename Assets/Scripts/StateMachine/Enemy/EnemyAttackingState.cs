using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe EnemyAttackingState: gestisce lo stato di attacco del nemico
public class EnemyAttackingState : EnemyBaseState
{
    //Hash per l'animazione di attacco
    private readonly int AttackHash = Animator.StringToHash("Attack");

    //Durata della transizione tra animazioni
    private const float TransitionDuration = 0.1f;

    //Costruttore dello stato
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    //Funzione chiamata quando si entra nello stato
    public override void Enter()
    {
        //Probabilità per blocco e attacco imparabile
        float blockChance = 0f;
        float superAttackChance = 0f;

        //Imposta le probabilità in base allo stile adattivo del nemico
        switch (stateMachine.Strategy)
        {
            case EnemyAdaptiveAI.EnemyBehaviour.Defensive:
                blockChance = 0.7f;
                superAttackChance = 0.3f;
                break;
            case EnemyAdaptiveAI.EnemyBehaviour.Balanced:
                blockChance = 0.5f;
                superAttackChance = 0.3f;
                break;
            case EnemyAdaptiveAI.EnemyBehaviour.Aggressive:
                blockChance = 0.3f;
                superAttackChance = 0.6f;
                break;
        }

        //Valore casuale tra 0 e 1
        float randomValue = Random.value;

        //Se il valore casuale è inferiore alla probabilità di blocco passa allo stato di blocco
        if (randomValue < blockChance)
        {
            stateMachine.SwitchState(new EnemyBlockingState(stateMachine));
            return;
        }
        //Se il valore è inferiore alla somma di blockChance + superAttackChance attacco imparabile
        else if (randomValue < blockChance + superAttackChance)
        {
            stateMachine.SwitchState(new EnemyUnblockableAttackState(stateMachine));
            return;
        }
        //Altrimenti attacco normale
        else
        {
            stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        }
    }

    //Funzione chiamata ogni frame durante lo stato
    public override void Tick(float deltaTime)
    {
        //Quando l'animazione di attacco è completata, passa allo stato di inseguimento
        if (GetNormalizeTime(stateMachine.Animator) >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }

        //Il nemico guarda sempre verso il player
        FacePlayer();
    }

    //Funzione chiamata quando si esce dallo stato
    public override void Exit()
    {

    }
}
