using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : SingletonMonoBehaviour<RoomManager>
{
    public Door currentDoor;

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
            if (value == state) return;
            
        }
    }
    private void Start() {
        RuntimeManager.RoomChange.Enter += RoomChangeEnter;
        RuntimeManager.RoomChange.Exit += RoomChangeExit;
    }

    private void Update() {
        switch (state)
        {
            case TransitionState.NONE:
                return;
                break;
            case TransitionState.BEGINANIMATION:
                if (playerPositioned) {
                    state = TransitionState.SWITCHROOMS;
                    playerPositioned = false;
                }
                break;
            case TransitionState.SWITCHROOMS:
                break;
            case TransitionState.ENDANIMATION:
                if (playerPositioned)
                {
                    state = TransitionState.NONE;
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
        if (incState == TransitionState.SWITCHROOMS) {
            PlayerMovement.Instance.Teleport( currentDoor.TargetRoom.FindEdge(CardinalRooms.GetOppositeDir( currentDoor.dir )) - (CardinalRooms.GetDir(currentDoor.dir) * 0.5f), Vector3.zero);
            //FlameLogic teleport goes here
            currentDoor.TargetRoom.Initialize();
            currentDoor.parent.DeInitialize();
            CameraRoomScaler.Instance.SetCameraOrthographicSize(currentDoor.TargetRoom);
        }
        if (incState == TransitionState.ENDANIMATION)
        {
            Room room = currentDoor.TargetRoom;
            PlayerMovement.Instance.GoTo(room.FindEdge(CardinalRooms.GetOppositeDir( currentDoor.dir )) + (CardinalRooms.GetDir(currentDoor.dir) * 0.5f), () => playerPositioned = true);
        }
    }
    public void SetRoom(Door door) {
        currentDoor = door;
    }

    private void ChangeSet(TransitionState newState) {
        Update(newState);
    }
    private void RoomChangeEnter() {
        state = TransitionState.BEGINANIMATION;
        
    }
    private void RoomChangeExit() {
        state = TransitionState.NONE;
    }
}
