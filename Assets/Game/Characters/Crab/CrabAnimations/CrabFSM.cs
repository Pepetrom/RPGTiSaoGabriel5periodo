using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabFSM : MonoBehaviour
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

    [Header("CombatAtributes")]
    public float meleeRange;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetState(new CrabIdleState(this));
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
    #endregion
}
