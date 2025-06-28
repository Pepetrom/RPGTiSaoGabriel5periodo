using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class KrokodilFSM : MonoBehaviour, IDamageable, IChefe
{
    IKrokodil state;
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject hoffMesh;
    public LayerMask groundLayer;
    [HideInInspector]public bool antecipation = false, end = false, combo = false, action = false, action2 = false, action3 = false, activate = false, hashitted = false, eventS = false, bigWall = false;
    [Header("COMBAT")]
    public string bossName;
    public Collider clawCollider, gunCollider, footCollider, twoHandedCollider, jumpCollider, mouthCollider;
    public CapsuleCollider ownCollider;
    public int randomValue,hp, basicAtt = 40, swingRate = 100,moveAtt = 40, damage, posture, maxPosture, jumpRate = 60, damage2, jumpAttRate, dashRate, bombCount = 0;
    public float meleeRange, maxRange, swingRange,jumpForce, fireRate, interval, arenaRadius, bombFireRate;
    public bool isSecondStage = false, canDoSecondStage = false, canRecoverPosture = true, canDash = true, grabbed = false;
    public Transform gunFireSpot, hitPos;
    public Vector3 arenaCenter;
    public GameObject bulletPrefab, armor, bulletPrefabSecondStage, jumpLocation, croc, bombPrefab;
    public Material secondStageMaterial;
    [Header("VFX")]
    public GameObject stun;
    public ParticleSystem bigImpactVFX, clawVFX, crackVFX, gunVFX;
    public VisualEffect hitVFX;
    public Coroutine runningCo;
    public VisualEffect[] vfxAtt;
    public Transform[] vfxAttPos;
    [Header("Arena")]
    public GameObject respawnOb;

    //swing
    Vector3 velocity, lVelocity;
    float moveY, moveX, time, nextBomb;
    void Start()
    {
        if(player == null)
            player = GameObject.FindWithTag("Player");
        if (jumpLocation == null)
            jumpLocation = GameObject.Find("JumpKroLocation");
        if(respawnOb == null)
            respawnOb = GameObject.Find("TeleporterJare");

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetState(new KroStartState(this));
        UIItems.instance.ShowBOSSHUD(true);
        UIItems.instance.ResetBossHP(hp, bossName);
        clawCollider.enabled = false; gunCollider.enabled = false; footCollider.enabled = false; twoHandedCollider.enabled = false; mouthCollider.enabled = false;
        posture = maxPosture;
        canDash = true;
    }
    void Update()
    {
        this.state?.OnUpdate();
        Posture();
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
        action2 = true;
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
        eventS = false;
    }

    public void End()
    {
        end = true;
        Debug.Log("Ei, sou true sim");
    }

    public void SpecificEvent()
    {
        eventS = true;
    }

    public void StopAction()
    {
        action = false;
    }

    public void StopAction2()
    {
        action2 = false;
    }
    public void Action3()
    {
        action3 = true;
    }
    public void StopAction3()
    {
        action3 = false;
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
    public Vector3 Swing()
    {
        Vector3 randomDirection = Random.insideUnitSphere * swingRange;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, swingRange, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
    public bool HasReachedDestination()
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
    public void SwingMove()
    {
        velocity = agent.velocity;
        lVelocity = transform.InverseTransformDirection(velocity);
        moveX = lVelocity.x;
        moveY = lVelocity.y;
        animator.SetFloat("MoveX", lVelocity.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", lVelocity.z, 0.1f, Time.deltaTime);
    }
    public void Impulse(float jumpForce)
    {
        transform.position += transform.up * jumpForce * Time.deltaTime;
    }
    public void Shoot()
    {
        time += Time.deltaTime;
        if(time >= fireRate)
        {
            FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.machineGun,transform.position); 
            if (isSecondStage)
                Instantiate(bulletPrefabSecondStage, gunFireSpot.position, gunFireSpot.rotation);
            else
                Instantiate(bulletPrefab, gunFireSpot.position, gunFireSpot.rotation);
            time = 0;
        }
    }
    private void Posture()
    {
        if (posture < maxPosture)
        {
            time += Time.deltaTime;
            if (time >= interval && canRecoverPosture)
            {
                posture += 5;
                time = 0;
            }
        }
        else posture = maxPosture;
        if (posture <= 0)
        {
            animator.Play("Stun");
            SetState(new KroStun(this));
        }
    }
    public void DropBombs()
    {
        for(int i = 0; i <= 12; i++)
        {
            Vector3 randomPos = GetRandomPointInArena();
            Vector3 spawnPos = randomPos + Vector3.up * Random.Range(80f, 120f);
            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 20f, groundLayer))
            {
                Instantiate(bombPrefab, hit.point + Vector3.up * 0.5f, Quaternion.identity);
            }
            else
            {
                Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
    Vector3 GetRandomPointInArena()
    {
        Vector2 randomCircle = Random.insideUnitCircle * arenaRadius;
        return new Vector3(arenaCenter.x + randomCircle.x, arenaCenter.y, arenaCenter.z + randomCircle.y);
    }
    public void FallTowardsSomething(float speed, Transform tg)
    {
        Vector3 target = new Vector3(tg.position.x, transform.position.y, tg.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    #endregion
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        UIItems.instance.bossCurrentHP -= damage;
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        posture -= damage;
        if(UIItems.instance.bossCurrentHP < hp / 2 && !isSecondStage)
        {
            canDoSecondStage = true;
        }
        if(UIItems.instance.bossCurrentHP <= 0)
        {
            animator.Play("ArmatureCroc_Death");
            SetState(new KroDeath(this));
        }
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.takingDamage, transform.position);
        PlayHitEffect();
    }
    public void CameraShakeKro()
    {
        CameraScript.instance.StartShake();
    }
    public void CombatCamera(float fovTarget, float value, float zoomSpeed)
    {
        CameraScript.instance.CombatCamera(fovTarget,value,zoomSpeed);
    }
    public void ChangeSkin()
    {
        Material[] mats = armor.GetComponent<SkinnedMeshRenderer>().materials;
        mats[0] = secondStageMaterial;
        armor.GetComponent<SkinnedMeshRenderer>().materials = mats;
    }
    public void PlayVFX(int i)
    {
        VisualEffect vfxInstance = Instantiate(vfxAtt[i], vfxAttPos[i].position, Quaternion.identity);
        vfxInstance.Play();
        vfxInstance.transform.SetParent(null);
    }
    public void PlaySoundAttached(string path)
    {
        FMODAudioManager.instance.PlaySoundAttached(path);
    }
    public void PlayHitEffect()
    {
        Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;
        Vector3 vfxDir = directionToPlayer.normalized;
        Quaternion vfxRotation = Quaternion.LookRotation(vfxDir);
        VisualEffect hitVFXinstance = Instantiate(hitVFX, hitPos.position, Quaternion.identity);
        hitVFXinstance.transform.rotation = vfxRotation;
        hitVFXinstance.Play();
        hitVFXinstance.transform.SetParent(null);
    }
    void SecondStageAnimTimes()
    {
        
    }
    public void DestroyBoss(GameObject boss)
    {
        Destroy(boss);
    }
    public IEnumerator RunningTime()
    {
        agent.speed = 20;
        agent.acceleration = 20;
        agent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(2.2f);
        agent.ResetPath();
        action = false;
        action2 = true;
        runningCo = null;
    }
}
