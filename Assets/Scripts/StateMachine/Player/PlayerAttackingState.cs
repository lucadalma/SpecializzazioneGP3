using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerAttackingState: stato del giocatore quando esegue un attacco
public class PlayerAttackingState : PlayerBaseState
{
    //Tempo normalizzato del frame precedente (per gestire la logica per frame)
    private float previousFrameTime;

    //Flag per sapere se la forza dell'attacco è già stata applicata
    private bool alreadyAppliedForce = false;

    //Riferimento all'attacco corrente
    private Attack attack;

    //Costruttore: prende l'indice dell'attacco nella lista del player
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    //Funzione chiamata quando il giocatore entra nello stato
    public override void Enter()
    {
        //Imposta i valori di danno e knockback sull'arma
        stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);

        //Avvia l'animazione dell'attacco
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        //Registra l'attacco nel tracker del giocatore (per IA adattiva)
        stateMachine.ActionTracker?.RegisterAttack();
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Gestisce il movimento del player
        Move(deltaTime);

        //Il player guarda il target
        FaceTarget();

        //Tempo normalizzato dell'animazione corrente
        float normalizedTime = GetNormalizeTime(stateMachine.Animator);

        //Se l'animazione è ancora in corso
        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            //Applica la forza solo al momento giusto
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            //Controlla se è possibile concatenare un attacco in combo
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else // Animazione finita
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    //Funzione chiamata quando il giocatore esce dallo stato
    public override void Exit()
    {

    }

    //Prova a eseguire l'attacco successivo della combo
    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;

        if (normalizedTime < attack.ComboAttackTime) return;

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex
            )
        );
    }

    //Applica la forza dell'attacco al player
    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }
}
