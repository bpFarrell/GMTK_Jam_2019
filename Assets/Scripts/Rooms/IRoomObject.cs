using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoomObject
{ 
    void OnRoomTransitionOut(Room room);
    void OnRoomTransitionIn(Room room);
}
