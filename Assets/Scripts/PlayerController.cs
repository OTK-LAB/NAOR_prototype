using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement
    [Header("Movement")]
    public float runSpeed;
    private bool runPressed;
    private float xAxis;
    private bool facingRight = true;
    private Rigidbody2D rb;

    //Jumping
    [Header("Jumping")]
    public float jumpForce;
    private bool jumpPressed = false;
    private bool isGrounded;
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
    const string attack1 = "PlayerAttack1";
    const string attack2 = "PlayerAttack2";
    const string attack3 = "PlayerAttack3";
    const string hit = "PlayerHit";
    const string death = "PlayerDeath";
    public float exitTime;

    //Attack
    float attack_time = 0.0f;
    int attack_count = 0;
    public float CurrentHealth = 100f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    bool attackPressed = false;

    //Die
    Vector3 movement;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckState();        
        CheckInputs();
        ChangeAnimations();    
        FlipPlayer();
        CheckIfAttack();
        attack_time += Time.deltaTime;
    }

    void FixedUpdate() 
    {
        Move();
        Jump();
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

        //Get Jump Input
        if(Input.GetButtonDown("Jump") && isGrounded)
            jumpPressed = true;
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
    void ChangeAnimations()
    {
        //Idle and Run
        if(isGrounded)
        {
            if(xAxis == 0)
                ChangeAnimationState(idle);
            else
                ChangeAnimationState(run);    
        }

        //Jump and Fall
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
    void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    void CheckIfAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackPressed = true;
            Attack();
        }
    }
    void Attack()
    {
        if (attackPressed)
        {
            attackDamage += 2;
            attack_count++;
            if (attack_count > 3)
            {
                attack_count = 1;
                attackDamage = 10;
            }

            if (attack_time > 1.0f)
            {
                attack_count = 1;
                attackDamage = 10;
            }

            animator.SetTrigger("Attack" + attack_count);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }

            attack_time = 0f;
           
        }

    }
    public virtual void DamagePlayer(float amount)
    {
        CurrentHealth -= amount;
        animator.SetTrigger("Hit");
        if (CurrentHealth <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        ChangeAnimationState(death);
        movement = new Vector3(transform.position.x, -3.83f, transform.position.z);
        transform.position = movement;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }


    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
