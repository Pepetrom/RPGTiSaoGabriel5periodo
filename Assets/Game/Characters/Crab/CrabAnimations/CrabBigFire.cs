using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBigFire : ICrabInterface
{
    CrabFSM controller;
    public CrabBigFire(CrabFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {
        controller.animator.SetBool("isJumping", false);
        controller.damage = 20;
        controller.transform.position = new Vector3(controller.secondStageLocation.transform.position.x, controller.secondStageLocation.transform.position.y, controller.secondStageLocation.transform.position.z);
    }

    public void OnExit()
    {
        controller.end = false;
        controller.bigWall = false;
    }

    public void OnUpdate()
    {
        if (controller.bigWall)
        {
            controller.FireWall();
        }
        if (controller.jump)
        {
            controller.RotateTowardsPlayer(5);
        }
        if (controller.activate)
        {
            controller.fireCircle.SetActive(true);
            controller.bigFire.SetActive(true);
        }
        else
        {
            controller.fireCircle.SetActive(false);
            controller.bigFire.SetActive(false);
        }
        if (!controller.end)
        {
            CameraScript.instance.CombatCamera(150, 0.6f, 1.2f);
        }
        if (controller.end)
        {
            controller.animator.SetBool("isJumping", true);
            controller.SetState(new CrabJumpState(controller));
        }
    }
}
