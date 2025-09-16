using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe Attack: rappresenta un singolo attacco con tutti i suoi parametri
[Serializable]
public class Attack
{
    //Nome dell'animazione associata all'attacco
    [field: SerializeField]
    public string AnimationName { get; private set; }

    //Durata della transizione verso questa animazione
    [field: SerializeField]
    public float TransitionDuration { get; private set; }

    //Indice dello stato combo (-1 se non è parte di una combo)
    [field: SerializeField]
    public int ComboStateIndex { get; private set; } = -1;

    //Tempo entro cui il giocatore può concatenare il prossimo attacco della combo
    [field: SerializeField]
    public float ComboAttackTime { get; private set; }

    //Durata dell’applicazione della forza durante l’attacco
    [field: SerializeField]
    public float ForceTime { get; private set; }

    //Forza applicata durante l’attacco (per knockback o spinta)
    [field: SerializeField]
    public float Force { get; private set; }

    //Danno inflitto dall’attacco
    [field: SerializeField]
    public int Damage { get; private set; }

    //Knockback applicato dall’attacco
    [field: SerializeField]
    public float Knockback { get; private set; }
}
