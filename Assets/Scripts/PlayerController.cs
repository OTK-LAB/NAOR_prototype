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
    private PlayerRoll rollScript;

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
    const string hit = "PlayerHit";
    const string death = "PlayerDeath";
    private bool hitAnimRunning;

    //Combat
    [Header("Combat")]
    public float CurrentHealth = 100f;
    private float attackTime = 0.0f;
    private int attackCount = 0;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    private bool isAttacking;
    private bool attackPressed = false;
    [HideInInspector] public bool dead = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rollScript = GetComponent<PlayerRoll>();
        dead = false;
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
        if(!rollScript.isRolling && !isAttacking)
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

        //Check Jump and Attack Input  
        if(isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
                jumpPressed = true;
            if (Input.GetMouseButtonDown(0))
                attackPressed = true;
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
    void FlipPlayer()
    {
        if(!rollScript.isRolling)
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
        if(isGrounded && !hitAnimRunning)
        {
            if(!rollScript.isRolling)
            {
                if(!isAttacking)
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
    void ChangeAnimationState(string newState)
    {
        if(currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    void Attack()
    {
        if (attackPressed)
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
    public virtual void DamagePlayer(float amount)
    {
        CurrentHealth -= amount;
        ChangeAnimationState(hit);
        hitAnimRunning = true;
        Invoke("CancelHitState", .33f);
        if (CurrentHealth <= 0.0f)
        {
            Die();
        }
    }
    void CancelHitState()
    {
        hitAnimRunning = false;
    }
    void Die()
    {
        dead = true;
        ChangeAnimationState(death);
        rb.simulated = false;
        this.enabled = false;
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
