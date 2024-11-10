using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PorquinStateMachine : MonoBehaviour
{
    IPorquinStateMachine state;
    public Animator animator;
    public NavMeshAgent agent;
    
    [Header("Patrol")]
    public float patrollingRadius;
    public float patrollingCooldown;
    [HideInInspector] public Vector3 patrolCenter;
    [HideInInspector] public Vector3 patrolPosition;
    public float nextPatrolTime = 0f;
    public int currentPatrolIndex = 0;
    public PatrolData patrolData; // Referência ao ScriptableObject
    public Transform[] patrolPoints;
    private void Start()
    {
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

    public void Patrol()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    #endregion
}
