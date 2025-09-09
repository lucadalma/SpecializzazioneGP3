using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;
    private float knockback;

    [SerializeField] private bool isUnblockable = false;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;

        if (alreadyCollidedWith.Contains(other)) return;

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
            {
                if (player.IsBlocking && isUnblockable)
                {
                    player.Health.SetInvulnerable(false);
                }
            }

            health.DealDamage(damage);

            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }
    }

    // Set normale
    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
        isUnblockable = false;
    }

    // Set con colpo imparabile
    public void SetUnblockableAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
        isUnblockable = true;
    }

    // Per sicurezza puoi anche resettarlo
    public void ResetUnblockable()
    {
        this.isUnblockable = false;
    }
}
