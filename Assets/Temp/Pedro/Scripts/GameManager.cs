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
        DamagePopUp dmp = Instantiate(damagePopUp, targetLocation.position, transform.rotation).GetComponent<DamagePopUp>();
        dmp.StartDamage(damageNumber);
    }
}
