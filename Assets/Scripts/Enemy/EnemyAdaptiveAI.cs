using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAdaptiveAI : MonoBehaviour
{
    public enum EnemyBehaviour
    {
        Balanced,
        Aggressive,
        Defensive
    }

    [Header("Tracker")]
    [SerializeField] private PlayerActionTracker playerActionTracker;

    [Header("StateMachine")]
    [SerializeField] private EnemyStateMachine enemyStateMachine;

    [Header("Adattamento")]
    [SerializeField] private float adaptInterval = 5f;

    private float adaptTimer;

    public EnemyBehaviour currentStyle = EnemyBehaviour.Balanced;

    //public EnemyBehaviour CurrentStyle => currentStyle;

    private void Update()
    {
        if (playerActionTracker == null || enemyStateMachine == null)
            return;

        adaptTimer += Time.deltaTime;

        if (adaptTimer >= adaptInterval)
        {
            adaptTimer = 0f;
            EvaluatePlayerBehavior();
        }
    }

    private void EvaluatePlayerBehavior()
    {
        int attackCount = playerActionTracker.AttackCount.Value;
        int blockCount = playerActionTracker.BlockCount.Value;
        float timeAtDistance = playerActionTracker.TimeAtDistance.Value;

        EnemyBehaviour newStyle = EnemyBehaviour.Balanced;

        if (attackCount-blockCount >= 5)
        {
            newStyle = EnemyBehaviour.Defensive;
        }
        else if (blockCount-attackCount >= 5)
        {
            newStyle = EnemyBehaviour.Aggressive;
        }
        else
        {
            newStyle = EnemyBehaviour.Balanced;
        }

        if (newStyle != currentStyle)
        {
            currentStyle = newStyle;
            enemyStateMachine.SetStrategy(currentStyle);
            Debug.Log($"[EnemyAdaptiveAI] Adattato stile: {currentStyle}");
        }

    }
}
