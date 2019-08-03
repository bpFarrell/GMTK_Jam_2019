using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _Instance = null;
    public static T Instance
    {
        get
        {
            if (_Instance != null) return _Instance;
            T[] runtimes = FindObjectsOfType<T>();
            if (runtimes.Length > 1) {
                Debug.LogError($"Multiple {typeof(T)} found in scene");
                for (int i = runtimes.Length - 1; i > 0; i--) {
                    Destroy(runtimes[i].gameObject);
                }
            }
            if (runtimes.Length == 0)
            {
                Debug.LogError($"No {typeof(T)} found in scene");
                return null;
            }
            _Instance = runtimes[0];
            return _Instance;
        }
    }
}
