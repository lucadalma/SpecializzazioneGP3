using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Classe EnemyAdaptiveAI: gestisce il comportamento adattivo del nemico
public class EnemyAdaptiveAI : MonoBehaviour
{
    //Possibili stili di comportamento del nemico
    [Header("Behaviour Scriptable Objects")]
    [SerializeField] private EnemyBehaviourSO balancedBehaviour;
    [SerializeField] private EnemyBehaviourSO aggressiveBehaviour;
    [SerializeField] private EnemyBehaviourSO defensiveBehaviour;

    [Header("Logo")]
    //Sprite per rappresentare lo stile attuale
    [SerializeField] private Image adaptiveLogo;

    [Header("Tracker")]
    //Riferimento al tracker delle azioni del player
    [SerializeField] private PlayerActionTracker playerActionTracker;

    [Header("StateMachine")]
    //Riferimento alla macchina a stati del nemico
    [SerializeField] private EnemyStateMachine enemyStateMachine;

    [Header("Adattamento")]
    //Intervallo di tempo tra una valutazione e l'altra
    [SerializeField] private float adaptInterval = 5f;

    //Timer per gestire l’intervallo di adattamento
    private float adaptTimer;

    //Stile corrente del nemico
    public EnemyBehaviourSO currentBehaviour;

    private void Start()
    {
        // Imposta il comportamento iniziale
        currentBehaviour = balancedBehaviour;
        enemyStateMachine.SetStrategy(currentBehaviour);
        UpdateVisualFeedback();
    }

    //Update viene chiamato ogni frame
    private void Update()
    {
        //Se mancano riferimenti, non fa nulla
        if (playerActionTracker == null || enemyStateMachine == null)
            return;

        //Aggiorna il timer
        adaptTimer += Time.deltaTime;

        //Quando passa l’intervallo stabilito, valuta il comportamento del player
        if (adaptTimer >= adaptInterval)
        {
            adaptTimer = 0f;
            EvaluatePlayerBehavior();
        }
    }

    //Funzione che valuta il comportamento del player e adatta lo stile del nemico
    private void EvaluatePlayerBehavior()
    {
        //Legge i dati dal tracker
        int attackCount = playerActionTracker.AttackCount.Value;
        int blockCount = playerActionTracker.BlockCount.Value;
        float timeAtDistance = playerActionTracker.TimeAtDistance.Value;
        float timeBlocking = playerActionTracker.TimeBlocking.Value;

        //Imposta lo stile di default come bilanciato
        EnemyBehaviourSO newBehaviour = balancedBehaviour;

        //Se il player attacca molto più di quanto blocchi nemico diventa difensivo
        if (attackCount - blockCount >= 3)
        {
            newBehaviour = defensiveBehaviour;
        }
        //Se il player blocca molto più di quanto attacca, o rimane troppo in distanza o blocca a lungo nemico diventa aggressivo
        else if (blockCount - attackCount >= 5 || timeBlocking > 5f || timeAtDistance > 5f)
        {
            newBehaviour = aggressiveBehaviour;
        }
        //Altrimenti resta bilanciato
        else
        {
            newBehaviour = balancedBehaviour;
        }

        //Se lo stile cambia, aggiorna la strategia e mostra log
        if (newBehaviour != currentBehaviour)
        {
            currentBehaviour = newBehaviour;
            enemyStateMachine.SetStrategy(currentBehaviour);
            UpdateVisualFeedback();
            Debug.Log($"[EnemyAdaptiveAI] Adattato stile: {currentBehaviour}");
        }

    }
    private void UpdateVisualFeedback()
    {
        if (adaptiveLogo != null)
        {
            adaptiveLogo.sprite = currentBehaviour.behaviourIcon;
        }
    }
}
