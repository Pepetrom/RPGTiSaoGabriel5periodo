using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int lifeTotal = 3;
    public int lifeActual = 3;
    private void Start()
    {
        lifeActual = lifeTotal;
    }
    public void TakeDamage(int damage)
    {
        lifeActual -= damage;
        GameManager.instance.SpawnDamageNumber(damage, transform);
        if(lifeActual <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
