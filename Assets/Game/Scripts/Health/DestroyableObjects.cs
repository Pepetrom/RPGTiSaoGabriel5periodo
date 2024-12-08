using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObjects : MonoBehaviour
{
    [SerializeField] bool hasDrop = false;
    [SerializeField] GameObject drop, destroyEffect;
    public void Die()
    {
        GameManager.instance.audioMan.PlayAudioThere(transform, 0);
        Instantiate(destroyEffect, transform.position, transform.rotation);
        if(hasDrop) Instantiate(drop, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
