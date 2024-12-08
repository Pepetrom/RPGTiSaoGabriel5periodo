using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThatMove : MonoBehaviour
{
    public static ObjectThatMove instance;
    public Vector3[] heights;
    Vector3 targetHeight;
    private void Awake()
    {
        instance = this;
        targetHeight = transform.position;
    }
    
    public void ChangeLocation(int heightId)
    {
        targetHeight = heights[heightId];
    }
    public void FixedUpdate()
    {
        if (targetHeight != transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, targetHeight, Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, targetHeight) < 1) transform.position = targetHeight;
        }
    }
}
