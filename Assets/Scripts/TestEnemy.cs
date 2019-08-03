using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IEnemy
{
    public void Hit()
    {
        Destroy(gameObject);
    }

}
