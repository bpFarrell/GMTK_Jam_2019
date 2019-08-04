using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFightHandler : MonoBehaviour
{
    public float timeToStart = 5;
    public float pauseTime = 5; 

    public GameObject slimePrefab;
    public GameObject chargerPrefab;
    public GameObject enemies;
    public GameObject controllSlime;
    public Transform player;

    public GameObject SconceRound1; 
    public GameObject SconceRound2;
    public GameObject SconceRound3; 

    private PlayerMovement playerMovement; 

    private float startTime;
    private float endWaveTime = 0;
    private int waveOn = 0;

    private float waveWaitTime = 3.0f;

    private float timeBeforeFightStart = 5.0f;
    private bool hasFightStarted = false;
    private bool canFightStart = false; 

    private bool isGateOpen = false; 

    public enum WAVE_STATE
    {
        WAVE_START, 
        IN_WAVE,
        WAVE_END,
        WAVE_WAIT
    }

    private static WAVE_STATE _state = WAVE_STATE.WAVE_WAIT;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("FAILED_TO_GETPLAYER_MOVEMENT");
        }

        // todo, set this after room initialization
        startTime = Time.time;
        endWaveTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        /* we have completed the waves! */
        if (isGateOpen) return;

        if (playerMovement.playerState == PlayerMovement.state.PlayerControlled)
        {
            playerMovement.GoTo(Vector3.zero, null);
            canFightStart = true;
        }

        /* initialize the first steps for the wave sequence. */
        if (!hasFightStarted && canFightStart)
        {
            if(Time.time - startTime >= timeBeforeFightStart)
            {
                _state = WAVE_STATE.WAVE_START;
                Debug.Log("THE_FIGHT_HAS_STARTED!!"); 
                hasFightStarted = true;
            }
        }

        switch (_state)
        {
            case WAVE_STATE.WAVE_START:
                if (!hasFightStarted) return;
                OnWaveStart();
                break;
            case WAVE_STATE.WAVE_END:
                break;
            case WAVE_STATE.WAVE_WAIT:
                float timeSinceWaveEnd = Time.time - endWaveTime;
                if(timeSinceWaveEnd >= waveWaitTime) _state = WAVE_STATE.WAVE_START;
                break;
            case WAVE_STATE.IN_WAVE:
                if(enemies.transform.childCount == 1)
                {
                    _state = WAVE_STATE.WAVE_END;
                    OnWaveEnd();
                }
                break; 
            default:
                break;
        }
    }

    private void OnWaveEnd()
    {
        _state = WAVE_STATE.WAVE_WAIT;
        endWaveTime = Time.time;

        /* turn on a sconce on the wall, depending on round completetion. */
        if(waveOn == 0) { SconceRound1.SetActive(true); }
        if(waveOn == 1) { SconceRound2.SetActive(true); }
        if(waveOn == 2) { SconceRound3.SetActive(true); }

        waveOn++;
    }

    private void OnWaveStart()
    {
        _state = WAVE_STATE.IN_WAVE; 
        SendWave(waveOn);
    }

    private void OpenGate()
    {
        if (isGateOpen)
        {
            Debug.LogWarningFormat("GATE_OPEN"); 
            return;
        }
        if (enemies.transform.childCount != 1)
        {
            Debug.LogWarningFormat("GATE_CLOSED;emenies_exist {0}", enemies.transform.childCount);
            return;
        }
        controllSlime.GetComponent<BaseEnemySlime>().Hit();
        isGateOpen = true;

        playerMovement.GiveControll();
    }

    private void SendWave(int _waveOn)
    {
        Debug.LogFormat("SendWave;ON_WAVE_NUMBER_{0}", _waveOn);

        if (_waveOn == 0)
        {
            SpawnSlime(0, 2);
            SpawnSlime(0, -2);
            SpawnSlime(2, 0);
            SpawnSlime(-2, 0);
            return;
        }
        else if (_waveOn == 1)
        {

            SpawnCharger(0, 2);
            SpawnCharger(0, -4);
            SpawnCharger(7, 0);
            SpawnCharger(-8, 0);
            return; 

        }
        else if (_waveOn == 2)
        {
            SendWave(0);
            SendWave(1);
        }
        else if (_waveOn == 3)
        {
            OpenGate();
        }
    }

    private void SpawnSlime(float x, float z) {

        GameObject slime = Instantiate(slimePrefab, new Vector3(x, 0, z), Quaternion.identity, enemies.transform);
        slime.transform.LookAt(player);
        slime.GetComponent<BaseEnemySlime>().OnRoomTransitionIn(new Room());
    }

    private void SpawnCharger(float x, float z)
    {

        GameObject charger = Instantiate(chargerPrefab, new Vector3(x, 0, z), Quaternion.identity, enemies.transform);
        charger.transform.LookAt(player);
        charger.GetComponent<EnemyCharger>().OnRoomTransitionIn(new Room());
    }
}
