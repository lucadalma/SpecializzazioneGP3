using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    //stato attuale
    private State currentState;

    // Imposta un nuovo stato
    public void SwitchState(State newState)
    {
        // Se esiste uno stato attuale, esegue Exit
        currentState?.Exit();

        // Cambia stato
        currentState = newState;

        // Esegue Enter del nuovo stato
        currentState?.Enter();
    }

    private void Update()
    {
        //esegue lo stato attuale
        currentState?.Tick(Time.deltaTime);
    }

}
