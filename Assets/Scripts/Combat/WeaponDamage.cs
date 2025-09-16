using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe WeaponDamage: gestisce il danno e l’effetto knockback delle armi
public class WeaponDamage : MonoBehaviour
{
    //Collider associato all’arma
    [SerializeField] private Collider myCollider;

    //Danno inflitto dall’attacco
    private int damage;
    //Forza del knockback
    private float knockback;

    //Se true colpo non può essere bloccato
    [SerializeField] private bool isUnblockable = false;

    //Lista dei collider già colpiti (per non colpire più volte lo stesso target nello stesso attacco)
    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        //Quando l’arma viene riattivata, resetta la lista dei collider già colpiti
        alreadyCollidedWith.Clear();
    }

    //Quando l’arma entra in contatto con un altro collider
    private void OnTriggerEnter(Collider other)
    {
        //Evita di colpire se stessa
        if (other == myCollider) return;

        //Se ha già colpito questo collider, ignora
        if (alreadyCollidedWith.Contains(other)) return;

        //Aggiunge il collider alla lista di quelli già colpiti
        alreadyCollidedWith.Add(other);

        //Controlla se l’oggetto colpito ha una Health
        if (other.TryGetComponent<Health>(out Health health))
        {
            //Se è un player controlla lo stato di blocco
            if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
            {
                //Se il player sta bloccando e l’attacco è imparabile rimuove l’invulnerabilità
                if (player.IsBlocking && isUnblockable)
                {
                    player.Health.SetInvulnerable(false);
                }
            }

            //Applica il danno
            health.DealDamage(damage);

            //Se ha un ForceReceiver, applica il knockback
            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                //Direzione dall’arma al bersaglio
                Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
                //Applica la forza di knockback
                forceReceiver.AddForce(direction * knockback);
            }
        }
    }

    //Set normale dell’attacco (parabile)
    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
        isUnblockable = false;
    }

    //Set dell’attacco imparabile
    public void SetUnblockableAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
        isUnblockable = true;
    }

    //Reset dell’attacco (torna parabile per sicurezza)
    public void ResetUnblockable()
    {
        this.isUnblockable = false;
    }
}
