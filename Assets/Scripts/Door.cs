using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IRoomObject
{
    public COMPASS_DIR dir;
    public Room parent;
    public LockedDoorLogic lockedDoor;
    public Room TargetRoom => this.parent.rooms.Get(this.dir);
    public bool locked = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + (Vector3.up * .05f), Vector3.one*.1f);
    }

    private void OnEnable()
    {
        BaseEnemy.OnEnemyClear += Unlock;
    }
    private void OnDisable()
    {

        BaseEnemy.OnEnemyClear -= Unlock;
    }
    private void Unlock() {
        locked = false;
        lockedDoor.Open();
        if (gameObject.activeInHierarchy)
        {
            GameObject go = Instantiate(Resources.Load("VSFXRumble")) as GameObject;
            go.transform.position = transform.position;
            go.transform.rotation = lockedDoor.transform.rotation;
        }
    }

    public void OnRoomTransitionIn(Room room)
    {
        parent = room;
        locked = true;
        if(parent == null)
        {
            Debug.LogWarning("TRANSITION_NULL_PARENT", this);
        }
    }

    public void OnRoomTransitionOut(Room room) {
        return; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked) return;
        if (parent == null) {
            Debug.LogWarning("NULL_PARENT", this); 
            return;
        }
        RoomManager.Instance.SetRoom(this);
        // probably should not be the authority of the door
        RuntimeManager.SetState(RuntimeManager.GameState.ROOMCHANGE);
    }

}
