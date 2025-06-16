using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TurtleStateMachine : MonoBehaviour, IDamageable
{
    #region Variables
    ITurtleStateMachine state;
    [HideInInspector] public float sortedNumber;

    [Header("Elements")]
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public Enemy enemy;
    public Collider ownCollider;
    public GameObject hpCanvas;

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
    [HideInInspector] public bool end;
    [HideInInspector] public bool combo;
    [HideInInspector] public bool antecipation;
    [HideInInspector] public bool impulse;
    [HideInInspector] public bool cannonFire;
    [HideInInspector] public bool active;
    [HideInInspector] public bool combed = false;
    [HideInInspector] public Rigidbody rb;

    [Header("AttacksControllers")]
    public float attRate;
    public Collider rightHand, leftHand;
    public float damage;
    public bool hashitted = false;
    public bool isInCombat = false;

    //DeathShader
    public Renderer[] turtleRenderers;

    // Instancia
    public PatrolData patrolData; // Referência ao ScriptableObject
    public Transform[] patrolPoints;

    public AudioManager audioMan;

    //fuzzy 
    public int fuzzyCannon;

    //vfx
    public VisualEffect hitVFX, bloodVFX;
    public Transform hitPos;
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
        leftHand.enabled = false;
        rightHand.enabled = false;
        SetState(new TurtlePatrolState(this));
        FuzzyGate(out fuzzyCannon);
        //Debug.Log(fuzzyCannon);
    } 

    void Update()
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
    #region Métodos Auxiliares de Lógica
    public void SortNumber()
    {
        sortedNumber = Random.value;
    }
    public float FuzzyLogic(int fuzzy, int min, int max)
    {
        float v = fuzzy - min;
        float size = max - min;
        float fuzzification = fuzzy / size;
        return fuzzification;
    }
    public void FuzzyGate(out int a)
    {
        a = Random.Range(1, 40);
    }
    #region ActionEvents
    public void AttackIdle()
    {
        end = true;
    }
    public void Combo()
    {
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
    #endregion
    public void DestroyTurtle(GameObject hpCanvas)
    {
        Destroy(hpCanvas);
    }
    #endregion

    #region Métodos auxiliares de física
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
    public Vector3 Velocity()
    {
        Vector3 vel = rb.velocity;
        return vel;
    }
    public void Fire()
    {
        Instantiate(cannonBallPrefab, cannonPosition.position,cannonPosition.rotation);
        cannonExplosion.Play();
        CameraScript.instance.StartShake();
        cannonDust.Play();
    }
    public void Impulse(float kbforce)
    {
        rb.AddForce(PlayerController.instance.moveDirection.normalized * kbforce, ForceMode.Impulse);
    }
    public void KB(float kbforce)
    {
        rb.velocity = transform.forward.normalized * kbforce;
    }
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
    #endregion

    #region Status/Life
    void UpdateHPBar()
    {
        hpBar.value = Mathf.Lerp(hpBar.value,hp,lerpSpeed);
    }
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        hp -= damage;
        playerHit = true;
        PlayHitEffect();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if(hp <= 0)
        {
            animator.Play("Death");
            SetState(new TurtleDeathState(this));
        }
    }
    void PlayHitEffect()
    {
        Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;
        Vector3 vfxDir = directionToPlayer.normalized;
        Quaternion vfxRotation = Quaternion.LookRotation(vfxDir);
        VisualEffect hitVFXinstance = Instantiate(hitVFX, hitPos.position, Quaternion.identity);
        VisualEffect bloodVFXinstance = Instantiate(bloodVFX, hitPos.position, Quaternion.identity);
        hitVFXinstance.transform.rotation = vfxRotation;
        bloodVFXinstance.transform.rotation = vfxRotation;
        hitVFXinstance.Play();
        bloodVFXinstance.Play();
        hitVFXinstance.transform.SetParent(null);
        bloodVFXinstance.transform.SetParent(null);
    }
    #endregion
}
