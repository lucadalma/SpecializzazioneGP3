using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Variabile MaxHealth
    [SerializeField] public int maxHealth = 100;

    //variabile health
    private int health;

    //variabile se è vulnerabile
    private bool isInvulnerable;

    //Evento per quando prende danno
    public event Action OnTakeDamage;

    //Evento per quando muore
    public event Action OnDie;

    //Se il player è morto
    public bool IsDead => health == 0;

    //riferimento alla HealthBar
    [SerializeField] FloatingHealthBar healthBar;

    //riferimento per il logo AI adattiva
    [SerializeField] GameObject adaptiveLogo;

    private void Start()
    {
        //Healt prende il valore di maxhealth
        health = maxHealth;
        
        //update della healthBar
        healthBar.UpdateHealthBar(health, maxHealth);
    }
    
    //Funzione per invulnerabilità
    public void SetInvulnerable(bool isInvulnerable) 
    {
        this.isInvulnerable = isInvulnerable;
    }

    //Funzione per prendere il danno
    public void DealDamage(int damage) 
    {
        //Controllo la health
        if (health == 0) return;

        //Controllo se vulnerabile
        if (isInvulnerable) return;

        //Togli vita
        health = Mathf.Max(health - damage, 0);
        //Update healthBar
        healthBar.UpdateHealthBar(health, maxHealth);

        //Chiama l'evento
        OnTakeDamage?.Invoke();

        //Se health = 0
        if (health == 0) 
        {
            //Chiama evento della morte
            OnDie?.Invoke();
            //Distattivo alcune cose
            healthBar.gameObject.SetActive(false);
            adaptiveLogo.SetActive(false);
        }
    }

}
