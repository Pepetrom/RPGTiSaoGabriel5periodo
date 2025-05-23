using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorItSelf : MonoBehaviour
{
    public ObjectThatMove o;
    private int locationID;

    private void Update()
    {
        if(o.transform.position == o.place[0])
        {
            locationID = 0;
        }
        else
        {
            locationID = 1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WhereToGo();
            Debug.Log("Elevei");
        }
    }
    void WhereToGo()
    {
        switch (locationID)
        {
            case 0:
                o.ChangeLocation(1); break;
            case 1:
                o.ChangeLocation(0); break;
        }
    }
}
