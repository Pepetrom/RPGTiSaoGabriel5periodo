using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public enum Atribute { strength, agility, constitution}
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Camera mainCamera;
    //Rigidbody rb;
    public Vector3 moveDirection, forwardDirection;
    public Animator animator;
    public GameObject model;
    public bool masterCanDo { get; private set; }
    public bool canGiveInput = true;
    [Header("Move Settings------------------")]
    public float moveSpeed;
    public float runningMultiplier = 1;
    public bool canMove;
    [Header("Dash Settings------------------")]
    public float baseDashForce;
    public float baseDashCooldown;
    public ParticleSystem dustParticle, bloodParticle, critical;
    [Header("Atack Settings------------------")]
    public int baseDamage;
    public int stamPerHit; // Vari�vel que indica a quantidade de estamina perdida por hit
    public bool isAttacking = false; // Vari�vel para saber se o jogador est� atacando ou n�o
    public int comboCounter = 1;
    public Transform target = null;
    public AtackCollider atackCollider;
    [SerializeField] float detectionAutoTargetRange = 30;
    public TrailRenderer swordTrail;
    [Header("Defese Settings------------------")]
    public float invencibilityTime;
    public bool canTakeDamage = true;
    public Transform damageFont;
    [Header("Actions------------------")]
    public bool[] canDoAction = new bool[4];
    public IAction[] actions = new IAction[4];
    [Header("Atacks------------------")]
    public bool holdingToAtack = false;
    public float timerHoldingAtack = 0;
    public bool canDoAtack = true;
    public IWeapon[] atacks = new IWeapon[1];
    [Header("Runes------------------")]
    [SerializeField] GameObject[] runesForIRune;
    public IRune[] runes;
    public int equipedPrimaryRune = 0, equipedSecondaryRune = 0, equipedTerciaryRune = 0;
    public float damageAdd, damageMultiplier, criticalMultiplier;
    [Header("GroundCheck------------------")]
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask groundMask;
    public bool grounded;
    [Header("Gravity Settings---------------")]
    [SerializeField] float gravityForce;
    [Header("Rotation------------------")]
    Quaternion newRotation;
    Vector3 mousePosition, targetDirection;
    Vector3 cameraAlignValue;
    [Header("Atributes------------------")]
    public int constitution = 0, agility = 0, strength = 0;

    //ABSURDLY COMPLICATED RENAN SHENANIGANS
    Vector3 playerRight;
    Vector3 playerForward;
    float targetLockedX, targetLockedY;
    float rightAmount;
    float forwardAmount;
    Enemy enemy;

    public CharacterController cc;
    float gravity = -9;
    float gravityToTakeDamage = -20;
    float baseGravity = -9;

    //Audio
    public AudioManager audioMan;

    //Detect Closest Enemy
    Collider[] hits;
    float closestDistance = Mathf.Infinity;
    Transform closestEnemy = null;
    float enemyDistance = 0;
    //Raycast
    Ray ray;
    RaycastHit hit;
    //SoulDrop
    public GameObject soulPickable;
    //Bools of cheat
    public bool imortal;
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
    }
    void Start()
    {
        GameManager.instance.UnPause();
        masterCanDo = true;
        cameraAlignValue = mainCamera.transform.forward;
        cameraAlignValue.y = 0;
        cameraAlignValue = cameraAlignValue.normalized;
        swordTrail.emitting = false;
        InitialActions();
    }
    void InitialActions()
    {
        actions[0] = new A_DashTowardMovement();
        actions[0].SetSlot(0);
        actions[1] = new A_AtackDash();
        actions[1].SetSlot(1);
        actions[2] = new A_KnockBack();
        actions[2].SetSlot(2);

        atacks[0] = new W_BigSwordAtack();
        atacks[0].SetSlot(0);


        runes = new IRune[runesForIRune.Length];
        for (int i = 0; i < runesForIRune.Length; i++)
        {
            runes[i] = runesForIRune[i].GetComponent<IRune>();
        }
    }
    void Update()
    {
        if (!masterCanDo) return;
        Controls();
        moveDirection.y = gravity;
        
    }
    void FixedUpdate()
    {
        DoActions();
        if (!masterCanDo) return;
        animator.speed = GameManager.instance.actionTime;
        CheckGround();
        SetDirection();
        cc.Move(moveDirection * Time.fixedDeltaTime);
        PlayerInteract.instance.FixedUpdatePlayerInteract();
    }
    void CheckDistanceTarget()
    {
        targetDirection = (target.position - transform.position).normalized;
        playerRight = Vector3.Cross(targetDirection, Vector3.up);
        playerForward = Vector3.Cross(playerRight, Vector3.up);

        rightAmount = Vector3.Dot(moveDirection.normalized, playerRight);
        forwardAmount = Vector3.Dot(moveDirection.normalized, playerForward);

        if (forwardAmount < 0)
        {
            targetLockedY += Time.fixedDeltaTime;
        }
        else if (forwardAmount > 0)
        {
            targetLockedY -= Time.fixedDeltaTime;
        }
        if (rightAmount < 0)
        {
            targetLockedX += Time.fixedDeltaTime;
        }
        else if (rightAmount > 0)
        {
            targetLockedX -= Time.fixedDeltaTime;
        }

        targetLockedX = Mathf.Clamp(targetLockedX, -1, 1);
        targetLockedY = Mathf.Clamp(targetLockedY, -1, 1);

        animator.SetFloat("X", targetLockedX);
        animator.SetFloat("Y", targetLockedY);
    }
    void DoActions()
    {
        actions[0].ActionUpdate(); // Dash
        actions[1].ActionUpdate(); // AtackDash
        actions[2].ActionUpdate(); // KnockBack
        atacks[0].AtackUpdate();
    }
    void Controls()
    {
        if (!canGiveInput) return;
        WalkInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            actions[0].ActionStart();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //light Atack
            Atack(0, false);
            timerHoldingAtack += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            //heavy Atack
            Atack(0, true);
            timerHoldingAtack += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1)){
            timerHoldingAtack = 0;
        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            DetectClosestEnemy();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseEstus();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerInteract.instance.UpdatePlayerInteract();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneController.sceneController.OpenCheatMenu(!SceneController.sceneController.cheatMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.sceneController.OpenCommands(!SceneController.sceneController.commands.activeSelf);
        }
    }
    void UseEstus()
    {
        if (Estus.instance.flaskQuantity <= 0) return;
        masterCanDo = false;
        canMove = false;
        comboCounter = 1;
        isAttacking = false;
        swordTrail.emitting = false;
        animator.SetBool("Atacking", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetTrigger("Estus");
        Estus.instance.UseEstus();
    }
    void Atack(int slot, bool heavy)
    {
        holdingToAtack = (timerHoldingAtack > 1 || timerHoldingAtack == 0) ? true : false;
        atacks[slot].AtackStart(heavy);
    }
    void CheckGround()
    {
        if (cc.isGrounded || !canMove)
        {
            grounded = true;
            if (gravity < gravityToTakeDamage)
            {
                HPBar.instance.FallDamage(-gravity * 10);
            }
            if (grounded) gravity = baseGravity;
            canDoAction[3] = true;
        }
        else
        {
            gravity += baseGravity * Time.fixedDeltaTime;
            grounded = false;
        }
    }
    void WalkInput()
    {
        moveDirection = Vector3.zero;
        if( !canMove ) return;
        moveDirection.y = 0;
        moveDirection.x = Input.GetAxis("Horizontal") * cameraAlignValue.x + (Input.GetAxis("Vertical") * cameraAlignValue.z);
        moveDirection.z = Input.GetAxis("Vertical") * cameraAlignValue.x - (Input.GetAxis("Horizontal") * cameraAlignValue.z);
        moveDirection.Normalize();
        Run();
        moveDirection = moveDirection * moveSpeed * runningMultiplier;
        animator.SetBool("Walk", (moveDirection.x != 0 || moveDirection.z != 0));
    }
    void SetDirection()
    {
        if (canMove)
        {
            LookForward();
            LookAtTarget();
        }
    }
    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero && !isAttacking)
        {
            runningMultiplier = 2;
            animator.SetBool("Run", true);
            target = null;
            if(closestEnemy != null)
                closestEnemy.GetComponent<Enemy>().ShowSprite(false);
        }
        else
        {
            runningMultiplier = 1;
            animator.SetBool("Run", false);
        }
    }
    void DetectClosestEnemy()
    {
        if (target)
        {
            target = null;
            closestEnemy.GetComponent<Enemy>().ShowSprite(false);
            return;
        }
        hits = Physics.OverlapSphere(model.transform.position, detectionAutoTargetRange);
        closestDistance = Mathf.Infinity;
        closestEnemy = null;
        enemyDistance = 0;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                enemyDistance = Vector3.Distance(transform.position, hit.transform.position);
                if (enemyDistance < closestDistance)
                {
                    closestDistance = enemyDistance;
                    closestEnemy = hit.transform;
                    closestEnemy.GetComponent<Enemy>().ShowSprite(true);
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
        CheckDistanceTarget();
    }
    void LookForward()
    {
        if (target != null) return;
        animator.SetFloat("X", 0);
        animator.SetFloat("Y", 1);
        forwardDirection = moveDirection;
        forwardDirection.y = 0;
        if (forwardDirection == Vector3.zero) return;
        newRotation = Quaternion.LookRotation(forwardDirection);
        newRotation = Quaternion.Slerp(model.transform.rotation, newRotation, 0.2f);
        model.transform.rotation = newRotation;
    }
    public Vector3 GetMousePosition()
    {
        if (target != null) return Vector3.zero;
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask)) // agora est� funcionando os raycasts
        {
            mousePosition = hit.point;
        }
        return mousePosition;
    }
    public void InstantiateEffect(GameObject effect)
    {
        Instantiate(effect, model.transform.position, model.transform.rotation);
    }
    public void AddAtribute(Atribute which)
    {
        switch (which)
        {
            case Atribute.constitution:
                constitution += 1;
                HPBar.instance.UpdateMaxHp(constitution);
                break;
            case Atribute.strength:
                strength += 1;
                break;
            case Atribute.agility:
                agility += 1;
                StaminaBar.instance.UpdateMaxStamina(agility);
                break;
        }
    }
    public void Die()
    {
        Debug.Log("MOrreu");
        animator.SetTrigger("Death");
        Pickable temp = Instantiate(soulPickable, transform.position, transform.rotation).GetComponent<Pickable>();
        temp.value =(int)(GameManager.instance.skillPoints / 2);
        GameManager.instance.Score(-temp.value);
        StopAllActionsDeath();
        GameManager.instance.Respawn();
    }
    public void ForceIddle()
    {
        animator.SetTrigger("ForceIddle");
    }
    public void StopAllActions()
    {
        masterCanDo = false;
        canMove = false;
        canGiveInput = false;
        comboCounter = 1;
        isAttacking = false;
        swordTrail.emitting = false;
        atackCollider.gameObject.SetActive(false);

        animator.SetBool("Atacking", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetTrigger("ForceIddle");
    }
    public void StopAllActionsDeath()
    {
        masterCanDo = false;
        canMove = false;
        canGiveInput = false;
        comboCounter = 1;
        isAttacking = false;
        swordTrail.emitting = false;
        atackCollider.gameObject.SetActive(false);

        animator.SetBool("Atacking", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }
    public void ResetLesserActions(){
        canDoAtack = true;
        canMove = true;
    }
    public void ResetAllActions()
    {
        moveDirection = Vector3.zero;
        masterCanDo = true;
        canDoAtack = true;
        canMove = true;
        canGiveInput = true;

        animator.SetBool("Atacking", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);

        isAttacking = false;
        swordTrail.emitting = false;
        atackCollider.gameObject.SetActive(false);
    }
}

