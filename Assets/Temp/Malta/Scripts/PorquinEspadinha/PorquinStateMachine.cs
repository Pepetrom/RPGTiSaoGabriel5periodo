using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PorquinStateMachine : MonoBehaviour
{
    IPorquinStateMachine state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    Rigidbody rb;
    
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
    public float kbForce;
    
    //bools de ataques
    [HideInInspector] public bool attIdle;
    [HideInInspector] public bool combo;
    [HideInInspector] public bool antecipation;
    [HideInInspector] public bool impulse;
    [HideInInspector] public bool cannonFire;
    [HideInInspector] public bool active;
    [HideInInspector] public bool combed = false;
    private void Start()
    {
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
    }
    private void FixedUpdate()
    {
        animator.speed = GameManager.instance.actionTime;
        state?.OnUpdate();
    }
    public void SetState(IPorquinStateMachine state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }
    #region Métodos auxiliares de lógica
    public void Patrolling()
    {
        if (Vector3.Distance(agent.transform.position, patrolPoints[currentPatrolIndex].position) <= 3f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
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
    public void Impulse()
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
    #endregion
}
