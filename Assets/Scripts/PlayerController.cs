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
    [HideInInspector] public bool canFlip = true;

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

    [Header("Wall Slide")]
    public Transform wallGrabPointFront;
    public Transform wallGrabPointBack;
    public float wallSlideSpeed = 0.2f;
    private bool isWallSliding = false;
    private bool grabFront, grabBack, canGrab;
    public float wallDistance = 0.05f;

    [Header("Wall Jump")]
    public float wallJumpTime = 0.1f;
    public float xWallForce = 5f;
    public float wallJumpLerp = 1f;
    private float jumpTime;
    private int wallDirection;
    private bool wallJumpPressed = false;
    private bool isWallJumping = false;

    [Header("Ledge Climb")]
    public Transform ledgeCheckUp;
    public Transform ledgeCheckDown;
    private bool isTouchingLedgeUp;
    private bool isTouchingLedgeDown;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    [SerializeField]
    private float ledgeDistance;
    public float ledgeXOffset1 = 0.0f;
    public float ledgeYOffset1 = 0.0f;
    public float ledgeXOffset2 = 0.0f;
    public float ledgeYOffset2 = 0.0f;

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
    const string climb = "PlayerClimb";

    //Combat
    [Header("Combat")]
    public Transform attackPoint;
    private float attackTime = 0.0f;
    private int attackCount = 0;
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
        CheckLedgeClimb();
        if (daggerCooldownController.GetQueue().Count > 0)
        {
            if (Time.time >= daggerCooldownController.GetDequeueTime())
            {
                //DequeueLastItem() fonksiyonuyla Queue'dan cikardigin dagger'i Stack'e koy
                daggerStack.PushToStack(daggerCooldownController.DequeueLastItem());
            }
        }
    }
    void FixedUpdate()
    {
        Move();
        Jump();
        WallSlide();
    }
    void CheckState()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        grabFront = Physics2D.OverlapCircle(wallGrabPointFront.position, wallDistance, groundLayer);
        grabBack = Physics2D.OverlapCircle(wallGrabPointBack.position, wallDistance, groundLayer);
        if(facingRight)
        {
            isTouchingLedgeUp = Physics2D.Raycast(ledgeCheckUp.position, transform.right, ledgeDistance, groundLayer);
            isTouchingLedgeDown = Physics2D.Raycast(ledgeCheckDown.position, transform.right, ledgeDistance, groundLayer);
        }
        else
        {
            isTouchingLedgeUp = Physics2D.Raycast(ledgeCheckUp.position, -transform.right, ledgeDistance, groundLayer);
            isTouchingLedgeDown = Physics2D.Raycast(ledgeCheckDown.position, -transform.right, ledgeDistance, groundLayer);

        }

        if(grabFront || grabBack)
        {
            canGrab = true;
        }
        else
        {
            canGrab = false;
        }

        if(grabFront && !grabBack)
        {
            if(wallGrabPointFront.transform.position.x > wallGrabPointBack.transform.position.x)
            {
                wallDirection = -1;
            }
            else
            {
                wallDirection = 1;
            }
        }
        else if(!grabFront && grabBack)
        {
            if(wallGrabPointBack.transform.position.x > wallGrabPointFront.transform.position.x)
            {
                wallDirection = -1;
            }
            else
            {
                wallDirection = 1;
            }
        }

        if(isTouchingLedgeDown && !isTouchingLedgeUp && !ledgeDetected){
            ledgeDetected = true;
            ledgePosBot = ledgeCheckDown.position;
        }

    }
    void CheckInputs()
    {
        //Get Horizontal Input
        if(!isWallJumping)
        {
            xAxis = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            xAxis = 0;
        }
        //Check Jump, Attack, Roll, Pray, Parry Input 
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && !isRolling && !isPraying && !isAttacking && !isWallSliding)
                jumpPressed = true;
            if (isWallSliding)
                wallJumpPressed = true;
        }
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
    
    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            if(facingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallDistance) - ledgeXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallDistance) + ledgeXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallDistance) + ledgeXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallDistance) - ledgeXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeYOffset2);
            }
            canMove = false;
            canFlip = false;
        }

        if(canClimbLedge)
        {
            transform.position = ledgePos1;
            FinishLedgeClimb();
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
    }
  
    void Move()
    {
        if (canMove)
        {
            if (!isWallJumping)
            {
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
            else
            {
                rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(xAxis * runSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
            }
        }
        else
        {
            return;
        }
    }
    void Jump()
    {
        if (jumpPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
            isGuarding = false;
        }
        if (wallJumpPressed)
        {
            if((wallDirection == 1 && !facingRight) || (wallDirection == -1 && facingRight))
            {
                Flip();
            }

            rb.velocity = new Vector2(xWallForce * wallDirection, jumpForce);
            wallJumpPressed = false;

            StartCoroutine(WallJumpWaiter());
        }
    }

    IEnumerator WallJumpWaiter()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(0.4f);
        isWallJumping = false;
    }

    void WallSlide()
    {
        if (canGrab && !isGrounded && !canClimbLedge && xAxis != 0)
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
        }       
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
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
                Flip();
            }
            else if(xAxis > 0 && !facingRight)
            {
                Flip();
            }
        }
    }
    
    void Flip(){
        if(canFlip)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facingRight = !facingRight;
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
        //Stack'ten gir dagger ��kar ve dagger objesine ata
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
                //Stack'ten ��karm�� oldu�un dagger objesini Queue'ya yerle�tir
                daggerCooldownController.EnqueueItem(dagger);
            }
            else
            {
                dagger.transform.position = firePoint.position;
                dagger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                dagger.GetComponent<Dagger>().Initialize(Vector2.left);
                dagger.SetActive(true);
                StartCoroutine(startDaggerLifeTime());
                //Stack'ten ��karm�� oldu�un dagger objesini Queue'ya yerle�tir
                daggerCooldownController.EnqueueItem(dagger);
            }
            //Daggerlar�n 3 saniye sonra sahneden ��kmas�na yarayan coroutine
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
