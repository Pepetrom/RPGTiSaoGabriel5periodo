using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Camera mainCamera;
    Rigidbody rb;
    public Vector3 moveDirection;
    public Animator animator;
    public GameObject model;
    [Header("Move Settings------------------")]
    public float moveSpeed;
    public float runningMultiplier = 1;
    public bool canMove;
    [Header("Dash Settings------------------")]
    public float baseDashForce;
    public float baseDashCooldown;
    public ParticleSystem dustParticle, bloodParticle;
    [Header("Atack Settings------------------")]
    public int baseDamage;
    public int stamPerHit; // Variável que indica a quantidade de estamina perdida por hit
    public bool isAttacking = false; // Variável para saber se o jogador está atacando ou não
    public int comboCounter = 1;
    public Transform target = null;
    public AtackCollider atackCollider;
    public float detectionAutoTargetRange = 15;
    public TrailRenderer swordTrail;
    [Header("Defese Settings------------------")]
    public float maxlife, invencibilityTime;
    public bool canTakeDamage = true;
    public Transform damageFont;
    [Header("Actions------------------")]
    public bool[] canDoAction = new bool[2];
    public IAction[] actions = new IAction[3];
    [Header("Atacks------------------")]
    public bool[] canDoAtack = new bool[2];
    public IWeapon[] atacks = new IWeapon[2];
    [Header("Runes------------------")]
    public GameObject[] runes;
    public int actualRune = 1;
    //Rotation
    Quaternion newRotation;
    Vector3 mousePosition, worldMousePosition, targetDirection;
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
    void Start()
    {
        swordTrail.emitting = false;
        InitialActions();
    }
    void InitialActions()
    {
        actions[0] = new A_SideStep();
        actions[0].SetSlot(0);
        actions[1] = new A_AtackDash();
        actions[1].SetSlot(1);
        actions[2] = new A_KnockBack();
        actions[2].SetSlot(1);

        atacks[0] = new W_TestAtack();
        atacks[0].SetSlot(0);
    }
    void Update()
    {
        moveDirection.y = rb.velocity.y;
        Controls();
    }
    void FixedUpdate()
    {
        Move();
        DoActions();
        animator.speed = GameManager.instance.actionTime;
        rb.velocity = moveDirection;
    }
    void DoActions()
    {
        actions[0].ActionUpdate();
        actions[1].ActionUpdate();
        actions[2].ActionUpdate();
        atacks[0].AtackUpdate();
    }
    void Controls()
    {
        if (canDoAction[0])
        {
            //Meu teclado não deixa apertar W+ A+ Space
            if (Input.GetKeyDown(KeyCode.Space) && StaminaBar.stambarInstance.currentStam >= stamPerHit)
            {
                actions[0].ActionStart();
                if (atacks[0].CanBeInterupted())
                {
                    atacks[0].InteruptAtack();
                }
                StaminaBar.stambarInstance.DrainStamina(stamPerHit * 2); // Aqui estou tirando a estamina do player
            }
        }
        if (canDoAtack[0])
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && StaminaBar.stambarInstance.currentStam >= stamPerHit)
            {
                Atack(0);
                StaminaBar.stambarInstance.DrainStamina(stamPerHit); // Aqui estou tirando a estamina do player
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (target)
            {
                target = null;
            }
            else
            {
                DetectClosestEnemy();
            }
        }

    }
    void Atack(int slot)
    {
        atacks[slot].AtackStart();
    }
    void Move()
    {
        if (!canMove)
        {
            moveDirection = Vector3.zero;
            return;
        }
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.Normalize();
        Run();
        moveDirection = moveDirection * moveSpeed * runningMultiplier;
        moveDirection.y = 0;
        animator.SetBool("Walk", moveDirection != Vector3.zero);
        LookForward();
        LookAtTarget();
    }
    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero)
        {
            runningMultiplier = 2;
            animator.SetBool("Run", true);
            target = null;
        }
        else
        {
            runningMultiplier = 1;
            animator.SetBool("Run", false);
        }
    }

    void DetectClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(model.transform.position, detectionAutoTargetRange);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }
        target = closestEnemy;
    }
    void LookAtTarget()
    {
        if (!canMove || target == null) return;

        targetDirection = (target.position - model.transform.position).normalized;
        targetDirection.y = 0;
        newRotation = Quaternion.LookRotation(targetDirection);
        model.transform.rotation = newRotation;
    }
    void LookForward()
    {
        if (target != null || moveDirection == Vector3.zero) return;

        newRotation = Quaternion.LookRotation(moveDirection);
        newRotation = Quaternion.Slerp(model.transform.rotation, newRotation, 0.2f);
        model.transform.rotation = newRotation;
    }
    public Vector3 GetMousePosition()
    {
        if (target != null) return Vector3.zero;

        mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.transform.position.y;
        worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition = worldMousePosition;
        mousePosition.y = model.transform.position.y;
        target = null;
        
        return mousePosition;
    }
    public void RuneEffect()
    {
        switch (actualRune)
        {
            case 0:
                break;
            case 1:
                Instantiate(runes[1], model.transform.position, model.transform.rotation);
                break;
            case 2:
                Instantiate(runes[2], model.transform.position, model.transform.rotation);
                break;
            case 3:
                Instantiate(runes[3], model.transform.position, model.transform.rotation);
                break;
        }
    }
    
}

