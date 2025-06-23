using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    public void StartPorquin()
    {
        CutsceneManager.instance.StartPorquin();
    }
    public void PlayVFX(VisualEffect vfx)
    {
        CutsceneManager.instance.PlayVFX();
    }
    public void ShakeCamera()
    {
        CutsceneManager.instance.ShakeCamera();
    }
    public void VolLight()
    {
        CutsceneManager.instance.VolLight();
    }
}
