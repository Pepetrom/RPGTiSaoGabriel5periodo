using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossStateMachine : MonoBehaviour
{
    IBossFSM state;
    public Animator animator;
    public GameObject player;
    public Player p;
    public NavMeshAgent boss;
    public StateMachineReceiver receiver;
    public float sortedNumber;  // Vari�vel para armazenar o n�mero sorteado

    [Header("COMBOS")]
    public bool att1att3 = false;
    public bool att3att2 = false;
    public bool animationIsEnded = false;
    public bool attack1toIdle = false;

    [Header("COLLIDERS")]
    public Collider leftHand;
    public Collider rightHand;
    public bool attHit;
    public int damage;
    [Header("THROW")]
    public GameObject rockPrefab;
    public GameObject rh;
    public bool rock;
    public bool throwRock;
    public bool isThrowing;
    public Rock rockLogic;
    public GameObject rockSpawnPos;
    public float speed;
    public Rigidbody rb;

    private void Start()
    {
        rockSpawnPos = GameObject.Find("RockHandler");
        leftHand.enabled = false;
        rightHand.enabled = false;
        attHit = false;
        att1att3 = false;
        isThrowing = false;
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

    #region M�todos Auxiliares
    //Os m�todos bool s�o chamados quando da trigger em um evento nas respectivas anima��es correspondentes, basta colocar o evento com o mesmo nome da fun��o
    //Al�m disso, � necess�rio que no script que o objeto que tenha a anima��o tamb�m possua um script que referencie o script principal(StateMachineReceiver)
    public Vector3 TargetDir()
    {
        Vector3 dir = player.transform.position - transform.position;
        return dir;
    }
    public void RotateBoss()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void OnAnimationEvent()
    {
        animationIsEnded = true;
    }

    public void Attack1toIdle()
    {
        Debug.Log("N�o Combou");
        attack1toIdle = true;
    }

    public void SortNumber()
    {
        sortedNumber = Random.value;
    }
    public bool Att1Att3()
    {
        att1att3 = true;
        Debug.Log(att1att3);
        return att1att3;
    }
    public bool Att3Att2()
    {
        att3att2 = true;
        return att3att2;
    }
    public void AllowColliderLeftHand()
    {
        leftHand.enabled = true;
    }
    public bool AllowColliderRightHand()
    {
        attHit = true;
        return attHit;
    }
    public void GrabRock()
    {
        Instantiate(rockPrefab, rh.transform.position, rh.transform.rotation);
        rock = false;
    }
    public bool GrabRockEvent()
    {
        rock = true;
        return rock;
    }
    public bool ThrowRockEvent()
    {
        Debug.Log("TacaTaca");
        throwRock = true;
        return throwRock;
    }
    #endregion
}
