using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : BaseEnemy,IRoomObject
{
    public enum ChargerState
    {
        Seeking,
        PrepCharge,
        Charging,
        Cooldown
    }
    public Vector3 startScale;
    public float moveSpeed = 1;
    public float chargeSpeed = 5;
    public float triggerDistance = 0.5f;
    public float chargeDist = 0.5f;
    public float chargeDelay = 0.5f;
    public float coolDown = 1;
    float stateTime;
    public ChargerState state = ChargerState.Seeking;
    public override void Hit()
    {
        Kill();
    }
    public void Start()
    {
        startScale = transform.localScale;
    }
    public void Update()
    {
        Vector3 playerPos = PlayerMovement.Instance.transform.position;
        switch (state)
        {
            case ChargerState.Seeking:
                transform.LookAt(playerPos);
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
                if (Vector3.Distance(transform.position, playerPos) < triggerDistance)
                {
                    state = ChargerState.PrepCharge;
                    stateTime = Time.time;
                }
                break;
            case ChargerState.PrepCharge:
                Vector3 temp = transform.localScale;
                temp.z *= 0.95f;
                transform.localScale = temp;
                if (stateTime + chargeDelay < Time.time)
                {
                    state = ChargerState.Charging;
                    stateTime = Time.time;
                    temp = startScale;
                    temp.z *= 1.25f;
                    transform.localScale = temp;
                }
                break;
            case ChargerState.Charging:

                transform.position += transform.forward * Time.deltaTime * chargeSpeed;
                if (stateTime + chargeDist < Time.time)
                {
                    state = ChargerState.Cooldown;
                    stateTime = Time.time;

                    transform.localScale = startScale;
                }
                break;
            case ChargerState.Cooldown:

                if (stateTime + coolDown < Time.time)
                {
                    state = ChargerState.Seeking;
                }
                break;
            default:
                break;
        }
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
