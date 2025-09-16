using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //Evento che viene chiamato quando il target viene distrutto
    public event Action<Target> OnDestroyed;

    //Funzione chiamata automaticamente da Unity quando l'oggetto viene distrutto
    private void OnDestroy()
    {
        //Invoca l'evento passando questo oggetto come parametro
        OnDestroyed?.Invoke(this);
    }
}
