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

    // Vari�veis para controlar a intensidade e a dura��o do shake
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 1.0f;
    private float shakeTime = 0.0f;

    //Zoom de combate
    public float zoomSpeed;
    public float targetZoom, targetPosition;
    private float currentZoom = 60, currentPosition, currentAngle;

    //Dissolver objetos do cen�rio

    private ObjectDissolver dissolve;
    public LayerMask layerMask;

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

        // Post Process (Depois eu vou criar um script s� para o post process, vai dar uma limpada no camera script) 
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
            CombatCamera(targetZoom, targetVig, zoomSpeed);
        }
        else
        {
            CombatCamera(60, 0.2f, zoomSpeed);
            //Debug.Log("Tome-lhe");
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

    public void StartShake()
    {
        shakeTime = shakeDuration;
    }
    public void FindPlayer()
    {
        // Obt�m a dire��o do jogador em rela��o ao objeto atual
        Vector3 dir = PlayerController.instance.model.transform.position - transform.position;

        // Cria o raio partindo da posi��o atual em dire��o ao jogador
        Ray ray = new Ray(transform.position, dir);

        // Raycast apenas para os objetos na camada especificada
        RaycastHit[] hits = Physics.RaycastAll(ray, dir.magnitude, layerMask);
        Debug.DrawRay(transform.position, dir, Color.red);

        // Obt�m todos os dissolvers e reseta a propriedade CanFade
        ObjectDissolver[] dissolvers = FindObjectsOfType<ObjectDissolver>();
        foreach (var d in dissolvers)
        {
            d.CanFade = false;
        }

        // Posi��o da c�mera (ou ponto de vis�o)
        Vector3 cameraPosition = Camera.main.transform.position;

        // Verifica os objetos atingidos pelo Raycast
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null) continue;

            // Verifica se o objeto tem o componente ObjectDissolver
            ObjectDissolver dissolve = hit.collider.gameObject.GetComponent<ObjectDissolver>();
            if (dissolve != null)
            {
                // Garante que o objeto est� entre a c�mera e o jogador
                Vector3 playerDir = PlayerController.instance.model.transform.position - cameraPosition;
                Vector3 hitDir = hit.point - cameraPosition;

                // Se o objeto estiver entre a c�mera e o jogador, ativa o fade
                if (Vector3.Dot(playerDir.normalized, hitDir.normalized) > 0.99f) // Ajuste o valor, se necess�rio
                {
                    dissolve.CanFade = true;
                }
            }
        }
    }

    public void CombatCamera(float target, float value, float zoomSpeed)
    {
        currentZoom = Mathf.Lerp(currentZoom, target, Time.deltaTime * zoomSpeed);
        cam.fieldOfView = currentZoom;
        currentVig = Mathf.Lerp(currentVig, value, Time.deltaTime * zoomSpeed);
        combatVignette.intensity.value = currentVig;
    }
    /*public void CombatCamera(float target, float value, float zoomSpeed)
    {
        Vector3 localPos = cam.transform.localPosition;
        localPos.z = Mathf.Lerp(localPos.z, target, Time.deltaTime * zoomSpeed);
        cam.transform.localPosition = localPos;

        currentVig = Mathf.Lerp(currentVig, value, Time.deltaTime * zoomSpeed);
        combatVignette.intensity.value = currentVig;
    }*/
    public void RedVignette()
    {
        if (isHit)
        {
            currentVigColor = Color.Lerp(currentVigColor, targetVigColor, Time.deltaTime * zoomSpeed * 10);
            combatVignette.color.value = currentVigColor;
            aberration.intensity.value = 0.5f;
            whiteBalance.temperature.value = 60;
            whiteBalance.tint.value = 60;
            aberration.intensity.value = 0.5f;
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
            aberration.intensity.value = 0f;
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
