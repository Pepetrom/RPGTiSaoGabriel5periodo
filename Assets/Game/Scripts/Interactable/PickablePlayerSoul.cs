using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickablePlayerSoul : MonoBehaviour
{
    public int value = 50;
    bool started = false;
    Vector3 playerPos;
    Vector3 thisPos;
    float delta = 0;
    private void Start()
    {
        thisPos = transform.position;
        thisPos.y = 0;
    }
    private void FixedUpdate()
    {
        if (PlayerController.instance.playerIsDead) return;
        if (!started)
        {
            playerPos = PlayerController.instance.transform.position;
            playerPos.y = 0;
            if (Vector3.Distance(thisPos, playerPos) < 20)
            {
                started = true;
            }
        }
        else
        {
            delta += (Time.fixedDeltaTime /5);
            this.transform.position = Vector3.Lerp(this.transform.position, PlayerController.instance.transform.position, delta);
            if(delta >= 1 || Vector3.Distance(this.transform.position,PlayerController.instance.transform.position) < 1)
            {
                GameManager.instance.Score(value);
                PlayerController.instance.audioMan.PlayAudio(5);
                FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.item, transform.position);
                LittleReminder.instance.littleReminder("You recovered your Essence");
                Destroy(this.gameObject);
            }
        }
        
    }
}
