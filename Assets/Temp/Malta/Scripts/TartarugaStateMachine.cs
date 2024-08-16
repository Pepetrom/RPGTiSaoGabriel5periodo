using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TartarugaStateMachine : MonoBehaviour
{
    ITartarugaInterface state;
    public GameObject player;
    public float lookingRadius;
    public float patrollingRadius;
    public float patrollingCooldown;
    public NavMeshAgent tartaruga;
    public Vector3 patrolCenter; // Ponto central da área de patrulha

    void Start()
    {
        patrolCenter = transform.position; // Definindo o ponto inicial como o centro da área de patrulha
        SetState(new TartarugaPatrolState(this));
    }

    void FixedUpdate()
    {
        state?.OnUpdate();
    }
    public void SetState(ITartarugaInterface state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state.OnEnter();
    }

    #region
    public Vector3 TargetDir()
    {
        Vector3 dir = player.transform.position - transform.position;
        return dir;
    }

    public bool IsNearTarget()
    {
        return (TargetDir().magnitude < lookingRadius);
    }

    public void Patrolling(Vector3 center)
    {
        Vector3 pos = Random.insideUnitCircle * patrollingRadius;
        pos.z = pos.y;
        pos.y = 0;

        Vector3 patrolPosition = center + pos; // A posição de patrulha é baseada no centro da área de patrulha
        tartaruga.SetDestination(patrolPosition);
    }
    #endregion
}
