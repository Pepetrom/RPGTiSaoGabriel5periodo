using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleHands : MonoBehaviour
{
    public TurtleStateMachine turtle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !turtle.hashitted)
        {
            HPBar.hpbarInstance.TakeDamage(turtle.damage);
            turtle.rightHand.gameObject.SetActive(false);
            turtle.leftHand.gameObject.SetActive(false);
            turtle.hashitted = true;
            Debug.Log("Colidiu");
        }
    }
}
