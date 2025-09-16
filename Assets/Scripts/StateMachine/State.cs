using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();             // Chiamato quando si entra nello stato
    public abstract void Tick(float deltaTime);  // Aggiornamento dello stato
    public abstract void Exit();              // Chiamato quando si esce dallo stato

    // Restituisce il tempo normalizzato dell'animazione "Attack"
    protected float GetNormalizeTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
            return nextInfo.normalizedTime;
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
            return currentInfo.normalizedTime;
        else
            return 0f;
    }
}

