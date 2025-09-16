using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Questa classe legge l'input del giocatore e notifica gli stati agli altri sistemi
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    // Flag pubblici che altri sistemi possono leggere
    public bool IsAttacking { get; private set; }  // True quando il giocatore preme il tasto attacco
    public bool IsBlocking { get; private set; }   // True quando il giocatore preme il tasto block

    // Vettore 2D della direzione di movimento
    public Vector2 MovementValue { get; private set; }

    // Eventi che altri script possono sottoscrivere
    public event Action JumpEvent;   // Evento scatenato quando il giocatore salta
    public event Action DodgeEvent;  // Evento scatenato quando il giocatore fa un dodge
    public event Action TargetEvent; // Evento scatenato quando il giocatore seleziona un target

    private Controls controls; // Riferimento al sistema di Input Actions generato da Unity Input System

    private void Start()
    {
        // Creazione dell’oggetto Controls (auto-generato da Input System)
        controls = new Controls();

        // Impostiamo questa classe come callback per le azioni del Player
        controls.Player.SetCallbacks(this);

        // Abilitiamo gli input del Player
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        // Disabilitiamo gli input quando l'oggetto viene distrutto
        controls.Player.Disable();
    }

    // Funzione chiamata dall'Input System quando si preme Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // Ignora eventi non eseguiti

        JumpEvent?.Invoke(); // Scatena l'evento Jump
    }

    // Funzione chiamata dall'Input System quando si preme Dodge
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // Ignora eventi non eseguiti

        DodgeEvent?.Invoke(); // Scatena l'evento Dodge
    }

    // Funzione chiamata dall'Input System quando il giocatore muove lo stick / WASD
    public void OnMove(InputAction.CallbackContext context)
    {
        // Salva il valore del movimento
        MovementValue = context.ReadValue<Vector2>();
    }

    // Funzione chiamata dall'Input System quando si muove la camera / mouse
    public void OnLook(InputAction.CallbackContext context)
    {
        // Al momento non facciamo nulla con il look
    }

    // Funzione chiamata dall'Input System quando si preme il tasto Target
    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // Ignora eventi non eseguiti

        TargetEvent?.Invoke(); // Scatena l'evento Target
    }

    // Funzione chiamata dall'Input System quando si preme il tasto Attack
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true; // Inizio attacco
        }
        else if (context.canceled)
        {
            IsAttacking = false; // Fine attacco
        }
    }

    // Funzione chiamata dall'Input System quando si preme il tasto Block
    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true; // Inizio block
        }
        else if (context.canceled)
        {
            IsBlocking = false; // Fine block
        }
    }
}
