using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public Text damageText;
    public float damageTime = 1;
    float damageTimer = 0;
    public void StartDamage(int damageNumber)
    {
        damageText.text = "" + damageNumber;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        damageTimer += Time.fixedDeltaTime;
        transform.position += Vector3.up * Time.fixedDeltaTime;
        if(damageTimer >= damageTime)
        {
            Destroy(this.gameObject);
        }
    }
}
