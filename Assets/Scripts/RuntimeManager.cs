﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeManager : SingletonMonoBehaviour<RuntimeManager>
{
    private static void StateLog(GameState state) {
        Debug.Log("StateChangeEvent | " + state);
    }
    public enum GameState
    {
        DUMMY,
        MAINMENU,
        NEWGAME,
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
                case GameState.NEWGAME:
                    return NewGame;
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
    public static readonly Transition NewGame = new Transition();
    public static readonly Transition Play = new Transition();
    public static readonly Transition RoomChange = new Transition();
    public static readonly Transition Death = new Transition();
    public static readonly Transition End = new Transition();

    [SerializeField]
    private GameState testState;
    [SerializeField]
    private bool setTestState = false;
        
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

    private void Update()
    {
        if (!setTestState) return;
        setTestState = false;
        state = testState;
    }
    private void OnEnable()
    {
        End.Enter += OnEnd;
    }
    private void OnDisable()
    {
        End.Enter -= OnEnd;
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
        testState = _state;
    }

    private void Start() {
        state = GameState.NEWGAME;
    }
    public static void SetState(GameState incState) {
        StateLog(incState);
        Instance.state = incState;
    }

    private void OnEnd()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}