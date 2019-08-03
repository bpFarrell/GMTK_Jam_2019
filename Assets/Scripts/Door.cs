using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IRoomObject
{
    public COMPASS_DIR dir;
    public Room parent;

    public Room TargetRoom => this.parent.rooms.Get(this.dir);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + (Vector3.up * .05f), Vector3.one*.1f);
    }

    public void OnRoomTransitionIn(Room room)
    {
        parent = room; 
        if(parent == null)
        {
            Debug.LogWarning("NULL_PARENT", this);
        }
    }

    public void OnRoomTransitionOut(Room room)
    {
        parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parent == null)
        {
            Debug.LogWarning("NULL_PARENT", this); 
            return;
        }
        RoomManager.Instance.SetRoom(this);
        // transition to room pointed by dir. 
    }

}
