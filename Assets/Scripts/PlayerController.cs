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
    public BoxCollider2D rollColl;
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
        Attack();
        ChangeAnimations();    
        FlipPlayer();
        attackTime += Time.deltaTime;
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
            if(Input.GetButtonDown("Jump"))
                jumpPressed = true;
            if (Input.GetMouseButtonDown(0))
                attackPressed = true;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                performRoll();
            if(Input.GetKeyDown(KeyCode.C) && inCheckpointRange)
                isPraying = true;    
        }
    }
    void Move()
    {
        rb.velocity = new Vector2(xAxis * runSpeed, rb.velocity.y);
    }
    void Jump()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
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
                    if(attackTime > 1)    
                        isAttacking = false;
                }
                if(isPraying)
                {
                    ChangeAnimationState(pray);
                    StartCoroutine(StopPraying());
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
            
            if (attackCount > 3 || attackTime > 1)
            {
                attackCount = 1;
                attackDamage = 10;
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Minion_wfireball>().TakeDamage(attackDamage);
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
}
