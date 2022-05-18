using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement
    [Header("Movement")]
    public float runSpeed;
    public float walkSpeed;
    private float xAxis;
    private bool walkToggle;
    [HideInInspector] public bool facingRight = true;
    private Rigidbody2D rb;
    private bool isPraying;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canFlip = true;

    [Header("Roll")]
    public float rollSpeed;
    private bool isRolling = false;
    public GameObject rollColl;
    public float iFrame = 0.3f;

    //Jumping
    [Header("Jumping")]
    public float jumpForce;
    private bool isJumping = false;
    public float jumpTimer;
    private float jumpTimeCounter;
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
    private float gravity;

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
    const string fallattack = "PlayerFallAttack";
    const string climb = "PlayerClimb";

    //Combat
    [Header("Combat")]
    public Transform attackPoint;
    public Transform fallAttackBox;
    public Vector3 fallAttackSize;
    private float attackTime = 0.0f;
    private int attackCount = 0;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    private bool isAttacking;
    private bool isFallAttacking;
    private PlayerManager playerManager;
    private float stamina;
    [HideInInspector] public bool inCheckpointRange;
    [HideInInspector] public bool dead = false;
    [HideInInspector] public bool isCombo = false;
    [HideInInspector] public bool isGuarding = false;
    private float guardTimer;
    private bool parryStamina;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject daggerobj;
    [SerializeField] private int daggerAmount;
    [SerializeField] private float cooldownTime;
    private CooldownController daggerCooldownController;
    private ItemStack daggerStack;

    public GameObject moveList;

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
        gravity = rb.gravityScale;
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
        stamina = StaminaBar.instance.currentStamina;
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
        //Jump();
        WallJump();
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
            if(isTouchingLedgeDown)
            {
                ledgePosBot = ledgeCheckDown.position;
             }
        }
        else
        {
            isTouchingLedgeUp = Physics2D.Raycast(ledgeCheckUp.position, -transform.right, ledgeDistance, groundLayer);
            isTouchingLedgeDown = Physics2D.Raycast(ledgeCheckDown.position, -transform.right, ledgeDistance, groundLayer);
            if(isTouchingLedgeDown)
            { 
                ledgePosBot = ledgeCheckDown.position;
            }        
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
        }

    }
    void CheckInputs()
    {
        //Get Horizontal Input
        if(!isWallJumping && !canClimbLedge)
        {
            xAxis = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            xAxis = 0;
        }
        //Walk Toggle
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            walkToggle = !walkToggle;
        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && !isRolling && !isPraying && !isAttacking && !isFallAttacking && !isWallSliding && !playerManager.isHealing)
            {
                isJumping = true;
                jumpTimeCounter = jumpTimer;
                Jump();
            }
            if (isWallSliding)
                wallJumpPressed = true;
        }
        if (Input.GetButton("Jump") && isJumping)
            if (jumpTimeCounter > 0)
            {
                jumpTimeCounter -= Time.deltaTime;
                Jump();
            }
            else 
                isJumping = false;
                
        if(Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        //Pray
        if (Input.GetKeyDown(KeyCode.C))
            if (isGrounded && inCheckpointRange && !isPraying && !isAttacking && !isFallAttacking && !isGuarding && !isRolling && !playerManager.hitAnimRunning && !playerManager.isHealing)
            {
                isPraying = true;
                //canMove = false;
                rb.velocity = new Vector2(0, 0);
            }
        //Roll
        if (Input.GetKeyDown(KeyCode.LeftShift))
            if (isGrounded && !isRolling && !isPraying && !isAttacking && !isFallAttacking && !playerManager.hitAnimRunning && !playerManager.isHealing && stamina >= 30 && !isGuarding)
                StartCoroutine(Roll()); 
        //Guard
        if (Input.GetMouseButton(1))
            if (isGrounded && !playerManager.hitAnimRunning && stamina >= 10)
                performGuard();
        if (Input.GetMouseButtonUp(1))
            if (isGuarding)
            {
                isGuarding = false;
                parryStamina = false;
                guardTimer = 0;
            }
        //Dagger
        if (Input.GetMouseButtonDown(2))
            if (!isBusy())
                ThrowDagger();
        //Potion
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(isGrounded && !isRolling && !isAttacking && !isFallAttacking && !isPraying && !playerManager.hitAnimRunning && !playerManager.isReviving && !playerManager.isHealing)
            {
                if (Potion.instance.potionCount > 0 && PlayerManager.instance.CurrentHealth < 100)
                {
                    PlayerManager.instance.HealthPotion(33);
                    Potion.instance.UsePotions(1);
                }
            }
        }
        //Toggle Move List
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            moveList.SetActive(!moveList.activeInHierarchy);
        }
    }
    
   private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            
            canClimbLedge = true;
            Debug.Log("canClimbLedge TRUE");

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

            rb.velocity = new Vector2(0,0);
            transform.position = ledgePos1;
            rb.gravityScale = 0;
        }

        if(canClimbLedge && Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = ledgePos2;
            FinishLedgeClimb();
        }
        if(canClimbLedge && Input.GetKeyDown(KeyCode.S))
        {
            FinishLedgeClimb();
        }   
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        rb.gravityScale = gravity;
    }
  
    void Move()
    {
        if (canMove)
        {
            if (!isWallJumping)
            {
                if (!isRolling && !isAttacking && !isPraying && !playerManager.isHealing && !isFallAttacking)
                {
                    if (!isGuarding)
                    {
                        if(!walkToggle)
                            rb.velocity = new Vector2(xAxis * runSpeed, rb.velocity.y);
                        else
                            rb.velocity = new Vector2(xAxis * walkSpeed, rb.velocity.y);
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
            rb.velocity = new Vector2(0,0);
            return;
        }
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGuarding = false;
        /*if (isJumping && jumpTimer < 1) //isGuarding eklenebilir
        {
            jumpTimer += Time.fixedDeltaTime;
            jumpForce = Math.Round(jumpTimer,2);
        }*/
        
    }
    void WallJump()
    {
        if (wallJumpPressed)
        {
            if((wallDirection == 1 && !facingRight) || (wallDirection == -1 && facingRight))
            {
                Flip();
            }

            rb.velocity = new Vector2(xWallForce * wallDirection, 10);
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
        if (canGrab && !isGrounded && !canClimbLedge && xAxis != 0 && !isFallAttacking)
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
        playerManager.damageable = false;
        rollColl.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
        StaminaBar.instance.useStamina(30);
        if (facingRight)
            rb.velocity = new Vector2(rollSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-rollSpeed, rb.velocity.y);

        yield return new WaitForSeconds(.5f);
        isRolling = false;
        playerManager.damageable = true;
        rollColl.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = true;
    }
    void performGuard()
    {
        if(!isRolling && isGrounded && !isPraying && !isAttacking)
        {
            if(!parryStamina)
            {
                StaminaBar.instance.useStamina(10);
                parryStamina = true;
            }
            guardTimer += Time.deltaTime;
            if(guardTimer > 1)
            {
                guardTimer = 0;
                StaminaBar.instance.useStamina(12.5f);
            }
            ChangeAnimationState(parry);
            isGuarding = true;
        }
    }
    
    void FlipPlayer()
    {
        if(!isRolling && !isPraying && !isAttacking)
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
        if(isGrounded && !isFallAttacking && !playerManager.hitAnimRunning && !playerManager.isReviving && !playerManager.isHealing)
        {
            if(!isRolling && !isGuarding)
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
            else if(isRolling && !isGuarding)
                ChangeAnimationState(roll);
        }

        //Air Animations --> Jump and Fall
        if(!isGrounded)
        {
            if(!isAttacking && !isFallAttacking)
            {
                if(rb.velocity.y > 0)
                    ChangeAnimationState(jump);
                if(rb.velocity.y < 0)
                    ChangeAnimationState(fall);    
            }
            else
            {       
                if(isAttacking) 
                {
                    ChangeAnimationState("PlayerAttack" + attackCount);
                    if(attackTime > 0.6f)    
                        isAttacking = false;
                }       
                if(isFallAttacking)
                {
                    ChangeAnimationState(fallattack);
                }
            }
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
        if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.S))
        {
            if((isGrounded && stamina >= 15) || (!isGrounded && stamina >= 25))
            {
                if (!isPraying && !isGuarding && !isRolling && !playerManager.isHealing && !isFallAttacking)
                {
                    if (isCombo && attackTime > 0.3f)
                        Attack();
                    else if (!isCombo)
                        Attack();
                }
            }
        }
        else if (!isGrounded && Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.S) && stamina >= 50 && rb.velocity.y <= 4 && !isFallAttacking && !isWallSliding)
        {
            StaminaBar.instance.useStamina(50);
            isFallAttacking = true;
            rb.velocity = new Vector2(0, rb.velocity.y - 1f);
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            FallAttack();
        }
    }
    void Attack()
    {
        isAttacking = true;
        if(isGrounded)
        {
            StaminaBar.instance.useStamina(15);
            rb.velocity = new Vector2(0,0);
            attackDamage += 2;
        }
        else
        {
            StaminaBar.instance.useStamina(25);
            attackDamage += 1;
        }
        attackCount++;
        isCombo = true;

        if ((attackCount > 3 || attackTime > 0.6f))
        {
            attackCount = 1;
            if(isGrounded)
                attackDamage = 20;
            else
                attackDamage = 10;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.CompareTag("Enemy"))
                enemy.GetComponent<Minion_wfireball>().TakeDamage(attackDamage);
            if(enemy.CompareTag("Villager"))
                enemy.GetComponent<VillagerHealthManager>().TakeDamage(attackDamage);
            if(enemy.CompareTag("Sword"))
                enemy.GetComponent<Sword_Behaviour>().TakeDamage(attackDamage);
            if(enemy.CompareTag("MinionwPoke"))
                enemy.GetComponent<Minion_wpoke>().TakeDamage(attackDamage);
            if(enemy.CompareTag("Legolas"))
                enemy.GetComponent<Legolas>().TakeDamage(attackDamage);
        }
        attackTime = 0f;
    }
    void FallAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(fallAttackBox.position, fallAttackSize, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.CompareTag("Enemy"))
                enemy.GetComponent<Minion_wfireball>().TakeDamage(40);
            if(enemy.CompareTag("Villager"))
                enemy.GetComponent<VillagerHealthManager>().TakeDamage(40);
            if(enemy.CompareTag("Sword"))
                enemy.GetComponent<Sword_Behaviour>().TakeDamage(40);
            if(enemy.CompareTag("MinionwPoke"))
                enemy.GetComponent<Minion_wpoke>().TakeDamage(40);
            if(enemy.CompareTag("Legolas"))
                enemy.GetComponent<Legolas>().TakeDamage(40);
        }
    }
    public void FallAttackTransition()
    {
        if(isGrounded)
        {
            ChangeAnimationState("PlayerFallAttackLanding");
        }
    }
    public void FallAttackDone()
    {
        isFallAttacking = false;
        //rb.constraints = ~RigidbodyConstraints2D.FreezePositionX;
    }
    public void ThrowDagger()
    {
        //Stack'ten gir dagger çıkar ve dagger objesine ata
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
                //Stack'ten çıkarmış dagger objesini Queue'ya yerleştir
                daggerCooldownController.EnqueueItem(dagger);
            }
            else
            {
                dagger.transform.position = firePoint.position;
                dagger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                dagger.GetComponent<Dagger>().Initialize(Vector2.left);
                dagger.SetActive(true);
                StartCoroutine(startDaggerLifeTime());
                //Stack'ten çıkarmış dagger objesini Queue'ya yerleştir
                daggerCooldownController.EnqueueItem(dagger);
            }
            //Daggerların 3 saniye sonra sahneden çıkmasına yarayan coroutine
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
        Gizmos.DrawWireCube(fallAttackBox.position, fallAttackSize);
    }

    public bool isBusy()
    {
        if (isAttacking || isFallAttacking || isGuarding || isPraying || isRolling || playerManager.isReviving || playerManager.hitAnimRunning || playerManager.isHealing)
            return true;
        else
            return false;
    }  
}
