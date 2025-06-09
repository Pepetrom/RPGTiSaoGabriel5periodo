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
    [HideInInspector] public bool antecipation = false, end = false, combo = false, action = false, activate = false, hashitted = false, eventS = false, bigWall = false;
    [Header("COMBAT")]
    public string bossName;
    public Collider ownCollider, clawCollider, gunCollider, footCollider, twoHandedCollider;
    public int randomValue, att2Count, hp, basicAtt = 40, damage;
    public float meleeRange, maxRange;
    public bool isSecondStage;
    void Start()
    {
        if(player == null)
            player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetState(new KroStartState(this));
        UIItems.instance.ShowBOSSHUD(true);
        UIItems.instance.ResetBossHP(hp, bossName);
        clawCollider.enabled = false; gunCollider.enabled = false; footCollider.enabled = false; twoHandedCollider.enabled = false;
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
        throw new System.NotImplementedException();
    }

    public void Activate()
    {
        Debug.Log("Evento");
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
        throw new System.NotImplementedException();
    }

    public void End()
    {
        end = true;
        Debug.Log("Ei, sou true sim");
    }

    public void SpecificEvent()
    {
        throw new System.NotImplementedException();
    }

    public void StopAction()
    {
        action = false;
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
        UIItems.instance.bossCurrentHP -= damage;
        /*//posture -= damage;
        //FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.takingDamage, transform.position);
        //playerHit = true;
        //hit.Play();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if (UIItems.instance.bossCurrentHP <= hp / 2 && !secondStage && UIItems.instance.bossCurrentHP >= 0)
        {
            posture = maxPosture + (maxPosture / 4);
            ownCollider.enabled = false;
            animator.SetBool("secondStage", true);
            SetState(new CrabSecondStage(this));
        }
        if (UIItems.instance.bossCurrentHP <= 0)
        {
            animator.Play("Crab_Death");
            SetState(new CrabDeath(this));
        }*/
    }
}
