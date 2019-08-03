﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Enemy"))
            PlayerCallbacks.PlayerHitByEnemy?.Invoke(other.gameObject);
            
    }
}