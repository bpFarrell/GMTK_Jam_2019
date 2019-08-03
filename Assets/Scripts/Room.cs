using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalRooms
{
    Room nw = null;
    Room ne = null;
    Room se = null;
    Room sw = null;
}

public class Room : MonoBehaviour
{

    CardinalRooms rooms = new CardinalRooms();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
