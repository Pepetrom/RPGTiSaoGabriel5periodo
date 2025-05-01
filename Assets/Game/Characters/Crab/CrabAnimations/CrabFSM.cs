using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
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
    [HideInInspector]public bool antecipation = false, end = false, combo = false, jump = false, fall = false, activate = false, hashitted = false;
    public Transform ground;

    [Header("CombatAtributes")]
    public float meleeRange;
    public int hp, damage;
    public float impulse, rotateSpeed;
    public SphereCollider jumpCollider;
    public GameObject fire;

    [Header("Fuzzy")]
    public int minJump, maxJump, fuzzyJump;
    public float jumpCount;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetState(new CrabStartState(this));
        FuzzyGate(out fuzzyJump, maxJump);
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
    public void FuzzyGate(out int a, int max)
    {
        a = Random.Range(1, maxJump);
    }
    public float FuzzyLogic(int fuzzy, int min, int max)
    {
        float v = fuzzy - min;
        float size = max - min;
        float fuzzification = fuzzy / size;
        return fuzzification;
    }
    #region ACTIONEVENTS
    public void Antecipation()
    {
        antecipation = true;
    }
    public void Activate()
    {
        activate = true;
    }
    public void Deactivate()
    {
        activate = false;
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

    #endregion
    #region MÉTODOS AUXILIARES FÍSICA
    public void RotateTowardsPlayer(float rotateSpeed)
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
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
    public void FallTowardsPlayer(float speed)
    {
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
