using UnityEngine;

public class PlayerActionTracker : MonoBehaviour
{
    private PlayerStateMachine player;

    [Header("Attacco")]
    [SerializeField] public IntVariable AttackCount;

    [Header("Difesa")]
    [SerializeField] public IntVariable BlockCount;

    [Header("Distance")]
    [SerializeField] public FloatVariable TimeAtDistance;
    public float minDistance = 1.5f;
    public float maxDistance = 10f;

    //[SerializeField] private Transform enemyTransform;


    private void Awake()
    {
        if (GetComponent<PlayerStateMachine>() != null)
        {
            player = GetComponent<PlayerStateMachine>();
        }

        ResetStats();
    }

    public void RegisterAttack()
    {
        AttackCount.Value++;
    }

    public void RegisterBlock()
    {
        BlockCount.Value++;
    }

    private void Update()
    {
        if (player.Targeter.ClosestEnemy() != null)
        {
            Target enemy = player.Targeter.ClosestEnemy();
            float distance = Vector3.Distance(player.gameObject.transform.position, enemy.gameObject.transform.position);

            if (distance >= minDistance && distance <= maxDistance)
            {
                TimeAtDistance.Value += Time.deltaTime;
            }
        }
    }

    public void ResetStats()
    {
        AttackCount.Value = 0;
        BlockCount.Value = 0;
        TimeAtDistance.Value = 0f;
    }
}
