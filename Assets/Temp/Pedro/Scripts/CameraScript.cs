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
    private ChromaticAberration aberration;
    private WhiteBalance whiteBalance;
    private float currentVig;
    public float targetVig;
    private Color currentVigColor;
    public Color targetVigColor;
    private bool isHit = false;
    private float timeSinceHit = 0f; 
    public float resetDelay = 0.5f; 

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cameraPlayerDiference = transform.position - PlayerController.instance.model.transform.position;

        // Post Process (Depois eu vou criar um script só para o post process, vai dar uma limpada no camera script) 
        if (mainVolume.profile.TryGet(out Vignette vignette))
        {
            combatVignette = vignette;
        }
        if(mainVolume.profile.TryGet(out ChromaticAberration ab))
        {
            aberration = ab;
        }
        if(mainVolume.profile.TryGet(out WhiteBalance wb))
        {
            whiteBalance = wb;
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
        RedVignette();
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
    public void RedVignette()
    {
        if (isHit)
        {
            currentVigColor = Color.Lerp(currentVigColor, targetVigColor, Time.deltaTime * zoomSpeed * 10);
            combatVignette.color.value = currentVigColor;
            aberration.intensity.value = 0.5f;
            whiteBalance.temperature.value = 60;
            whiteBalance.tint.value = 60;
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit >= resetDelay)
            {
                isHit = false; 
            }
        }
        else
        {
            currentVigColor = Color.Lerp(currentVigColor, Color.black, Time.deltaTime * zoomSpeed * 20);
            combatVignette.color.value = currentVigColor;
            aberration.intensity.value = 0f;
            whiteBalance.temperature.value = 0;
            whiteBalance.tint.value = 0;
        }
    }
    public void TakeHit(Color hitColor)
    {
        // Definir como estado de "dano"
        isHit = true;
        timeSinceHit = 0f; // Reiniciar o temporizador
        targetVigColor = hitColor; // Atualizar para a cor do impacto
    }
}
