using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_wpoke : MonoBehaviour
{
    //Animations
    private Animator animator;
    private string currentState;
    const string idle = "EnemyIdle";
    const string hit = "EnemyHit";
    const string attack = "EnemyAttack";
    const string death = "EnemyDeath";
    
    
    //Move
    private Rigidbody2D rb;
    Vector3 movement;
    public float speed = 5;
    bool Moveright = false;

    //Hit
    public int maxHealth = 100;
    public int currentHealth;


    //Attack
    public float CalculatedTime;
    public float TimeBtwEachShot;
    private GameObject Fireball;
    bool playerOnline = false;
    private Transform PlayerPosition;
    private GameObject player;
    public float minimumFiringDistance;
    public float maxFiringDistance;
    public float damage = 12.5f;
    float totalDamage;
    bool playerAlive = true;
    float cr;

    void Start()
    {
        animator = GetComponent<Animator>();   
        currentHealth = maxHealth;
        CalculatedTime = TimeBtwEachShot; 
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");

        //ChangeAnimationState(idle);
    }

    void Update()
    {
        if(playerAlive)
            CheckAttack();
        AutoMove();
        CheckPlayerDead();
        //ChangeAnimations();
    }
    void CheckPlayerDead()
    {
        if (player.GetComponent<PlayerController>().dead == true)
        {
            playerOnline = false;
            playerAlive = false;
        }
    }
    void CheckAttack()
    {
        if (Vector2.Distance(transform.position, PlayerPosition.position) <= minimumFiringDistance)
        {
            if(!playerOnline) cr = transform.position.x;
            playerOnline = true;          
            if (Moveright) { transform.Rotate(0f, 180f, 0f); Moveright = false; }
            AttackMechanism();
        }
        else
        {
            playerOnline = false;
        }
    }
    void AutoMove()
    {
        if (!playerOnline)
        {
            if (Moveright)
            {
                movement = new Vector3(2, 0f, 0f);
                transform.position = transform.position + movement * Time.deltaTime;
            }
            else
            {
                movement = new Vector3(-2, 0f, 0f);
                transform.position = transform.position + movement * Time.deltaTime;
            }
        }
    }
   
    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.CompareTag("turn")&& !playerOnline)
        {
            if (Moveright) Moveright = false;
            else Moveright = true;
            transform.Rotate(0f, 180f, 0f);
        }
        if (trig.CompareTag("Player"))
        {       
            trig.transform.SendMessage("DamagePlayer", damage);
            totalDamage += damage;
        }

    }
    void AttackMechanism()
    {
            if (CalculatedTime <= 0)
            {
            
                movement = new Vector3((PlayerPosition.position.x + 0.5f), -2.5f, 0f);
                transform.position = movement;
                animator.SetBool("Attack", true);
                CalculatedTime = TimeBtwEachShot;
                
            }
            else
            {
            movement = new Vector3(cr, -2.5f, 0f);
            transform.position = movement;
            CalculatedTime -= Time.deltaTime;
            animator.SetBool("Attack", false);
        }
    }

    void ChangeAnimations()
    {
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            animator.SetBool("Alive", false);
            Die();           
        }
    }
    void Die()
    {
        ChangeAnimationState(death);
        movement = new Vector3(transform.position.x, -3.10f, transform.position.z);
        transform.position = movement;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

}
