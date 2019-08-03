using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlameInteraction : MonoBehaviour
{
    public float distanceTillOutOfLigt = 0.5f;
    public float timeEnteredDarkness = 0;

    private GameObject flame;
    private bool wasInLigt = true;
    private float nextInDarknessUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        flame = FindObjectOfType<FlameLogic>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (flame != null)
        {
            bool inLigt = distanceTillOutOfLigt > Vector3.Distance(this.transform.position, flame.transform.position)
                          && CheckLOS();
            if (wasInLigt && inLigt)
            {

            }
            else if (wasInLigt && !inLigt)
            {
                timeEnteredDarkness = Time.time;
                wasInLigt = false;
                nextInDarknessUpdate = 0;
                PlayerCallbacks.PlayeEnteredDarknes?.Invoke();
            }
            else if (!wasInLigt && inLigt)
            {
                wasInLigt = true;
                PlayerCallbacks.PlayerEnteredLight?.Invoke();
            }
            else
            {
                float timeInDarkness = Time.time - timeEnteredDarkness;
                if (nextInDarknessUpdate < timeInDarkness)
                {
                    PlayerCallbacks.PlayerStayedInDarkness?.Invoke(nextInDarknessUpdate);
                    nextInDarknessUpdate = nextInDarknessUpdate + 1;
                }
            }
        }
       
    }

    bool CheckLOS() {
        RaycastHit hit;
        var rayDirection = flame.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, ~((1 << 13) + (1 << 9))))
        {
            if (ReferenceEquals(hit.transform, flame.transform))
            {
                return true;
            }
            return false;
        }
        return false;

    }
}
