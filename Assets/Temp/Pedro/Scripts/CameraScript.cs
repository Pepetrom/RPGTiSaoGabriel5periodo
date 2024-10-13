using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    Vector3 cameraPlayerDiference;
    Vector3 newPosition;
    public float maxDistance = 1, speed = 1;
    public static CameraScript instance;

    // Variáveis para controlar a intensidade e a duração do shake
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 1.0f;
    private float shakeTime = 0.0f;

    //Zoom de combate
    public float zoomSpeed;
    public float targetZoom;
    private float currentZoom = 60;

    //Dissolver objetos do cenário

    private ObjectDisssolveShader dissolve; 


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cameraPlayerDiference = transform.position - PlayerController.instance.model.transform.position;
    }
    private void Update()
    {
        FindPlayer();
    }
    void FixedUpdate()
    {
        FollowPlayer();
        Shake();
        if(GameManager.instance.isCombat)
        {
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
            cam.fieldOfView = currentZoom;
        }
    }
    void FollowPlayer()
    {
        newPosition = transform.position;
        maxDistance = speed * Vector3.Distance(transform.position, PlayerController.instance.model.transform.position + cameraPlayerDiference);
        newPosition = Vector3.Lerp(transform.position, PlayerController.instance.model.transform.position + cameraPlayerDiference, maxDistance);
        //newPosition.y = PlayerController.instance.model.transform.position.y + cameraPlayerDiference.y;
        transform.position = newPosition;
    }
    void Shake()
    {
        if (shakeTime > 0)
        {
            transform.position = newPosition + Random.insideUnitSphere * shakeIntensity;
            shakeTime -= Time.deltaTime;
        }
    }

    // Função para iniciar o shake
    public void StartShake()
    {
        shakeTime = shakeDuration;
    }

    public void FindPlayer()
    {
        Vector3 dir = PlayerController.instance.model.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
                return;
            if(hit.collider.gameObject == PlayerController.instance.gameObject)
            {
                if(dissolve != null)
                {
                    dissolve.CanFade = false;
                }
            }
            else
            {
                dissolve = hit.collider.gameObject.GetComponent<ObjectDisssolveShader>();
                if(dissolve != null)
                {
                    dissolve.CanFade = true;
                }
            }
        }
    }
}
