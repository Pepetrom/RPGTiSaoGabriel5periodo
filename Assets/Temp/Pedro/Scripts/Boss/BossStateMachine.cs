using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossStateMachine : MonoBehaviour
{
    IBossFSM state;
    public Animator animator;
    public GameObject player;
    public NavMeshAgent boss;
    public bool animationIsEnded = false;
    public bool attack1toIdle = false;
    public StateMachineReceiver receiver;
    public float sortedNumber;  // Variável para armazenar o número sorteado

    private void Start()
    {
        if (receiver != null)
        {
            receiver.boss = this;
        }
        SetState(new StateBossFightBegins(this));
    }

    private void FixedUpdate()
    {
        state?.OnUpdate();
    }

    public void SetState(IBossFSM state)
    {
        this.state?.OnExit();
        this.state = state;
        this.state?.OnEnter();
    }

    #region Métodos Auxiliares
    public Vector3 TargetDir()
    {
        Vector3 dir = player.transform.position - transform.position;
        return dir;
    }

    public void OnAnimationEvent()
    {
        animationIsEnded = true;
    }

    public void Attack1toIdle()
    {
        Debug.Log("Não Combou");
        attack1toIdle = true;
    }

    public void SortNumber()
    {
        sortedNumber = Random.value;
    }
    #endregion
}
