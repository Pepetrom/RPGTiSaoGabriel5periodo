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
    public bool jump = false;
    public bool fall = false;

    [Header("CombatAtributes")]
    public float meleeRange;
    public int hp;
    public float impulse;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetState(new CrabStartState(this));
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
    #region MÉTODOS AUXILIARES LÓGICA
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
    public void Jump()
    {
        jump = true;
    }
    public void StopJump()
    {
        jump = false;
    }
    public void Fall()
    {
        fall = true;
    }
    public void StopFall()
    {
        fall = false;
    }
    #endregion
    #region MÉTODOS AUXILIARES FÍSICA
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
    public void Impulse(float kbforce)
    {
        transform.position += transform.up * kbforce * Time.deltaTime;
    }
    public void FallTowardsPlayer(float fallSpeed)
    {
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, fallSpeed * Time.deltaTime);
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
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
