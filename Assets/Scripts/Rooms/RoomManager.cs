using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : SingletonMonoBehaviour<RoomManager>
{
    public Door firstDoor;
    public Door currentDoor;
    public List<Room> roomList; 

    private bool playerPositioned; 
    private bool flamePositioned; 

    public enum TransitionState
    {
        NONE,
        BEGINANIMATION,
        SWITCHROOMS,
        ENDANIMATION
    } 
    private TransitionState _state;

    public TransitionState state
    {
        get { return _state; }
        private set
        {
            if (_state == value) return;
            _state = value;
        }
    }
    private void OnEnable() {
        RuntimeManager.NewGame.Enter += NewGameEnter;
        RuntimeManager.RoomChange.Enter += RoomChangeEnter;
        RuntimeManager.RoomChange.Exit += RoomChangeExit;
        roomList = GetComponentsInChildren<Room>().ToList();
        foreach (Room room in roomList) {
            room.transform.position = Vector3.zero;
            room.Initialize();
            room.DeInitialize();
        }
    }

    private void NewGameEnter()
    {
        currentDoor = firstDoor;
        currentDoor.parent.Initialize();
        PlayerMovement.Instance.Teleport( currentDoor.TargetRoom.FindEdge(CardinalRooms.GetOppositeDir( currentDoor.dir )) - (CardinalRooms.GetDir(currentDoor.dir) * 0.5f), Vector3.zero);
        ChangeState(TransitionState.ENDANIMATION);
    }

    private void Update() {
        switch (state)
        {
            case TransitionState.NONE:
                return;
                break;
            case TransitionState.BEGINANIMATION:
                if (playerPositioned) {
                    ChangeState(TransitionState.SWITCHROOMS);
                    playerPositioned = false;
                }
                break;
            case TransitionState.SWITCHROOMS:
                break;
            case TransitionState.ENDANIMATION:
                if (playerPositioned)
                {
                    ChangeState(TransitionState.NONE);
                    playerPositioned = false;
                    RuntimeManager.SetState(RuntimeManager.GameState.PLAY);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void Update(TransitionState incState)
    {
        if (incState == TransitionState.NONE) return;
        if (incState == TransitionState.BEGINANIMATION)
        {
            Room room = currentDoor.parent;
            PlayerMovement.Instance.GoTo(room.FindEdge(currentDoor.dir) + (CardinalRooms.GetDir(currentDoor.dir) * 0.5f), () => playerPositioned = true);
        }
        else if (incState == TransitionState.SWITCHROOMS) {
            Vector3 foundEdge = currentDoor.TargetRoom.FindEdge(CardinalRooms.GetOppositeDir( currentDoor.dir )) - (CardinalRooms.GetDir(currentDoor.dir) * 0.5f);
            PlayerMovement.Instance.Teleport( foundEdge, Vector3.zero);
            FlameLogic.Instance.Teleport(foundEdge);
            currentDoor.TargetRoom.Initialize();
            currentDoor.parent.DeInitialize();
            CameraRoomScaler.Instance.SetCameraOrthographicSize(currentDoor.TargetRoom);
            ChangeState(TransitionState.ENDANIMATION);
        }
        else if (incState == TransitionState.ENDANIMATION)
        {
            Room room = currentDoor.TargetRoom;
            FlameLogic.Instance.TargetClosestToPosition(FlameLogic.Instance.transform.position);
            PlayerMovement.Instance.GoTo(room.FindEdge(CardinalRooms.GetOppositeDir( currentDoor.dir )) + (CardinalRooms.GetDir(currentDoor.dir) * 0.5f), () => playerPositioned = true);
        }
    }
    public void SetRoom(Door door) {
        currentDoor = door;
    }

    private void ChangeState(TransitionState newState) {
        state = newState;
        Update(newState);
    }
    private void RoomChangeEnter() {
        ChangeState(TransitionState.BEGINANIMATION);
        
    }
    private void RoomChangeExit()
    {
        ChangeState(TransitionState.NONE);
    }
}
