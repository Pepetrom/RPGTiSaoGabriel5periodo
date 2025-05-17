using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast
    : MonoBehaviour
{
    public Transform player;
    public Vector3 offest;
    [SerializeField]
    private List<Transform> ObjectToHide = new List<Transform>();
    private List<Transform> ObjectToShow = new List<Transform>();
    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();


    private void LateUpdate()
    {
        ManageBlockingView();

        foreach (var obstruction in ObjectToHide)
        {
            HideObstruction(obstruction);
        }

        foreach (var obstruction in ObjectToShow)
        {
            ShowObstruction(obstruction);
        }
    }
    void ManageBlockingView()
    {
        Vector3 playerPosition = player.transform.position + offest;
        float characterDistance = Vector3.Distance(transform.position, playerPosition);
        int layerNumber = LayerMask.NameToLayer("dissolve");
        int layerMask = 1 << layerNumber;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, playerPosition - transform.position, characterDistance, layerMask);
        if (hits.Length > 0)
        {
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }

            ObjectToHide.Clear();

           
            foreach (var hit in hits)
            {
                Transform obstruction = hit.transform;
                ObjectToHide.Add(obstruction);
                ObjectToShow.Remove(obstruction);
                SetModeTransparent(obstruction);
            }
        }
        else
        {
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }

            ObjectToHide.Clear();

        }
    }

    private void HideObstruction(Transform obj)
    {
        var color = obj.GetComponent<Renderer>().material.color;
        color.a = Mathf.Max(0, color.a - WorldConfigurator.Instance.ObstructionFadingSpeed * Time.deltaTime);
        obj.GetComponent<Renderer>().material.color = color;

    }

    private void SetModeTransparent(Transform tr)
    {
        MeshRenderer renderer = tr.GetComponent<MeshRenderer>();
        Material originalMat = renderer.sharedMaterial;
        if (!originalMaterials.ContainsKey(tr))
        {
            originalMaterials.Add(tr, originalMat);
        }
        else
        {
            return;
        }
        Material materialTrans = new Material(WorldConfigurator.Instance.transparentMaterial);
        renderer.material = materialTrans;
        renderer.material.mainTexture = originalMat.mainTexture;
    }

    private void SetModeOpaque(Transform tr)
    {
        if (originalMaterials.ContainsKey(tr))
        {
            tr.GetComponent<MeshRenderer>().material = originalMaterials[tr];
            originalMaterials.Remove(tr);
        }

    }

    private void ShowObstruction(Transform obj)
    {
        var color = obj.GetComponent<Renderer>().material.color;
        color.a = Mathf.Min(1, color.a + WorldConfigurator.Instance.ObstructionFadingSpeed * Time.deltaTime);
        obj.GetComponent<Renderer>().material.color = color;
        if (Mathf.Approximately(color.a, 1f))
        {
            SetModeOpaque(obj);
        }
    }
}