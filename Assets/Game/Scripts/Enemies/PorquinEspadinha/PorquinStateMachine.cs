using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PorquinStateMachine : MonoBehaviour, IDamageable
{
    IPorquinStateMachine state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    Rigidbody rb;
    public Enemy enemy;

    [Header("Patrol")]
    //public float patrollingRadius;
    public float patrollingCooldown;
    public float patrolRange;
    [HideInInspector] public Vector3 patrolCenter;
    [HideInInspector] public Vector3 patrolPosition;
    public float nextPatrolTime = 0f;
    public int currentPatrolIndex = 0;
    public PatrolData patrolData; // Referência ao ScriptableObject
    public Transform[] patrolPoints;

    [Header("COMBAT")]
    [HideInInspector]public bool isInCombat = false;
    public float meleeRange;
    public float safeRange;
    public float kbForce;
    public string lastAttack = "";
    public int attack2Counter = 0;
    public float attackSpeed;
    public TurtleHands rightHand, leftHand;
    public float damage;
    public bool hashitted = false;
    public GameObject sword;
    public int fullCombatCounter;
    public Collider selfCollider;
    // FUZZY
    [HideInInspector] public int fuzzyDash, fuzzySwing;
    public int minDash, maxDash, minSwing, maxSwing;
    public float swingRange;

    [Header("Status")]
    public int maxHP;
    public int hp;
    public Slider hpBar;
    public float lerpSpeed;
    public bool playerHit = false;
    public ParticleSystem hit;

    //bools de ataques
    [HideInInspector] public bool attIdle;
    [HideInInspector] public bool combo;
    [HideInInspector] public bool antecipation;
    [HideInInspector] public bool impulse;
    [HideInInspector] public bool cannonFire;
    [HideInInspector] public bool active;
    [HideInInspector] public bool combed = false;

    public Renderer[] porquinRenderers;

    public float sortedNumber;

    public AudioManager audioMan;
    Vector3 velocity, lVelocity;
    private void Start()
    {
        attack2Counter = 0;
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (patrolData != null)
        {
            patrolPoints = new Transform[patrolData.patrolPositions.Length];

            for (int i = 0; i < patrolData.patrolPositions.Length; i++)
            {
                GameObject patrolPoint = new GameObject("PatrolPoint_" + i);
                patrolPoint.transform.position = patrolData.patrolPositions[i];
                patrolPoints[i] = patrolPoint.transform;
            }
        }
        else
        {
            Debug.LogError("PatrolData não foi atribuído ao inimigo!");
        }
        rb = GetComponent<Rigidbody>(); 
        SetState(new PorquinPatrolState(this));
        hp = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = hp;
        sword.gameObject.SetActive(false);
        //fuzzy
        FuzzyGate(out fuzzyDash, out fuzzySwing);
        Debug.Log($"{fuzzySwing}");
    }
    private void FixedUpdate()
    {
        animator.speed = GameManager.instance.actionTime;
        state?.OnUpdate();
        UpdateHPBar();
    }
    public void SetState(IPorquinStateMachine state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }
    #region Métodos auxiliares de lógica
    public void SortNumber()
    {
        sortedNumber = Random.value;
    }
    public void FuzzyGate(out int a, out int b)
    {
        a = Random.Range(1, 101);
        b = Random.Range(1, 101);
    }
    public float FuzzyLogic(int fuzzy, int min, int max)
    {
        float v = fuzzy - min;
        float size = max - min;
        float fuzzification = fuzzy/size;
        return fuzzification;
    }
    public void AttackIdle()
    {
        //Debug.Log("Voltei pro Idle");
        attIdle = true;
    }
    public void Combo()
    {
        //Debug.Log("Combo");
        combo = true;
    }
    public void ImpulseEvent()
    {
       impulse = true;
    }
    public void Antecipation()
    {
        antecipation = true;
    }
    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }

    public void Patrol()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
    public void Die()
    {
        SetState(new PorquinDeathState(this));
    }
    public void DestroyPorquin(GameObject porquin)
    {
        Destroy(porquin);
    }
    #endregion
    #region Métodos auxiliares de física
    public Vector3 TargetDir()
    {
        Vector3 dir = player.transform.position - transform.position;
        return dir;
    }
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
    public void AttacksKB(float value)
    {
        rb.AddForce(transform.forward.normalized * value, ForceMode.Impulse);
    }
    public void Patrolling()
    {
        if (Vector3.Distance(agent.transform.position, patrolPoints[currentPatrolIndex].position) <= 3f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
    public void Impulse(float kbforce)
    {
        rb.AddForce(PlayerController.instance.moveDirection.normalized * kbforce, ForceMode.Impulse);
    }
    public void KB(float value)
    {
        rb.AddForce(-transform.forward.normalized * (kbForce * value), ForceMode.Impulse);
    }
    void UpdateHPBar()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, hp, lerpSpeed);
    }
    public Vector3 Swing()
    {
        Vector3 randomDirection = Random.insideUnitSphere * swingRange;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, swingRange, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
    public bool HasReachedDestination()
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
    /*public void SwingMove()
    {
        velocity = agent.velocity;
        lVelocity = transform.InverseTransformDirection(velocity);
        float moveX = lVelocity.x;
        float moveY = lVelocity.y;
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
    }*/

    public void TakeDamage(int damage, float knockbackStrenght)
    {
        Impulse(kbForce * knockbackStrenght);
        hp -= damage;
        playerHit = true;
        hit.Play();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if (hp <= 0)
        {
            animator.SetBool("death", true);
            animator.SetBool("stun", false);
            Die();
        }
    }
    #endregion
}
