using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Classe EnemyAdaptiveAI: gestisce il comportamento adattivo del nemico
public class EnemyAdaptiveAI : MonoBehaviour
{
    //Enumerazione dei possibili stili di comportamento del nemico
    public enum EnemyBehaviour
    {
        Balanced,   // Comportamento bilanciato
        Aggressive, // Comportamento aggressivo
        Defensive   // Comportamento difensivo
    }

    [Header("Logo")]
    //Lista degli sprite per rappresentare lo stile attuale
    [SerializeField] private List<Sprite> sprites;

    [Header("Tracker")]
    //Riferimento al tracker delle azioni del player
    [SerializeField] private PlayerActionTracker playerActionTracker;

    [Header("StateMachine")]
    //Riferimento alla macchina a stati del nemico
    [SerializeField] private EnemyStateMachine enemyStateMachine;

    [Header("Adattamento")]
    //Intervallo di tempo tra una valutazione e l'altra
    [SerializeField] private float adaptInterval = 5f;

    //Logo UI che mostra lo stile attuale
    [SerializeField] private Image adaptiveLogo;

    //Timer per gestire l’intervallo di adattamento
    private float adaptTimer;

    //Stile corrente del nemico
    public EnemyBehaviour currentStyle = EnemyBehaviour.Balanced;

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
        EnemyBehaviour newStyle = EnemyBehaviour.Balanced;

        //Se il player attacca molto più di quanto blocchi nemico diventa difensivo
        if (attackCount - blockCount >= 3)
        {
            newStyle = EnemyBehaviour.Defensive;
            adaptiveLogo.sprite = sprites[1];
        }
        //Se il player blocca molto più di quanto attacca, o rimane troppo in distanza o blocca a lungo nemico diventa aggressivo
        else if (blockCount - attackCount >= 5 || timeBlocking > 5f || timeAtDistance > 5f)
        {
            newStyle = EnemyBehaviour.Aggressive;
            adaptiveLogo.sprite = sprites[0];
        }
        //Altrimenti resta bilanciato
        else
        {
            newStyle = EnemyBehaviour.Balanced;
            adaptiveLogo.sprite = sprites[2];
        }

        //Se lo stile cambia, aggiorna la strategia e mostra log
        if (newStyle != currentStyle)
        {
            currentStyle = newStyle;
            enemyStateMachine.SetStrategy(currentStyle);
            Debug.Log($"[EnemyAdaptiveAI] Adattato stile: {currentStyle}");
        }
    }
}
