using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKiller : MonoBehaviour
{
    public float killTime = 1;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + killTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
