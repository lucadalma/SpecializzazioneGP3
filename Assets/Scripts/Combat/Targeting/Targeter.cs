using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe Targeter: gestisce i bersagli disponibili e la selezione del target più vicino
public class Targeter : MonoBehaviour
{
    //Riferimento al CinemachineTargetGroup per gestire i target della camera
    [SerializeField]
    private CinemachineTargetGroup cineTargetGroup;

    //Riferimento alla camera principale
    private Camera mainCamera;

    //Lista dei target rilevati
    private List<Target> targets = new List<Target>();

    //Target attualmente selezionato
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        //Prende la Camera principale della scena
        mainCamera = Camera.main;
    }

    //Quando entra in collisione con un target
    private void OnTriggerEnter(Collider other)
    {
        //Se l'oggetto non ha il componente Target, esce
        if (!other.TryGetComponent<Target>(out Target target)) return;

        //Aggiunge il target alla lista
        targets.Add(target);
        //Si iscrive all'evento di distruzione del target
        target.OnDestroyed += RemoveTarget;
    }

    //Quando esce dal trigger di un target
    private void OnTriggerExit(Collider other)
    {
        //Se l'oggetto non ha il componente Target, esce
        if (!other.TryGetComponent<Target>(out Target target)) return;

        //Rimuove il target dalla lista
        targets.Remove(target);
        //Chiama la rimozione del target
        RemoveTarget(target);
    }

    //Funzione per selezionare il target più vicino al centro dello schermo
    public bool SelectTarget()
    {
        //Se non ci sono target disponibili, esce
        if (targets.Count == 0) return false;

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        //Controlla tutti i target disponibili
        foreach (Target target in targets)
        {
            //Converte la posizione del target in coordinate viewport
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            //Se il target non è visibile, continua col prossimo
            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            //Calcola la distanza del target dal centro dello schermo
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        //Se non trova un target valido, esce
        if (closestTarget == null)
            return false;

        //Imposta il target corrente
        CurrentTarget = closestTarget;
        //Aggiunge il target al gruppo di Cinemachine
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    //Funzione per annullare il target corrente
    public void Cancel()
    {
        if (CurrentTarget == null) return;
        //Rimuove il target dal CinemachineTargetGroup
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    //Funzione per rimuovere un target dalla lista
    private void RemoveTarget(Target target)
    {
        //Se il target rimosso è quello corrente
        if (CurrentTarget == target)
        {
            //Lo rimuove dal CinemachineTargetGroup e lo resetta
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        //Si disiscrive dall'evento di distruzione
        target.OnDestroyed -= RemoveTarget;
        //Rimuove il target dalla lista
        targets.Remove(target);
    }

    //Funzione che restituisce il nemico più vicino al player
    public Target ClosestEnemy()
    {
        Target closestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        //Posizione attuale del giocatore
        Vector3 currentPosition = gameObject.transform.position;

        //Cerca il target più vicino
        foreach (Target target in targets)
        {
            float distance = Vector3.Distance(currentPosition, target.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = target;
            }
        }
        return closestEnemy;
    }
}
