﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy :  IEnemy, IRoomObject
{
    public override void Hit()
    {
        Kill();
    }

    public void OnRoomTransitionIn(Room room)
    {
        activeEnemyCount++;
    }

    public void OnRoomTransitionOut(Room room)
    {
        activeEnemyCount--;
    }
}
