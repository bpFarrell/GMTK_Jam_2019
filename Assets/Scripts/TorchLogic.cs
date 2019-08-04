using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLogic : MonoBehaviour, IRoomObject
{
    public static List<TorchLogic> torches = new List<TorchLogic>();
    public Light light;
    public float speed = 3;
    public float speedDecay = 0.5f;
    public float range = 2;

    public bool turningOn;
    private void OnEnable()
    {
        if(!torches.Contains(this))
            torches.Add(this);
        light.enabled = false;
    }
    private void OnDisable()
    {
        torches.Remove(this);
    }


    public void OnRoomTransitionOut(Room room)
    {
        if (torches.Contains(this))
            torches.Remove(this);
    }

    public void OnRoomTransitionIn(Room room)
    {
        if (!torches.Contains(this))
            torches.Add(this);
    }
    public void Update()
    {
        if (light.isActiveAndEnabled && !turningOn)
        {
            light.range -= Time.deltaTime * speedDecay;
            if (light.range < 0)
                light.enabled = false;
        }
        else if (turningOn)
        {
            light.range += Time.deltaTime * speed;
            light.range = Mathf.Clamp(light.range,0, range);
        }
    }
    public void SetOn()
    {
        light.enabled = true;
        turningOn = true;
    }
    public void SetOff()
    {

        turningOn = false;
    }
}
