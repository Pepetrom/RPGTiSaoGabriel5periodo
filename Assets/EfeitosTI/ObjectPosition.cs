using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPosition : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Shader.SetGlobalVector("ObjectPosition", new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, this.transform.localScale.x));
    }
}
