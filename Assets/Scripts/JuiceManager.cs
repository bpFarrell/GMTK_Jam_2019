using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceManager : MonoBehaviour
{
    private static JuiceManager instance;
    private int framesLeftFrozen = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (framesLeftFrozen == 0) return;
        framesLeftFrozen--;

        if(framesLeftFrozen == 0)
        {
            Time.timeScale = 1;
        }
    }
    public static void Hang(int frames)
    {
        Time.timeScale = 0;
        instance.framesLeftFrozen = frames + 1;
    }
}
