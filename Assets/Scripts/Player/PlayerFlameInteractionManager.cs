using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlameInteractionManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerCallbacks.PlayeEnteredDarknes += EnteredDark;
        PlayerCallbacks.PlayerEnteredLight += EnteredLight;
        PlayerCallbacks.PlayerStayedInDarkness += StayedInDark;

    }

    private void OnDisable()
    {

        PlayerCallbacks.PlayeEnteredDarknes -= EnteredDark;
        PlayerCallbacks.PlayerEnteredLight -= EnteredLight;
        PlayerCallbacks.PlayerStayedInDarkness -= StayedInDark;
    }

    private void EnteredDark()
    {
        Debug.LogFormat("Player EnteredDark ");
    }
    private void StayedInDark(float timeInDark)
    {
        Debug.LogFormat("Player stayedInDark for: {0}", timeInDark);
    }
    private void EnteredLight()
    {
        Debug.LogFormat("Player enteredLight ");
    }
}
