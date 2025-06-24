using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;

public class KroBubble : MonoBehaviour
{
    public ProjectilesSO p;
    public VisualEffect explosion;
    GameObject player;
    Vector3 targetPosition, direction;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
    }
    private void LateUpdate()
    {
        targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        direction = (targetPosition - transform.position).normalized;
        transform.position += direction * p.speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HPBar.instance.TakeDamage(p.damage, transform);
            VisualEffect vfxI = Instantiate(explosion, transform.position, Quaternion.identity);
            vfxI.transform.SetParent(null);
            vfxI.Play();
            FMODAudioManager.instance.PlayOneShotAttached(FMODAudioManager.instance.bubbleExplosion, gameObject);
            Destroy(gameObject);
            Debug.Log("Player");
        }
        if (other.CompareTag("Scenario"))
        {
            VisualEffect vfxI = Instantiate(explosion, transform.position, Quaternion.identity);
            vfxI.transform.SetParent(null);
            vfxI.Play();
            explosion.Play();
            FMODAudioManager.instance.PlayOneShotAttached(FMODAudioManager.instance.bubbleExplosion, gameObject);
            Destroy(gameObject);
            Debug.Log("Cenário");
        }
    }
}
