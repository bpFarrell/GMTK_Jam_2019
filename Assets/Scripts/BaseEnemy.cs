using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyClearEvent();
public abstract class BaseEnemy : MonoBehaviour
{
    public static EnemyClearEvent OnEnemyClear;
    public bool isDead = false; 
    
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

    public static bool registered = false;

    public void OnEnable() {
        if (registered) return;
        registered = true;
        RuntimeManager.Play.Enter += CheckEnemyCount;
    }

    private void CheckEnemyCount()
    {
        if (_activeEnemyCount == 0) OnEnemyClear?.Invoke();
    }

    private void OnDisable()
    {
        if (!registered) return;
        
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
        isDead = true;
        Destroy(gameObject);
    }
    
}
