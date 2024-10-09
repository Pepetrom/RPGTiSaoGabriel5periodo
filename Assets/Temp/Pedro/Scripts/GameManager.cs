using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject damagePopUp;
    public static GameManager instance;
    public float actionTime = 1;
    private void Awake()
    {
        instance = this;
        actionTime = 1;
    }
    public void SpawnDamageNumber(int damageNumber, Transform targetLocation)
    {
        GameObject temp = Instantiate(damagePopUp, targetLocation.position, transform.rotation);
        DamagePopUp dmp = temp.GetComponent<DamagePopUp>();
        dmp.StartDamage(damageNumber);
    }
    public void ChangeRune(int rune)
    {
        PlayerController.instance.actualRune = rune;
    }
    public void CallHitStop(float tempo)
    {
        StartCoroutine(HitStop(tempo));
    }
    IEnumerator HitStop(float tempoHitStop)
    {
        actionTime = 0;
        yield return new WaitForSeconds(tempoHitStop);
        actionTime = 1;
    }
}
