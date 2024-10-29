using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRune
{
    public void HitEffect();
    public void ProjectileHitEffect(Collider other);
    public void AtackEffect1();
    public void AtackEffect2();
    public void AtackEffect3();
    public void HeavyEffect1();
    public void HeavyEffect2();
    public void HeavyEffect3();
}
