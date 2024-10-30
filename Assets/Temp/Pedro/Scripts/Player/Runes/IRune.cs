using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRune
{
    public void HitEffect();
    public void ProjectileHitEffect(Collider other);
    public void AtackCriticalEffect1();
    public void AtackCriticalEffect2();
    public void AtackCriticalEffect3();
    public void HeavyEffect1();
    public void HeavyEffect2();
    public void HeavyEffect3();
}
