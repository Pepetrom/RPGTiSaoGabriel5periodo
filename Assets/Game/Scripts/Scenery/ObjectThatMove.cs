using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThatMove : MonoBehaviour
{
    public Vector3[] place;
    Vector3 targetPosition;
    public float speed = 12;
    private void Awake()
    {
        targetPosition = transform.position;
    }
    
    public void ChangeLocation(int positionID)
    {
        targetPosition = place[positionID];
    }
    public void FixedUpdate()
    {
        if (targetPosition != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        }

    }
}
