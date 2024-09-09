using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Camera mainCamera;
    Rigidbody rb;
    public Vector3 moveDirection;
    public Animator animator;
    public GameObject model;
    public Camera fakeCamera;
    [Header("Move Settings------------------")]
    public float moveSpeed;
    public float runningMultiplier = 1;
    public bool canMove;
    [Header("Dash Settings------------------")]
    public float baseDashForce;
    public float baseDashCooldown;
    public ParticleSystem particle;
    [Header("Atack Settings------------------")]
    public float baseDamage;
    public float atackSpeed;
    float damage = 0;
    [Header("Defese Settings------------------")]
    public float maxlife, invencibilityTime;
    bool canTakeDamage = true;
    float life;
    [Header("Actions------------------")]
    public bool[] canDoAction = new bool[2];
    IAction[] actions = new IAction[2];
    [Header("Atacks------------------")]
    public bool canDoAtack = true;
    //Rotation
    Quaternion rotation;
    Vector3 mousePosition, worldMousePosition, direction;
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
    void Start()
    {
        //InitialActions();
        actions[0] = new A_SideStep();
        actions[0].SetSlot(0);
        life = maxlife;
    }
    void InitialActions()
    {
        actions[0] = new A_SideStep();
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].SetSlot(i);
            canDoAction[i] = true;
        }
    }
    void Update()
    {
        LookAtMouse();
        Controls();
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
        DoActions();
        rb.velocity = moveDirection;
    }
    void DoActions()
    {
        actions[0].DoAction();
    }
    void Controls()
    {
        if (canDoAction[0])
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                actions[0].ActionStart();
            }
        }
        if (canDoAtack)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Atack(0);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //Atack(1);
            }
        }
    }
    void Atack(int slot)
    {
        canDoAtack = false;
        canMove = false;
        animator.speed = atackSpeed;
        moveDirection = Vector3.zero;
        runningMultiplier = 1;
        animator.SetBool("Atacking", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }

    void Move()
    {
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.Normalize();
        moveDirection = moveDirection * moveSpeed * runningMultiplier;
        moveDirection.y = 0;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage && life > 0)
        {
            StartCoroutine(InvulnableTime());
            life -= damage;
        }
        if (life <= 0)
        {
            Die();
            StopCoroutine(InvulnableTime());
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void LookAtMouse()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = fakeCamera.transform.position.y; 
        worldMousePosition = fakeCamera.ScreenToWorldPoint(mousePosition);
        direction = worldMousePosition;
        direction.y = model.transform.position.y;

        model.transform.LookAt(direction);
    }

    public IEnumerator InvulnableTime()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(invencibilityTime);
        canTakeDamage = true;
    }
}

