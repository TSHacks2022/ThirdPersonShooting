using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    void Start()
    {
        nextFloor();
    }

    public void setFloor(int floor)
    {
        StaticData.floor = floor;
    }

    public int getFloor()
    {
        return StaticData.floor;
    }

    public void nextFloor()
    {
        StaticData.floor++;
        
    }
}
