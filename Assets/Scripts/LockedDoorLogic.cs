using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorLogic : MonoBehaviour
{
    public bool opening;
    public GameObject hinge;
    public Collider col;
    public float closeSpeed = 2;
    public float shakeAmount = 1;
    public float shakeSpeed = 20;
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
        if (door.dir == COMPASS_DIR.NORTHWEST || door.dir == COMPASS_DIR.SOUTHEAST)
            transform.Rotate(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!opening) return;
        Vector3 tempPos = hinge.transform.localPosition;
        tempPos.x = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        tempPos.y -= closeSpeed * Time.deltaTime;
        if (tempPos.y < -0.9f)
        {
            tempPos.y = -0.9f;
            opening = false;
            col.enabled = false;
        }
        hinge.transform.localPosition = tempPos;
    }
    public void Open()
    {
        opening = true;
    }
}
