using System;
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

    public static COMPASS_DIR GetOppositeDir(COMPASS_DIR dir)
    {
        return GetOppositeDir((int)dir);
    }
    public static COMPASS_DIR GetOppositeDir(int dir)
    {
        return (COMPASS_DIR) ((dir + 2) % 4);
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
    public Transform torchContainer;
    public Transform doorContainer;
    public MeshRenderer floorBounds;

    public CardinalRooms rooms = new CardinalRooms();
    private IRoomObject[] _roomObjects;

    public IRoomObject[] roomObjects
    {
        get
        {
            if (_roomObjects != null)
            {
                return _roomObjects;
            }

            _roomObjects = GetComponentsInChildren<IRoomObject>();
            return _roomObjects;
        }
    }

// Characteristics
    public float size
    {
        get { return Mathf.Max(floorBounds.bounds.size.x, floorBounds.bounds.size.z); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < 4; i++)
        {
            Room room = rooms.Get(i);
            COMPASS_DIR opDir = CardinalRooms.GetOppositeDir(i);
            if (room == null) continue;
            Vector3 myEdgeCenter = this.FindEdge(i);
            Vector3 toEdgeCenter = room.FindEdge(opDir);
            Gizmos.DrawLine(myEdgeCenter, toEdgeCenter);
        }
    }

    public Vector3 FindEdge(COMPASS_DIR dir)
    {
        Vector3 vectorMask = CardinalRooms.GetDir(dir);
        Vector3 val = new Vector3(vectorMask.x * floorBounds.bounds.size.x, 0, vectorMask.z * floorBounds.bounds.size.z);
        return this.transform.position + (val * .5f);
    }
    public Vector3 FindEdge(int dir)
    {
        Vector3 vectorMask = CardinalRooms.GetDir(dir);
        Vector3 val = new Vector3(vectorMask.x * floorBounds.bounds.size.x, 0, vectorMask.z * floorBounds.bounds.size.z);
        return this.transform.position + (val * .5f);
    }
    
    public void Initialize()
    {
        gameObject.SetActive(true);
        if (roomObjects == null) return; 
        foreach (IRoomObject roomObject in roomObjects)
        {
            roomObject.OnRoomTransitionIn(this);
        }
    }

    public void DeInitialize()
    {
        gameObject.SetActive(false);
        if (roomObjects == null) return;
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

        COMPASS_DIR opDir = CardinalRooms.GetOppositeDir(dir);
        Room newRoom = Instantiate(Resources.Load<Room>(PREFABRESOURCEPATH_ROOM), this.transform.parent);
        if (newRoom == null) {
            Debug.LogError($"Resources.Load<Room>({PREFABRESOURCEPATH_ROOM});", this);
            return null;
        }

        Door myDoor = Instantiate(Resources.Load<Door>(PREFABRESOURCEPATH_DOOR), doorContainer);
        if(myDoor == null)
        {
            Debug.LogError($"Resources.Load<Room>({PREFABRESOURCEPATH_DOOR});", this);
            return null;
        }

        myDoor.transform.position = this.transform.position + (CardinalRooms.GetDir(dir) * (size * .5f));
        myDoor.parent = this;
        myDoor.dir = dir;
        if (myDoor.dir == COMPASS_DIR.NORTHEAST || myDoor.dir == COMPASS_DIR.SOUTHWEST)
            myDoor.transform.Rotate(0, 90, 0);

        Door newDoor = Instantiate(Resources.Load<Door>(PREFABRESOURCEPATH_DOOR), newRoom.doorContainer);
        if (newDoor == null)
        {
            Debug.LogError($"Resources.Load<Room>({PREFABRESOURCEPATH_DOOR});", this);
            return null;
        }
        newDoor.transform.position = newRoom.transform.position + (CardinalRooms.GetDir(opDir) * (size * .5f));
        newDoor.parent = newRoom;
        newDoor.dir = opDir;
        if (newDoor.dir == COMPASS_DIR.NORTHEAST || newDoor.dir == COMPASS_DIR.SOUTHWEST)
            newDoor.transform.Rotate(0, -90, 0);

        newRoom.transform.position = this.transform.position + (CardinalRooms.GetDir(dir) * (size + .5f));
        
        newRoom.rooms.Set(opDir, this);
        this.rooms.Set(dir, newRoom);
        
        return newRoom;
    }
    // Room Statics
}