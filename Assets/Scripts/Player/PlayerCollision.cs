using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
            PlayerCallbacks.PlayerHitByEnemy?.Invoke(other.gameObject);
            
    }
}
