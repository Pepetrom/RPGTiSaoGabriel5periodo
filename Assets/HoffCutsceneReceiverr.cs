using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoffCutsceneReceiverr : MonoBehaviour
{
    public void ChangeCamera(int index)
    {
        CutsceneManager.instance.ChangeCamera(index);
    }
    public void StartCrocAnim()
    {
        CutsceneManager.instance.StartCrocAnim();
    }
}
