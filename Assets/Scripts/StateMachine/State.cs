using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe astratta per la state machine
public abstract class State
{

    //funzione astratta per quando entra in un specifico stato
    public abstract void Enter();

    //funzione astratta per quando esegue un specifico stato
    public abstract void Tick(float deltaTime);
    
    //funzione astratta per quando esce da un specifico stato
    public abstract void Exit();

    protected float GetNormalizeTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}
