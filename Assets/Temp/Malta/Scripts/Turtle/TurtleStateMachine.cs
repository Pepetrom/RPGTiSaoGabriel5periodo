using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleStateMachine : MonoBehaviour
{
    ITurtleStateMachine state;
    public float sortedNumber;
    public float attackSpeed;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;

    //bools de ataques
    public bool attIdle;
    public bool combo;
    public bool antecipation;
    public bool impulse;
    public bool combed = false;
    Rigidbody rb;

    public string lastAttack = ""; 
    public int attack2Counter = 0; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetState(new TurtleCombatIdleState(this));
    }
    void FixedUpdate()
    {
        state?.OnUpdate();
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
    public void AttackIdle()
    {
        Debug.Log("Voltei pro Idle");
        attIdle = true;
    }
    public void Combo()
    {
        Debug.Log("Combo");
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
    #endregion
    #region Métodos auxiliares de física
    public void Impulse()
    {
        rb.AddForce(transform.forward * attackSpeed,ForceMode.Impulse);
    }
    public void RotateTowardsPlayer()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRot,Time.deltaTime * 10);
    }
    #endregion
}
