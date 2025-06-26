using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PorquinEscudoFSM : MonoBehaviour, IDamageable, IChefe
{
    IPorquinEscudo state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public Rigidbody rb;
    public Slider hpBar;
    [HideInInspector] public bool antecipation = false, end = false, combo = false, action = false, action2 = false, action3 = false, activate = false, hashitted = false, eventS = false, bigWall = false;
    [Header("COMBAT")]
    public Collider shield;
    public CapsuleCollider ownCollider;
    public int randomValue,maxHP,hp, basicAtt, swingRate, moveAtt = 40;
    public float meleeRange, runRange, patrolRange, lerpSpeed, swingRange;
    public bool isShieldIsActive = false;
    [Header("VFX")]
    public VisualEffect hitVFX, blood;
    public Transform hitPos;

    //patrol
    public float nextPatrolTime = 0f, patrollingCooldown;
    public int currentPatrolIndex = 0;
    public PatrolData patrolData; // Referência ao ScriptableObject
    public Transform[] patrolPoints;

    //swing
    Vector3 velocity, lVelocity;
    float moveY, moveX, time, nextBomb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
            player = GameObject.FindWithTag("Player");

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
        SetState(new PEscudoIdle(this));
        hp = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = hp;
        shield.enabled = false;
    }
    void Update()
    {
        this.state?.OnUpdate();
    }
    public void SetState(IPorquinEscudo state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }
    #region MÉTODOS DE LÓGICA
    public void SortNumber()
    {
        randomValue = Random.Range(0, 100);
    }
    #region ACTION EVENTS
    public void Action()
    {
        action = true;
    }

    public void Action2()
    {
        action2 = true;
    }

    public void Activate()
    {
        activate = true;
    }

    public void Antecipation()
    {
        antecipation = true;
    }
    public void AntecipationFalse()
    {
        antecipation = false;
    }

    public void Combo()
    {
        combo = true;
    }

    public void Deactivate()
    {
        activate = false;
    }

    public void DeactivateSpecificEvent()
    {
        eventS = false;
    }

    public void End()
    {
        end = true;
    }

    public void SpecificEvent()
    {
        eventS = true;
    }

    public void StopAction()
    {
        action = false;
    }

    public void StopAction2()
    {
        action2 = false;
    }
    public void Action3()
    {
        action3 = true;
    }
    public void StopAction3()
    {
        action3 = false;
    }
    #endregion
    #endregion
    #region MÉTODOS DE FÍSICA
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
    public void SwingMove()
    {
        velocity = agent.velocity;
        lVelocity = transform.InverseTransformDirection(velocity);
        moveX = lVelocity.x;
        moveY = lVelocity.y;
        animator.SetFloat("MoveX", lVelocity.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", lVelocity.z, 0.1f, Time.deltaTime);
    }
    public void Patrol()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
    public void Patrolling()
    {
        if (Vector3.Distance(agent.transform.position, patrolPoints[currentPatrolIndex].position) <= 3f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
    #endregion
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        UIItems.instance.bossCurrentHP -= damage;
        if (UIItems.instance.bossCurrentHP <= 0)
        {

        }
        if (isShieldIsActive)
        {
            FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.takingDamage, transform.position);
            PlayHitEffect();
        }
        else
        {
            FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.porquinBlood, transform.position);
            PlayHitEffect();
        }
    }
    public void CameraShakeEscudeba()
    {
        CameraScript.instance.StartShake();
    }

    public void PlaySoundAttached(string path)
    {
        FMODAudioManager.instance.PlaySoundAttached(path);
    }
    public void PlayHitEffect()
    {
        Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;
        Vector3 vfxDir = directionToPlayer.normalized;
        Quaternion vfxRotation = Quaternion.LookRotation(vfxDir);
        VisualEffect hitVFXinstance = Instantiate(hitVFX, hitPos.position, Quaternion.identity);
        hitVFXinstance.transform.rotation = vfxRotation;
        hitVFXinstance.Play();
        hitVFXinstance.transform.SetParent(null);
    }
}
