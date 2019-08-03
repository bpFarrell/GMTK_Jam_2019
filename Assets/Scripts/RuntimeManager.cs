using System;
using UnityEngine;

public class RuntimeManager : SingletonMonoBehaviour<RuntimeManager>
{
    private static void StateLog(GameState state) {
        Debug.Log("StateChangeEvent | " + state);
    }
    public enum GameState
    {
        DUMMY,
        MAINMENU,
        PLAY,
        ROOMCHANGE,
        DEATH,
        END
    }
    

    public class Transition
    {
        public Action Enter;
        public Action Exit;

        public static Transition Get(GameState state)
        {
            switch (state)
            {
                case GameState.MAINMENU:
                    return MainMenu;
                    break;
                case GameState.PLAY:
                    return Play;
                    break;
                case GameState.ROOMCHANGE:
                    return RoomChange;
                    break;
                case GameState.DEATH:
                    return Death;
                    break;
                case GameState.END:
                    return End;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
    
    public static readonly Transition MainMenu = new Transition();
    public static readonly Transition Play = new Transition();
    public static readonly Transition RoomChange = new Transition();
    public static readonly Transition Death = new Transition();
    public static readonly Transition End = new Transition();

    private GameState _state = GameState.DUMMY;
    public GameState state
    {
        get { return _state;}
        private set
        {
            if (_state == value) return;
            ChangeState(value);
            _state = value;
        }
    }

    private void ChangeState(GameState newState)
    {
        Transition trans;
        if(_state != GameState.DUMMY) {
            trans = Transition.Get(state);
            trans.Exit?.Invoke();
        }
     
        _state = newState;
        
        trans = Transition.Get(newState);
        trans.Enter?.Invoke();
    }

    private void OnEnable() {
        state = GameState.PLAY;
    }

    public static void SetState(GameState incState) {
        StateLog(incState);
        Instance.state = incState;
    }
}