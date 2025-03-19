using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThatMove : MonoBehaviour
{
    public Vector3[] place;
    Vector3 targetPosition;
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
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 1) transform.position = targetPosition;
        }
    }
}
