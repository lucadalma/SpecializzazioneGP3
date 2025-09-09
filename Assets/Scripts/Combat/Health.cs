using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;

    private int health;

    private bool isInvulnerable;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public bool IsDead => health == 0;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] GameObject adaptiveLogo;

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }
    

    public void SetInvulnerable(bool isInvulnerable) 
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage) 
    {
        if (health == 0) return;

        if (isInvulnerable) return;

        health = Mathf.Max(health - damage, 0);
        healthBar.UpdateHealthBar(health, maxHealth);

        OnTakeDamage?.Invoke();

        if (health == 0) 
        {
            OnDie?.Invoke();
            healthBar.gameObject.SetActive(false);
            adaptiveLogo.SetActive(false);
        }

        Debug.Log(health);
    }

}
