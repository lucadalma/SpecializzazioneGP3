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

}
