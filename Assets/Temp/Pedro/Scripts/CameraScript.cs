using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Vector3 originalPosition;
    Vector3 cameraPlayerDiference;
    Vector3 newPosition;
    private void Start()
    {
        cameraPlayerDiference = transform.position - PlayerController.instance.model.transform.position;
    }
    void FixedUpdate()
    {
        newPosition = transform.position;
        newPosition = Vector3.Lerp(transform.position, PlayerController.instance.model.transform.position + cameraPlayerDiference, 0.2f);
        newPosition.y = PlayerController.instance.model.transform.position.y + cameraPlayerDiference.y;
        transform.position = newPosition;
    }
}
