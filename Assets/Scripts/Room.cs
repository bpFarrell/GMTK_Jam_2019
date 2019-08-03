using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardinalRooms
{
    public Room nw = null;
    public Room ne = null;
    public Room se = null;
    public Room sw = null;

    public void Set(COMPASS_DIR dir, Room room) {
        switch (dir)
        {
            case COMPASS_DIR.NORTHWEST:
                nw = room;
                break;
            case COMPASS_DIR.NORTHEAST:
                ne = room;
                break;
            case COMPASS_DIR.SOUTHEAST:
                se = room;
                break;
            case COMPASS_DIR.SOUTHWEST:
                sw = room;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }
    public Room Get(int dir)
    {
        if (!System.Enum.IsDefined(typeof(COMPASS_DIR), (COMPASS_DIR) dir)) return null;
        return Get((COMPASS_DIR) dir);
    }
    public Room Get(COMPASS_DIR dir) {
        switch (dir)
        {
            case COMPASS_DIR.NORTHWEST:
                return nw;
                break;
            case COMPASS_DIR.NORTHEAST:
                return ne;
                break;
            case COMPASS_DIR.SOUTHEAST:
                return se;
                break;
            case COMPASS_DIR.SOUTHWEST:
                return sw;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }

    public static Vector3 GetDir(int dir)
    {
        if (!System.Enum.IsDefined(typeof(COMPASS_DIR), (COMPASS_DIR) dir)) return Vector3.zero;
        return GetDir((COMPASS_DIR) dir);
    }
    public static Vector3 GetDir(COMPASS_DIR dir) {
        switch (dir)
        {
            case COMPASS_DIR.NORTHWEST:
                return new Vector3(0,0,1);
                break;
            case COMPASS_DIR.NORTHEAST:
                return new Vector3(1,0,0);
                break;
            case COMPASS_DIR.SOUTHEAST:
                return new Vector3(0,0,-1);
                break;
            case COMPASS_DIR.SOUTHWEST:
                return new Vector3(-1,0,0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }
}

public enum COMPASS_DIR
{
    NORTHWEST = 0,
    NORTHEAST = 1,
    SOUTHEAST = 2,
    SOUTHWEST = 3
}

public class Room : MonoBehaviour
{
    public CardinalRooms rooms = new CardinalRooms();
    public IRoomObject[] roomObjects;
    
    // Characteristics
    public float size
    {
        get { return transform.localScale.x; }
    }

    // Start is called before the first frame update
    private void Start() {
        roomObjects = GetComponentsInChildren<IRoomObject>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < 4; i++)
        {
            Room room = rooms.Get(i);
            if (room == null) continue;
            Gizmos.DrawLine(transform.position, room.transform.position);
        }
    }

    public void Initialize()
    {
        foreach (IRoomObject roomObject in roomObjects)
        {
            roomObject.OnRoomTransitionIn(this);
        }
    }

    public void DeInitialize()
    {
        foreach (IRoomObject roomObject in roomObjects)
        {
            roomObject.OnRoomTransitionOut(this);
        }
    }

    private const string PREFABRESOURCEPATH_ROOM = "Room";
    private const string PREFABRESOURCEPATH_DOOR = "Door";
    
    public Room CreateNeighbor( COMPASS_DIR dir )
    {
        if (rooms.Get(dir) != null)
        {
            Debug.LogError("Room already created.");
            return null;
        }
        COMPASS_DIR opDir = (COMPASS_DIR) (((int) dir + 2) % 4);
        Room room = Instantiate(Resources.Load<Room>(PREFABRESOURCEPATH_ROOM));
        if (room == null) {
            Debug.LogError($"Resources.Load<Room>({PREFABRESOURCEPATH_ROOM});", this);
            return null;
        }

        Door door = Instantiate(Resources.Load<Door>(PREFABRESOURCEPATH_DOOR));
        if(door == null)
        {
            Debug.LogError($"Resources.Load<Room>({PREFABRESOURCEPATH_DOOR});", this);
            return null;
        }
        door.dir = dir;

        Door opDoor = Instantiate(Resources.Load<Door>(PREFABRESOURCEPATH_DOOR));
        if (opDoor == null)
        {
            Debug.LogError($"Resources.Load<Room>({PREFABRESOURCEPATH_DOOR});", this);
            return null;
        }
        opDoor.dir = opDir;

        room.transform.position = this.transform.position + (CardinalRooms.GetDir(dir) * (size + .5f));
        
        room.rooms.Set(opDir, this);
        this.rooms.Set(dir, room);
        
        return room;
    }
    
    // Room Statics
    
}
