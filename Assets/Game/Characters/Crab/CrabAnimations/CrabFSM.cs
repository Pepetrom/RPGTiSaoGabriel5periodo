using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrabFSM : MonoBehaviour, IDamageable
{
    #region VARIABLES
    ICrabInterface state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public bool secondStage;
    [HideInInspector]public Rigidbody rb;

    //logic
    public int randomValue;
    [HideInInspector]public bool antecipation = false, end = false, combo = false, jump = false, fall = false, activate = false, hashitted = false, eventS = false;
    public GameObject secondStageLocation;

    [Header("CombatAtributes")]
    public float meleeRange, minRange, maxRange, kbForce;
    public int hp, damage, posture, maxPosture;
    public float impulse, rotateSpeed;
    public SphereCollider jumpCollider, claw1, claw2, furnaceCollider, ownCollider;
    public GameObject fire, fireCircle, bigFire;
    public bool canDoFireDamage, spinCombo = false;
    public string bossName;

    [Header("Fuzzy")]
    public int minJump, maxJump, fuzzyJump, minDash, maxDash, fuzzyDash;
    public int jumpCount = 0, spinCount = 0, comboValue;

    [Header("Effects")]
    public ParticleSystem VFXJumpImpact, VFXBigConcrete, VFXSmallConcreteFL, VFXSmallConcreteFR, VFXSmallConcreteBL, VFXSmallConcreteBR;
    public GameObject arenaFires; 
    public TrailRenderer[] trails;

    private float interval = 2, time;
    #endregion
    void Start()
    {
        UIItems.instance.ShowBOSSHUD(true);
        UIItems.instance.ResetBossHP(hp, bossName);
        posture = maxPosture;
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if(secondStageLocation == null)
        {
            secondStageLocation = GameObject.FindWithTag("CrabLocation");
        }
        rb = GetComponent<Rigidbody>();
        SetState(new CrabStartState(this));
        FuzzyGate(out fuzzyJump);
        FuzzyGate(out fuzzyDash);
        //Instantiate(arenaFires);
    }
    private void FixedUpdate()
    {
        this.state?.OnUpdate();
        Posture();
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
    public void FuzzyGate(out int a)
    {
        a = Random.Range(1, 101);
    }
    public float FuzzyLogic(int fuzzy, int min, int max)
    {
        float v = fuzzy - min;
        float size = max - min;
        float fuzzification = fuzzy / size;
        return fuzzification;
    }
    public void ActivateTrails(bool state, bool isJumping)
    {
        for(int i = 0;i < trails.Length; i++)
        {
            trails[i].gameObject.SetActive(state);
            if (isJumping)
            {
                trails[i].time = 1.4f;
            }
            else
            {
                trails[i].time = 0.3f;
            }
        }
    }
    #region ACTIONEVENTS
    public void Antecipation()
    {
        antecipation = true;
    }
    public void Activate()
    {
        activate = true;
    }
    public void Deactivate()
    {
        activate = false;
    }
    public void End()
    {
        end = true;
    }
    public void Combo()
    {
        combo = true;
    }
    public void Jump()
    {
        jump = true;
    }
    public void StopJump()
    {
        jump = false;
    }
    public void Fall()
    {
        fall = true;
    }
    public void StopFall()
    {
        fall = false;
    }
    public void SpecificEvent()
    {
        eventS = true;
    }
    public void DeactivateSpecificEvent()
    {
        eventS = false;
    }
    #endregion

    #endregion
    #region MÉTODOS AUXILIARES FÍSICA
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
    public void Impulse(float kbforce)
    {
        transform.position += transform.up * kbforce * Time.deltaTime;
    }
    public void FallTowardsSomething(float speed, Transform tg)
    {
        Vector3 target = new Vector3(tg.position.x, transform.position.y, tg.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    public void KB(float value)
    {
        rb.AddForce(transform.forward.normalized * value, ForceMode.Impulse);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    private void Posture()
    {
        if (posture < maxPosture)
        {
            time += Time.deltaTime;
            if(time >= interval)
            {
                posture += 5;
                time = 0;
            }
        }
        else posture = maxPosture;
        if (posture <= 0)
        {
            animator.Play("Crab_Stun");
            SetState(new CrabStun(this));
        }
    }
    #endregion
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        UIItems.instance.bossCurrentHP -= damage;
        posture -= damage;
        //playerHit = true;
        //hit.Play();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if(UIItems.instance.bossCurrentHP <= hp / 2 && !secondStage)
        {
            secondStage = true;
            posture = maxPosture + (maxPosture/4);
            animator.Play("Crab_SecondStage");
            SetState(new CrabSecondStage(this));
        }
        if (UIItems.instance.bossCurrentHP <= 0)
        {
            //animator.SetBool("death", true);
            //animator.SetBool("stun", false);
            Die();
        }
    }
}
