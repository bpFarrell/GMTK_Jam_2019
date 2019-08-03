using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    private void Awake()
    {
        PlayerCallbacks.PlayerHitByEnemy += HitByEnemyAction;
    }

    private void OnDisable()
    {
        PlayerCallbacks.PlayerHitByEnemy -= HitByEnemyAction;
    }

    private void HitByEnemyAction(GameObject enemyGO) {
        Debug.LogFormat("Player Hit by enemy {0}", enemyGO.name);
    }
}
