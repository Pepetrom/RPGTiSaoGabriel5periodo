using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class CrabFSM : MonoBehaviour, IDamageable
{
    #region VARIABLES
    ICrabInterface state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    Rigidbody rb;

    //logic
    public int randomValue;
    public bool antecipation = false;
    public bool end = false;
    public bool combo = false;

    [Header("CombatAtributes")]
    public float meleeRange;
    public int hp;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetState(new CrabIdleState(this));
    }
    private void FixedUpdate()
    {
        this.state?.OnUpdate();
    }
    public void SetState(ICrabInterface state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }
    #region M�TODOS AUXILIARES L�GICA
    public void SortNumber()
    {
        randomValue = Random.Range(0, 100);
    }
    public void Antecipation()
    {
        antecipation = true;
    }
    public void Deactivate()
    {

    }
    public void End()
    {
        end = true;
    }
    public void Combo()
    {
        combo = true;
    }
    #endregion
    #region M�TODOS AUXILIARES F�SICA
    public void RotateTowardsPlayer()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10);
        }
    }
    public Vector3 TargetDir()
    {
        Vector3 dir = player.transform.position - transform.position;
        return dir;
    }

    public void TakeDamage(int damage, float knockbackStrenght)
    {
        hp -= damage;
        //playerHit = true;
        //hit.Play();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if (hp <= 0)
        {
            //animator.SetBool("death", true);
            //animator.SetBool("stun", false);
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
