using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject damagePopUp;
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
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
}
