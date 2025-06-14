using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinDeathState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    private Material[] materials;
    bool finished;
    public PorquinDeathState(PorquinStateMachine controller)
    {
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.porquinDeath, controller.transform.position);
        this.controller = controller;
        materials = new Material[controller.porquinRenderers.Length];
        for (int i = 0; i < controller.porquinRenderers.Length; i++)
        {
            materials[i] = controller.porquinRenderers[i].material;
        }
        GameManager.instance.RemoveEnemy(controller.gameObject);
    }
    public void OnEnter()
    {
        controller.GetComponent<Collider>().enabled = false;
        GameManager.instance.Score(100);
        controller.DestroyPorquin(controller.studioEventEmitter, controller.hpCanvas);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (controller.attIdle)
        {
            if (QuestManager.instance.canDropMedicine)
            {
                QuestManager.instance.DropMedicine();
            }
           
        }
    }
    /*private IEnumerator DissolveOverTime(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(0, 1, elapsedTime / duration); // Alterar os valores conforme necess�rio
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_Dissolve", dissolveValue); // Assume que a propriedade dissolve no shader � chamada _Dissolve
            }
            yield return null;
        }
        controller.DestroyPorquin(controller.gameObject);
    }*/
}
