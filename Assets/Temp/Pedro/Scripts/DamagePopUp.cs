using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public TextMesh damageText;
    public float damageTime = 2;
    float damageTimer = 0;
    public void StartDamage(int damageNumber)
    {
        damageText.text = "" + damageNumber;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        damageTimer += Time.fixedDeltaTime;
        transform.position += Vector3.up * Time.fixedDeltaTime * 10;
        if(damageTimer >= damageTime)
        {
            Destroy(this.gameObject);
        }
    }
}
