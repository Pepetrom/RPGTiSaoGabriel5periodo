using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public TextMesh numberText;
    public float numberTime = 2;
    float numberTimer = 0;
    public void StartNumber(int damageNumber, Color color)
    {
        numberText.text = "" + damageNumber;
        numberText.color = color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        numberTimer += Time.fixedDeltaTime;
        transform.position += Vector3.up * Time.fixedDeltaTime * 10;
        if(numberTimer >= numberTime)
        {
            Destroy(this.gameObject);
        }
    }
}
