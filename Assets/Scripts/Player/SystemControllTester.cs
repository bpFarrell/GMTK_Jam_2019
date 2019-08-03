using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemControllTester : MonoBehaviour
{
    PlayerMovement movement;
    public bool once = true;

    private void Awake()
    {
        PlayerCallbacks.PlayerGoToDone += GoToDone; 
    }

    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.FindObjectOfType<PlayerMovement>();
        movement.Teleport(new Vector3(.8f, -.1f, .8f), Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (once) {
            movement.GoTo(Vector3.zero);
            once = false;
        }
    }

    void GoToDone() {
        movement.GiveControll();
        Debug.Log("DoToDone");
    }
}
