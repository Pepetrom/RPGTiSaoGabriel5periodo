using UnityEngine;

public class WallCutterController : MonoBehaviour {
    public Transform playerTarget; // O centro do jogador que a câmera foca
    public Camera gameCamera;
    public float cutoutRadius = 2.0f;
    public float edgeSoftness = 0.5f; // Quão suave é a borda do corte (0-1)
    public LayerMask wallLayerMask; // Layer das paredes

    private static readonly int MaskWorldPositionID = Shader.PropertyToID("_MaskWorldPosition");
    private static readonly int MaskRadiusID = Shader.PropertyToID("_MaskRadius");
    private static readonly int MaskEdgeSoftnessID = Shader.PropertyToID("_MaskEdgeSoftness");
    private static readonly int EnableWallCutoutID = Shader.PropertyToID("_EnableWallCutout");

    void Awake() {
        if (gameCamera == null) gameCamera = GetComponent<Camera>();
        if (playerTarget == null && PlayerController.instance != null && PlayerController.instance.model != null) {
            playerTarget = PlayerController.instance.model.transform;
        }
    }

    void Update() {
        if (playerTarget == null || gameCamera == null) {
            Shader.SetGlobalInt(EnableWallCutoutID, 0);
            return;
        }

        Vector3 cameraPosition = gameCamera.transform.position;
        Vector3 playerPosition = playerTarget.position;
        Vector3 directionToPlayer = (playerPosition - cameraPosition).normalized;
        float distanceToPlayer = Vector3.Distance(cameraPosition, playerPosition);

        RaycastHit[] hits = Physics.RaycastAll(cameraPosition, directionToPlayer, distanceToPlayer, wallLayerMask);
        bool wallFound = false;

        // Encontrar o hit mais próximo que não seja o próprio jogador (se o jogador estiver na wallLayerMask)
        float closestWallHitDistance = float.MaxValue;
        RaycastHit closestWallHit = new RaycastHit();

        foreach (RaycastHit hit in hits) {
            if (hit.transform == playerTarget || (playerTarget.IsChildOf(hit.transform))) // Ignora o jogador
                continue;

            if (hit.distance < closestWallHitDistance) {
                closestWallHitDistance = hit.distance;
                closestWallHit = hit;
                wallFound = true;
            }
        }

        // Seu código de Raycast original:
        // foreach (RaycastHit hit in hits) { ... if (Vector3.Dot ... ) dissolve.CanFade = true; }
        // Adapte a lógica acima para encontrar o 'hit.point' relevante na parede.

        if (wallFound) {
            // O ponto 'closestWallHit.point' é o centro do nosso círculo na superfície da parede
            Shader.SetGlobalVector(MaskWorldPositionID, closestWallHit.point);
            Shader.SetGlobalFloat(MaskRadiusID, cutoutRadius);
            // edgeSoftness é uma fração do raio. Shader espera valor absoluto.
            Shader.SetGlobalFloat(MaskEdgeSoftnessID, cutoutRadius * Mathf.Clamp01(edgeSoftness));
            Shader.SetGlobalInt(EnableWallCutoutID, 1);
        }
        else {
            Shader.SetGlobalInt(EnableWallCutoutID, 0);
        }
    }
}