using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TurtleStateMachine : MonoBehaviour
{
    #region Variables
    ITurtleStateMachine state;
    [HideInInspector] public float sortedNumber;

    [Header("Elements")]
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;

    [Header("Cannon")]
    public float maxCannonRange;
    public float minCannonRange;
    public float meleeRange;
    public GameObject cannonBallPrefab;
    public Transform cannonPosition;
    public ParticleSystem cannonExplosion, cannonDust;

    [Header("KB")]
    public float kbforce;
    [Header("Patrol")]
    public float patrollingRadius;
    public float patrollingCooldown;
    [HideInInspector] public Vector3 patrolCenter;
    [HideInInspector] public Vector3 patrolPosition;
    public float nextPatrolTime = 0f;
    public int currentPatrolIndex = 0;
    //public Transform[] patrolPoints;
    public float patrolDistance;
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
    [HideInInspector] public Rigidbody rb;

    [Header("AttacksControllers")]
    public string lastAttack = "";
    public int attack2Counter = 0;
    public float attackSpeed;
    public TurtleHands rightHand, leftHand;
    public float damage;
    public bool hashitted = false;
    public bool isInCombat = false;

    //DeathShader
    public Renderer[] turtleRenderers;

    // Instancia
    public PatrolData patrolData; // Refer�ncia ao ScriptableObject
    public Transform[] patrolPoints;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hp = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = maxHP;
        if (player == null)
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
            Debug.LogError("PatrolData n�o foi atribu�do ao inimigo!");
        }
        SetState(new TurtlePatrolState(this));
    } 

    void FixedUpdate()
    {
        state?.OnUpdate();
        //Tem que colocar esse comando abaixo no script de cada personagem
        animator.speed = GameManager.instance.actionTime;
        UpdateHPBar();
    }
    public void SetState(ITurtleStateMachine state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }
    #region M�todos Auxiliares de L�gica
    public void SortNumber()
    {
        sortedNumber = Random.value;
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
    public void Antecipation()
    {
        antecipation = true;
    }
    public void ImpulseEvent()
    {
        impulse = true;
    }
    public void CannonFire()
    {
        cannonFire = true;
    }
    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }
    public void DestroyTurtle(GameObject enemy)
    {
        Destroy(enemy);
    }
    public void Die()
    {
        SetState(new TurtleDeathState(this));
    }
    #endregion

    #region M�todos auxiliares de f�sica
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
    public Vector3 Velocity()
    {
        Vector3 vel = rb.velocity;
        return vel;
    }
    public void Fire()
    {
        Instantiate(cannonBallPrefab, cannonPosition.position,cannonPosition.rotation);
        cannonExplosion.Play();
        CannonKB(2);
        CameraScript.instance.StartShake();
        cannonDust.Play();
        //Debug.Log("Atirou");
    }
    public void Impulse(float kbforce)
    {
        rb.AddForce(PlayerController.instance.moveDirection.normalized * kbforce, ForceMode.Impulse);
    }
    public void CannonKB(float value)
    {
        rb.AddForce(-transform.forward.normalized * (kbforce * value),ForceMode.Impulse);
    }
    /*public void Patrolling(Vector3 center)
    {
        Vector3 pos = Random.insideUnitCircle.normalized * Random.Range(0.9f, 1f) * patrollingRadius;
        pos.z = pos.y;
        pos.y = 0;
        patrolPosition = center + pos;
    }*/
    public void Patrolling()
    {
        if (Vector3.Distance(agent.transform.position, patrolPoints[currentPatrolIndex].position) <= 3f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    public void Patrol()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    public void RotateTowards()
    {
        Vector3 direction = patrolPoints[currentPatrolIndex].position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }
    #endregion

    #region Status/Life
    void UpdateHPBar()
    {
        hpBar.value = Mathf.Lerp(hpBar.value,hp,lerpSpeed);
    }
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        Impulse(kbforce * knockbackStrenght);
        hp -= damage;
        playerHit = true;
        hit.Play();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if(hp <= 0)
        {
            animator.SetBool("Dead", true);
            Die();
        }
    }
    #endregion
}
