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
    Material material;
    public Vector3 startScale;
    public float moveSpeed = 1;
    public float chargeSpeed = 5;
    public float triggerDistance = 0.5f;
    public float chargeDist = 0.5f;
    public float chargeDelay = 0.5f;
    public float coolDown = 1;
    public float turnSpeed = 70;
    public float angleChargeThreshold = 0.8f;
    float stateTime;
    bool isDying;
    bool isPaused;
    float timeOfDeath;
    public ChargerState state = ChargerState.Seeking;
    public override void Hit()
    {
        JuiceManager.Hang(3);
        isDying = true;
        timeOfDeath = Time.time;
        MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
        material = new Material(mr[0].material);
        for(int x = 0; x < mr.Length; x++)
        {
            mr[x].material = material;
        }
        Destroy(GetComponent<Collider>());
        //Destroy(gameObject);
    }
    public void Start()
    {
        startScale = transform.localScale;
    }
    public void Update()
    {

        if (isPaused) return;
        if (isDying)
        {
            Dying();
            return;
        }
        Vector3 playerPos = PlayerMovement.Instance.transform.position;
        switch (state)
        {
            case ChargerState.Seeking:
                Vector3 dir2Player = PlayerMovement.Instance.transform.position - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(dir2Player, Vector3.up),
                    turnSpeed*Time.deltaTime);
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
                if (Vector3.Distance(transform.position, playerPos) < triggerDistance&&
                    Vector3.Dot(transform.forward,dir2Player.normalized)>angleChargeThreshold)
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
    void Dying()
    {
        Vector3 scale = transform.localScale;
        scale.x += Time.deltaTime * 1;
        scale.y += Time.deltaTime * 1;
        scale.z += Time.deltaTime * 1;
        transform.localScale = scale;
        float t = (Time.time - timeOfDeath) * 3;
        material.SetFloat("_Death", t);
        if (t > 1)
            Kill();
    }
}
