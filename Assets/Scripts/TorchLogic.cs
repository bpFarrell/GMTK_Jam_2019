using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLogic : MonoBehaviour, IRoomObject
{
    public static List<TorchLogic> torches = new List<TorchLogic>();
    private void OnEnable()
    {
        if(!torches.Contains(this))
            torches.Add(this);
    }
    private void OnDisable()
    {
        torches.Remove(this);
    }

    public void OnRoomTransitionOut(Room room)
    {
        if (!torches.Contains(this))
            torches.Remove(this);
    }

    public void OnRoomTransitionIn(Room room)
    {
        if (!torches.Contains(this))
            torches.Add(this);
    }
    
    
}
