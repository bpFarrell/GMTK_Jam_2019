using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyClearEvent();
public abstract class BaseEnemy : MonoBehaviour
{
    public static EnemyClearEvent OnEnemyClear;

    private static int _activeEnemyCount;
    public static int activeEnemyCount {
        get {
            return _activeEnemyCount;
        }
        set {
            _activeEnemyCount = value;
            Debug.Log("Set active enemy count to " + value);
        }
    }
    public abstract void Hit();
    protected void Kill()
    {
        activeEnemyCount--;
        if (activeEnemyCount == 0)
        {
            Debug.Log("Enemies have been purged!!!");
            if (OnEnemyClear != null)
            {
                OnEnemyClear();
            }
        }
        Destroy(gameObject);
    }
    
}
