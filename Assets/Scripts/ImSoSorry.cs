using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImSoSorry : MonoBehaviour
{
    public List<string> appologyList = new List<string>();
    private Queue<string> appologyQueue = new Queue<string>();

    public float delay = 4f;
    private float count = 0f;

    private void OnEnable()
    {
        appologyQueue.Clear();
        foreach (string s in appologyList) {
            appologyQueue.Enqueue(s);
        }
    }

    private void Update()
    {
        if (appologyList.Count == 0) return;
        
        count += Time.deltaTime;
        if (count <= delay) return;
        
        count -= delay;
        VSFXLogic vsfx = Instantiate(Resources.Load<VSFXLogic>("VSFXSorry"), transform);
        vsfx.sound = appologyQueue.Dequeue();
    }
}
