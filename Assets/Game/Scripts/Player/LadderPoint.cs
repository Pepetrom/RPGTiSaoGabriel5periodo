using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderPoint : MonoBehaviour
{
    public bool top;
    public Ladder ladder;
    private void OnTriggerEnter(Collider other)
    {
        ladder.StartClimb();
    }
}
