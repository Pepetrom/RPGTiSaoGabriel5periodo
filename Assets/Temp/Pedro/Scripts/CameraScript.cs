using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    private ObjectDissolver dissolve;

    //PostProcess
    [Header("PostProcess")]
    public Volume mainVolume;
    private Vignette combatVignette;
    private float currentVig;
    public float targetVig;
    private Color currentVigColor;
    public Color targetVigColor;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cameraPlayerDiference = transform.position - PlayerController.instance.model.transform.position;

        // Tenta buscar o efeito de vinheta no Volume Profile
        if (mainVolume.profile.TryGet(out Vignette vignette))
        {
            combatVignette = vignette; // Armazena a referência ao efeito de vinheta
        }
        else
        {
            Debug.LogError("Efeito de vinheta não encontrado no Volume Profile.");
        }
    }

    private void Update()
    {
        FindPlayer();
        if (GameManager.instance.isCombat)
        {
            CombatCamera(targetZoom, targetVig);
        }
        else
        {
            CombatCamera(60, 0.2f);
            Debug.Log("Tome-lhe");
        }
    }
    void FixedUpdate()
    {
        FollowPlayer();
        Shake();
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
                dissolve = hit.collider.gameObject.GetComponent<ObjectDissolver>();
                if(dissolve != null)
                {
                    dissolve.CanFade = true;
                }
            }
        }
    }

    public void CombatCamera(float target, float value)
    {
        currentZoom = Mathf.Lerp(currentZoom, target, Time.deltaTime * zoomSpeed);
        cam.fieldOfView = currentZoom;
        currentVig = Mathf.Lerp(currentVig, value, Time.deltaTime * zoomSpeed);
        combatVignette.intensity.value = currentVig;
    }
    public void TakeHit(Color value)
    {
        currentVigColor = Color.Lerp(currentVigColor,value, Time.deltaTime * zoomSpeed * 20);
        combatVignette.color.value = value;
        StartCoroutine(ResetVignette()); 
    }
    IEnumerator ResetVignette()
    {
        yield return new WaitForSeconds(0.5f);
        while(currentVigColor != Color.black)
        {
            currentVigColor = Color.Lerp(currentVigColor, Color.black, Time.deltaTime * zoomSpeed);
            combatVignette.color.value = currentVigColor;
        }
        yield return null;
    }
}
