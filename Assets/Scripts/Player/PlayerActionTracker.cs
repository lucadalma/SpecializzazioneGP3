using UnityEngine;

//Classe PlayerActionTracker: tiene traccia delle azioni del giocatore
public class PlayerActionTracker : MonoBehaviour
{
    //Riferimento allo state machine del player
    private PlayerStateMachine player;

    [Header("Attacco")]
    //Numero di attacchi effettuati dal player
    [SerializeField] public IntVariable AttackCount;

    [Header("Difesa")]
    //Numero di parate effettuate
    [SerializeField] public IntVariable BlockCount;
    //Tempo totale passato in parata
    [SerializeField] public FloatVariable TimeBlocking;

    [Header("Distanza")]
    //Tempo trascorso mantenendo una certa distanza dal nemico
    [SerializeField] public FloatVariable TimeAtDistance;
    //Distanza minima considerata "utile"
    public float minDistance = 1.5f;
    //Distanza massima considerata "utile"
    public float maxDistance = 10f;

    //Inizializzazione
    private void Awake()
    {
        //Se il player ha lo StateMachine, lo assegna
        if (GetComponent<PlayerStateMachine>() != null)
        {
            player = GetComponent<PlayerStateMachine>();
        }

        //Reset iniziale delle statistiche
        ResetStats();
    }

    //Registra un attacco del player
    public void RegisterAttack()
    {
        AttackCount.Value++;
    }

    //Registra una parata del player
    public void RegisterBlock()
    {
        BlockCount.Value++;
    }

    //Update viene chiamato ogni frame
    private void Update()
    {
        //Se esiste un nemico vicino
        if (player.Targeter.ClosestEnemy() != null)
        {
            Target enemy = player.Targeter.ClosestEnemy();
            float distance = Vector3.Distance(player.gameObject.transform.position, enemy.gameObject.transform.position);

            //Se il player rimane a una distanza considerata utile, incrementa il tempo
            if (distance >= minDistance && distance <= maxDistance)
            {
                TimeAtDistance.Value += Time.deltaTime;
            }
            else
            {
                //Se è fuori range, azzera il timer
                TimeAtDistance.Value = 0f;
            }
        }

        //Se il player sta bloccando, accumula tempo in parata
        if (player.IsBlocking)
        {
            TimeBlocking.Value += Time.deltaTime;
        }
        else
        {
            //Se non blocca, resetta il timer
            TimeBlocking.Value = 0;
        }
    }

    //Reset di tutte le statistiche
    public void ResetStats()
    {
        AttackCount.Value = 0;
        BlockCount.Value = 0;
        TimeAtDistance.Value = 0f;
    }
}
