using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement
    [Header("Movement")]
    public float runSpeed;
    private float xAxis;
    [HideInInspector] public bool facingRight = true;
    private Rigidbody2D rb;
    private bool isPraying;
    [HideInInspector] public bool canMove = true;

    [Header("Roll")]
    public float rollSpeed;
    private bool isRolling = false;
    public CircleCollider2D rollColl;
    public float iFrame = 0.3f;

    //Jumping
    [Header("Jumping")]
    public float jumpForce;
    private bool jumpPressed = false;
    [HideInInspector] public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    //Animations
    private Animator animator;
    private string currentState;
    const string idle = "PlayerIdle";
    const string run = "PlayerRun";
    const string jump = "PlayerJump";
    const string fall = "PlayerFall";
    const string roll = "PlayerRoll";
    const string pray = "PlayerPray";
    const string parry = "PlayerParry";

    //Combat
    [Header("Combat")]
    private float attackTime = 0.0f;
    private int attackCount = 0;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    private bool isAttacking;
    private PlayerManager playerManager;
    [HideInInspector] public bool inCheckpointRange;
    [HideInInspector] public bool dead = false;
    [HideInInspector] public bool isCombo = false;
    [HideInInspector] public bool isGuarding = false;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject daggerobj;
    [SerializeField] private int daggerAmount;
    [SerializeField] private float cooldownTime;
    private CooldownController daggerCooldownController;
    private ItemStack daggerStack;

    private void Awake()
    {
        daggerStack = GetComponent<ItemStack>();
        daggerStack.SetItem(daggerobj, daggerAmount);

        daggerCooldownController = GetComponent<CooldownController>();
        daggerCooldownController.SetCooldown(cooldownTime);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
    }
    void Update()
    {
        CheckState();
        CheckInputs();
        CheckAttack();
        ChangeAnimations();
        FlipPlayer();

        if (daggerCooldownController.GetQueue().Count > 0)
        {
            if (Time.time >= daggerCooldownController.GetDequeueTime())
            {
                //DequeueLastItem() fonksiyonuyla Queue'dan çýkardýðýn dagger'ý Stack'e koy
                daggerStack.PushToStack(daggerCooldownController.DequeueLastItem());
            }
        }
    }
    void FixedUpdate()
    {
        Move();
        Jump();
    }
    void CheckState()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void CheckInputs()
    {
        //Get Horizontal Input
        xAxis = Input.GetAxisRaw("Horizontal");

        //Check Jump, Attack, Roll, Pray, Parry Input 
        if (Input.GetButtonDown("Jump"))
            if (isGrounded && !isRolling && !isPraying && !isAttacking)
                jumpPressed = true;
        if (Input.GetKeyDown(KeyCode.C)) // Pray
            if (inCheckpointRange && isGrounded && !isPraying && !isGuarding)
                isPraying = true;
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Roll
            if (!isRolling && isGrounded && !isPraying)
                StartCoroutine(Roll());            
        if (Input.GetMouseButton(1)) //Guard
            if (isGrounded && !playerManager.hitAnimRunning)
                performGuard();
        if (Input.GetMouseButtonUp(1))
            if (isGuarding)
                isGuarding = false;
        if (Input.GetMouseButtonDown(2))
            if (!isBusy())
                ThrowDagger();
        
    }
    
  
    void Move()
    {
        if (!canMove)
            return;

        if (!isRolling && !isAttacking && !isPraying)
        {
            if (!isGuarding)
            {
                rb.velocity = new Vector2(xAxis * runSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(xAxis * runSpeed / 2, rb.velocity.y);
            }
        }
    }
    void Jump()
    {
        if (jumpPressed) //isGuarding eklenebilir
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
            isGuarding = false;
        }
    }
    IEnumerator Roll()
    {
        isRolling = true;
        rollColl.enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
        
        if (facingRight)
            rb.velocity = new Vector2(rollSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-rollSpeed, rb.velocity.y);

        yield return new WaitForSeconds(.5f);
        isRolling = false;
        rollColl.enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;


    }
    void performGuard()
    {
        if(!isRolling && isGrounded && !isPraying && !isAttacking)
            ChangeAnimationState(parry);
            isGuarding = true;
    }
    
    void FlipPlayer()
    {
        if(!isRolling && !isPraying)
        {
            if(xAxis < 0 && facingRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = !facingRight;
            }
            else if(xAxis > 0 && !facingRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = !facingRight;
            }
        }
    }
    void ChangeAnimations()
    {
        //Ground Animations --> Idle, Run, Attack and Roll
        if(isGrounded && !playerManager.hitAnimRunning && !playerManager.isReviving)
        {
            if(!isRolling)
            {
                if(!isGuarding)
                {
                    if(!isAttacking && !isPraying)
                    { 
                        if(xAxis == 0)
                            ChangeAnimationState(idle);
                        else
                            ChangeAnimationState(run); 
                    }
                    if(isAttacking)
                    {                 
                        ChangeAnimationState("PlayerAttack" + attackCount);
                        if(attackTime > 0.6f)    
                            isAttacking = false;
                    }
                    if(isPraying)
                    {
                        ChangeAnimationState(pray);
                        StartCoroutine(StopPraying());
                    }
                }
            }
            else
                ChangeAnimationState(roll);
        }

        //Air Animations --> Jump and Fall
        if(!isGrounded)
        {
            if(rb.velocity.y > 0)
                ChangeAnimationState(jump);
            if(rb.velocity.y < 0)
                ChangeAnimationState(fall);    
        }
   
     }
    public void ChangeAnimationState(string newState)
    {
        if(currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    IEnumerator StopPraying()
    {
        yield return new WaitForSeconds(1.7f);
        isPraying = false;
    }
    private void CheckAttack()
    {
        attackTime += Time.deltaTime;
        if (attackTime > 0.6f)
            isCombo = false;
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isPraying && !isGuarding && isGrounded && !isRolling)
                if (isCombo && attackTime > 0.3f)
                    Attack();
                else if (!isCombo)
                    Attack();
        }
    }
    void Attack()
    {
        isAttacking = true;
        attackDamage += 2;
        attackCount++;
        isCombo = true;

        if (attackCount > 3 || attackTime > 0.6f)
        {
            attackCount = 1;
            attackDamage = 10;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.CompareTag("Enemy"))
                enemy.GetComponent<Minion_wfireball>().TakeDamage(attackDamage);
            if(enemy.CompareTag("Villager"))
                enemy.GetComponent<VillagerHealthManager>().TakeDamage(attackDamage);    
        }
        attackTime = 0f;
    }
    public void ThrowDagger()
    {
        //Stack'ten gir dagger çýkar ve dagger objesine ata
        GameObject dagger = daggerStack.PopFromStack();

        if (dagger != null)
        {
            if (GetComponent<PlayerController>().facingRight)
            {
                dagger.transform.position = firePoint.position;
                dagger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                dagger.GetComponent<Dagger>().Initialize(Vector2.right);
                dagger.SetActive(true);
                StartCoroutine(startDaggerLifeTime());
                //Stack'ten çýkarmýþ olduðun dagger objesini Queue'ya yerleþtir
                daggerCooldownController.EnqueueItem(dagger);
            }
            else
            {
                dagger.transform.position = firePoint.position;
                dagger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                dagger.GetComponent<Dagger>().Initialize(Vector2.left);
                dagger.SetActive(true);
                StartCoroutine(startDaggerLifeTime());
                //Stack'ten çýkarmýþ olduðun dagger objesini Queue'ya yerleþtir
                daggerCooldownController.EnqueueItem(dagger);
            }
            //Daggerlarýn 3 saniye sonra sahneden çýkmasýna yarayan coroutine
            IEnumerator startDaggerLifeTime()
            {
                yield return new WaitForSeconds(10f);
                dagger.SetActive(false);
            }
        }
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public bool isBusy()
    {
        if (isAttacking || isGuarding || isPraying || isRolling || playerManager.isReviving || playerManager.hitAnimRunning)
            return true;
        else
            return false;
    }    
}
