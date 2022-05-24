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

    [Header("Roll")]
    public float dodgeSpeed;
    public float CooldownDuration;
    private float Cooldown;
    private bool IsAvailable;
    private bool isRolling = false;
    public BoxCollider2D regularColl;
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
    private bool attackPressed = false;
    private PlayerManager playerManager;
    [HideInInspector] public bool inCheckpointRange;
    [HideInInspector] public bool dead = false;
    [HideInInspector] public bool isCombo = false;
    [HideInInspector] public bool isGuarding = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        dead = false;
    }

    void Update()
    {
        CheckState();
        CheckInputs();
        CheckAttack();
        ChangeAnimations();
        FlipPlayer();
    }


    void FixedUpdate() 
    {
        RollCooldown();
        if(!isRolling && !isAttacking && !isPraying)
        {
            Move();
            Jump();
        }
    }

    void CheckState()
    {
        //Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void CheckInputs()
    {
        //Get Horizontal Input
        xAxis = Input.GetAxisRaw("Horizontal"); 

        //Check Jump, Attack, Roll, Pray Input  
        if(isGrounded && !isPraying)
        {
            if(!isGuarding)
            {
                if(Input.GetButtonDown("Jump"))
                    jumpPressed = true;
                if (Input.GetMouseButtonDown(0))
                    attackPressed = true;
                if(Input.GetKeyDown(KeyCode.C) && inCheckpointRange)
                    isPraying = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
                performRoll();
            if (Input.GetMouseButton(1) && !playerManager.hitAnimRunning)
                performGuard();
            if (Input.GetMouseButtonUp(1))
                isGuarding = false;
            
        }
    }
    private void CheckAttack()
    {
        attackTime += Time.deltaTime;
        if (attackTime > 0.6f)
            isCombo = false;
        if (Input.GetMouseButtonDown(0))
        {
            if (isCombo == true && attackTime > 0.3f)
                Attack();
            else if (isCombo == false)
                Attack();
        }
    }
    void Move()
    {
        if (!isGuarding)
        {
            rb.velocity = new Vector2(xAxis * runSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(xAxis * runSpeed/2, rb.velocity.y);
        }


    }
    void Jump()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void performGuard()
    {
        if(!isRolling)
            ChangeAnimationState(parry);
        //guard collision
        isGuarding = true;
    }

    private void performRoll()
    {

        // if not available to use (still cooling down) just exit    
        if (!IsAvailable)     //ground check here tooo
        {
            return;
        }

        if (!isRolling)
        {
            // made it here then ability is available to use...
            isRolling = true;
            // start the cooldown timer
            StartCooldown();



            if (facingRight)
            {
                rb.velocity = new Vector2(dodgeSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-dodgeSpeed, rb.velocity.y);
            }

            regularColl.enabled = false;

            StartCoroutine(RollFrameTimer());     //iframe
            StartCoroutine(stopDodge());
        }
    }
    public void StartCooldown()        //roll cooldown
    {
        Cooldown += CooldownDuration / 2;
    }
    void RollCooldown()
    {
        if (Cooldown <= CooldownDuration / 2)
        {
            IsAvailable = true;
        }
        else if (Cooldown > CooldownDuration / 2)
        {
            IsAvailable = false;
        }


        if (Cooldown > 0)
        {
            Cooldown -= 1 * Time.deltaTime;
        }
    }
    IEnumerator RollFrameTimer()        //iframe cooldown
    {
        yield return new WaitForSeconds(iFrame);

        regularColl.enabled = true;                //after I frame 
    }
    IEnumerator stopDodge()
    {
        yield return new WaitForSeconds(0.4f);            //dodge duration                               
        regularColl.enabled = true;

        isRolling = false;
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
    void Attack()
    {
        if (attackPressed && !isPraying)
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
                if (enemy.CompareTag("Villager"))
                    enemy.GetComponent<PreacherHealthManager>().TakeDamage(attackDamage);
            }
            
            
            attackPressed = false;
            attackTime = 0f;
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
