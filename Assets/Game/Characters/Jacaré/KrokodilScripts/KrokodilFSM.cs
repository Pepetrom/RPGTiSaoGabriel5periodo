using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KrokodilFSM : MonoBehaviour, IDamageable, IChefe
{
    IKrokodil state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    [HideInInspector] public bool antecipation = false, end = false, combo = false, jump = false, fall = false, activate = false, hashitted = false, eventS = false, bigWall = false;
    [Header("COMBAT")]
    public Collider ownCollider;
    void Start()
    {
        if(player == null)
            player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetState(new KroStartState(this));
    }
    void Update()
    {
        this.state?.OnUpdate();
    }
    public void SetState(IKrokodil state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }
    #region MÉTODOS DE LÓGICA
    #region ACTION EVENTS
    public void Action()
    {
        throw new System.NotImplementedException();
    }

    public void Action2()
    {
        throw new System.NotImplementedException();
    }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    public void Antecipation()
    {
        throw new System.NotImplementedException();
    }

    public void Combo()
    {
        throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public void DeactivateSpecificEvent()
    {
        throw new System.NotImplementedException();
    }

    public void End()
    {
        end = true;
    }

    public void SpecificEvent()
    {
        throw new System.NotImplementedException();
    }

    public void StopAction()
    {
        throw new System.NotImplementedException();
    }

    public void StopAction2()
    {
        throw new System.NotImplementedException();
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
    #endregion
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        throw new System.NotImplementedException();
    }
}
