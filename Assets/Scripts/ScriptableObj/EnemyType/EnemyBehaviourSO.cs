using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyBehavior", menuName = "Behaviors/Enemy Behavior")]
public class EnemyBehaviourSO : ScriptableObject
{
    [Header("Behaviour Settings")]
    public string behaviourName;
    public Sprite behaviourIcon;

    [Header("Probabilities")]
    [Range(0f, 1f)] public float blockChance = 0.5f;
    [Range(0f, 1f)] public float superAttackChance = 0.3f;
}
