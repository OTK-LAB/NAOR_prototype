using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_wfireball : MonoBehaviour
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
    bool Moveright = false;

    //Hit
    public int maxHealth = 100;
    public int currentHealth;


    //Attack
    public float CalculatedTime;
    public float TimeBtwEachShot;
    public GameObject Fireball;
    bool playerOnline = false;
    Transform PlayerPosition;
    public float minimumFiringDistance;
    public float maxFiringDistance;
    public float damage = 12.5f;
    private GameObject player;
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
        if (player.GetComponent<PlayerManager>().dead == true)
        {
            playerOnline = false;
            playerAlive = false;
            animator.SetBool("Attack", false);
        }
        else
            playerAlive = true;
    }
    void CheckAttack()
    {
        if (Vector2.Distance(transform.position, PlayerPosition.position) <= minimumFiringDistance)
        {
            playerOnline = true;          
            if (Moveright) { transform.Rotate(0f, 180f, 0f); Moveright = false; }
            FireballMexhanism();
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
    void FireballMexhanism()
    {
        if (CalculatedTime <= 0)
        {
            animator.SetBool("Attack", true);
            Instantiate(Fireball, transform.position, Quaternion.LookRotation(Vector3.forward, transform.position - PlayerPosition.position));
            CalculatedTime = TimeBtwEachShot;
        }
        else
        {
            CalculatedTime -= Time.deltaTime;
            animator.SetBool("Attack", false);
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    void ChangeAnimations()
    {
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
        //cr = transform.position.y;
        animator.applyRootMotion=false;
        ChangeAnimationState(death);
        StartCoroutine(waitingDarling());

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
    IEnumerator waitingDarling()
    {
        yield return new WaitForSeconds(0.2f);
        movement = new Vector3(transform.position.x, -3.10f, transform.position.z);
        transform.position = movement;
    }

}
