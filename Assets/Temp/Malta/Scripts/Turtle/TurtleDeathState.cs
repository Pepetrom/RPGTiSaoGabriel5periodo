using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleDeathState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    private Material[] turtleMaterials;
    bool finished;
    public TurtleDeathState(TurtleStateMachine controller)
    {
        this.controller = controller;
        this.controller = controller;
        turtleMaterials = new Material[controller.turtleRenderers.Length];
        for (int i = 0; i < controller.turtleRenderers.Length; i++)
        {
            turtleMaterials[i] = controller.turtleRenderers[i].material;
        }
    }
    public void OnEnter()
    {
        controller.CannonKB(1.5f);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.attIdle)
        {
            controller.StartCoroutine(DissolveOverTime(2f));
        }
    }
    private IEnumerator DissolveOverTime(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(0, 1, elapsedTime / duration); // Alterar os valores conforme necessário
            for (int i = 0; i < turtleMaterials.Length; i++)
            {
                turtleMaterials[i].SetFloat("_Dissolve", dissolveValue); // Assume que a propriedade dissolve no shader é chamada _Dissolve
            }
            yield return null;
        }
        controller.DestroyTurtle(controller.gameObject);
    }
}
