using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField]
    CharacterController controller; // Riferimento al CharacterController del personaggio

    [SerializeField]
    private NavMeshAgent Agent; // Riferimento opzionale al NavMeshAgent per i nemici controllati da NavMesh

    [SerializeField]
    private float drag; // Valore di smorzamento per ridurre gradualmente le forze applicate

    private Vector3 dampingVelocity; // Variabile ausiliaria per SmoothDamp
    private Vector3 impact;          // Vettore che accumula le forze esterne (knockback, attacchi)
    private float verticalVelocity;  // Velocità verticale (gravità + salto)

    // Proprietà pubblica per ottenere il movimento totale da applicare al CharacterController
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        // Se stiamo cadendo e siamo a terra, resettiamo la velocità verticale
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // Altrimenti applichiamo la gravità normalmente
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // Smorzamento graduale della forza applicata (impact)
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        // Gestione dell'Agent per nemici
        if (Agent != null)
        {
            // Se l'impatto è quasi nullo, riattiviamo l'Agent
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                Agent.enabled = true;
            }
        }
    }

    // Metodo per applicare una forza esterna (knockback, spinta)
    public void AddForce(Vector3 force)
    {
        impact += force;

        // Disabilitiamo temporaneamente l'Agent per permettere al personaggio di essere mosso dalla forza
        if (Agent != null)
        {
            Agent.enabled = false;
        }
    }

    // Metodo per far saltare il personaggio aggiungendo un impulso verticale
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
