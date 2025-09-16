using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe Ragdoll: gestisce l’attivazione/disattivazione del ragdoll per un personaggio
public class Ragdoll : MonoBehaviour
{
    //Riferimento all’animator del personaggio
    [SerializeField]
    private Animator animator;

    //Riferimento al CharacterController
    [SerializeField]
    private CharacterController controller;

    //Array di tutti i collider dei figli
    private Collider[] allColliders;

    //Array di tutti i rigidbody dei figli
    private Rigidbody[] allRigidbodies;

    private void Start()
    {
        //Prende tutti i collider dei figli (anche quelli disattivi)
        allColliders = GetComponentsInChildren<Collider>(true);

        //Prende tutti i rigidbody dei figli (anche quelli disattivi)
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        //All’avvio disattiva il ragdoll
        ToggleRagdoll(false);
    }

    //Funzione per attivare/disattivare il ragdoll
    public void ToggleRagdoll(bool isRagdoll)
    {
        //Gestione dei collider
        foreach (Collider collider in allColliders)
        {
            //Solo i collider con tag "Ragdoll"
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                //Abilita/disabilita i collider in base allo stato del ragdoll
                collider.enabled = isRagdoll;
            }
        }

        //Gestione dei rigidbody
        foreach (Rigidbody rigidbody in allRigidbodies)
        {
            //Solo i rigidbody con tag "Ragdoll"
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                //Se ragdoll attivo rigidbody fisico
                //Se ragdoll disattivo rigidbody kinematic (controllato da animator/CharacterController)
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        //Disattiva CharacterController e Animator quando il ragdoll è attivo
        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
