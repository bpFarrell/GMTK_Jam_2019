using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallbacks : MonoBehaviour
{
    public static Action<GameObject> PlayerHitByEnemy;
    public static Action PlayeEnteredDarknes;
    public static Action PlayerEnteredLight;
    public static Action<float> PlayerStayedInDarkness; 
    

}
