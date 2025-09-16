using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Questa classe gestisce l'attivazione e disattivazione della logica della weapon (es. collider, danno, effetti)
public class WeaponHandler : MonoBehaviour
{
    // Riferimento al GameObject che contiene la logica della weapon
    // Può essere un oggetto figlio con Collider, ParticleSystem o script WeaponDamage
    [SerializeField] private GameObject weaponLogic;

    // Funzione pubblica per abilitare la weapon
    public void EnableWeapon()
    {
        // Attiva il GameObject della weapon
        weaponLogic.SetActive(true);
    }

    // Funzione pubblica per disabilitare la weapon
    public void DisableWeapon()
    {
        // Disattiva il GameObject della weapon
        weaponLogic.SetActive(false);
    }
}
