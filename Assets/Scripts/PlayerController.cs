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
    public Transform attackPoint;
    private float attackTime = 0.0f;
    private int attackCount = 0;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    private bool isAttacking;
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

    //gems icin eklediklerim
    public float rollStaminaRate=0;
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
        stamina = StaminaBar.instance.currentStamina;
        if (daggerCooldownController.GetQueue().Count > 0)
        {
            if (Time.time >= daggerCooldownController.GetDequeueTime())
            {
                //DequeueLastItem() fonksiyonuyla Queue'dan ��kard���n dagger'� Stack'e koy
                daggerStack.PushToStack(daggerCooldownController.DequeueLastItem());
            }
        }
    }
    void FixedUpdate()
    {
        Move();
        //Jump();
    }
    void CheckState()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void CheckInputs()
    {     
        //Get Horizontal Input
        xAxis = Input.GetAxisRaw("Horizontal");
        //Walk Toggle
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            walkToggle = !walkToggle;
        //Jump
        if(Input.GetButtonDown("Jump"))
        {
            if (isGrounded && !isRolling && !isPraying && !isAttacking && !playerManager.isHealing)
            {
                isJumping = true;
                jumpTimeCounter = jumpTimer;
                Jump();
            }
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
            if (isGrounded && inCheckpointRange && !isPraying && !isGuarding && !isRolling && !playerManager.hitAnimRunning && !playerManager.isHealing)
                isPraying = true;
        //Roll
        if (Input.GetKeyDown(KeyCode.LeftShift))
            if (isGrounded && !isRolling && !isPraying && !playerManager.hitAnimRunning && !playerManager.isHealing && stamina >= 30)
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
            if(isGrounded && !isRolling && !isAttacking && !isPraying && !playerManager.hitAnimRunning && !playerManager.isReviving && !playerManager.isHealing)
            {
                if (Potion.instance.potionCount > 0 && PlayerManager.instance.CurrentHealth < 100)
                {
                    PlayerManager.instance.HealthPotion(33);
                    Potion.instance.UsePotions(1);
                }
            }
        }
    }
    
  
    void Move()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2 (0,0);
            return;
        }

        if (!isRolling && !isAttacking && !isPraying)
        {
            if (!isGuarding)
            {
                if(!walkToggle)
                    rb.velocity = new Vector2(xAxis * runSpeed, rb.velocity.y);
                else
                    rb.velocity = new Vector2(xAxis * walkSpeed, rb.velocity.y);
            }
            else
                rb.velocity = new Vector2(xAxis * runSpeed / 2, rb.velocity.y);
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
    IEnumerator Roll()
    {
        isRolling = true;
        playerManager.damageable = false;
        rollColl.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
        StaminaBar.instance.useStamina(30*rollStaminaRate/100);
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
        if(!isRolling && isGrounded && !isPraying && !isAttacking)
        {
            ChangeAnimationState(parry);
            isGuarding = true;
        }
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
        if(isGrounded && !playerManager.hitAnimRunning && !playerManager.isReviving && !playerManager.isHealing)
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
            if (!isPraying && !isGuarding && isGrounded && !isRolling && !playerManager.isHealing && stamina >= 15)
                if (isCombo && attackTime > 0.3f)
                    Attack();
                else if (!isCombo)
                    Attack();
        }
    }
    void Attack()
    {
        StaminaBar.instance.useStamina(15);
        isAttacking = true;
        rb.velocity = new Vector2(0,0);
        attackDamage += 2;
        attackCount++;
        isCombo = true;

        if (attackCount > 3 || attackTime > 0.6f)
        {
            attackCount = 1;
            attackDamage = 20;
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
                enemy.GetComponent<Minion_wpoke>().TakeDamage(attackDamage * 1.25f);
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
        if (isAttacking || isGuarding || isPraying || isRolling || playerManager.isReviving || playerManager.hitAnimRunning || playerManager.isHealing)
            return true;
        else
            return false;
    }    
}
