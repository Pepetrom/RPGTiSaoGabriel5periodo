using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoDeath : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    private Material[] materials;
    bool finished;
    public PEscudoDeath(PorquinEscudoFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.porquinDeath, controller.transform.position);
        GameManager.instance.RemoveEnemy(controller.gameObject);
        controller.enemy.dead = true;
        PlayerController.instance.EnemyDied();
        controller.ownCollider.enabled = false;
        GameManager.instance.Score(100);
        controller.DestroyPorquin(controller.studioEventEmitter, controller.hpCanvas);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            if (QuestManager.instance.canDropMedicine)
            {
                QuestManager.instance.DropMedicine();
            }

        }
    }
}
