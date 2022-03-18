using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_wpoke : MonoBehaviour
{
    //Animations
    private Animator animator;
    private string currentState;
    const string idle = "enemy_idle";
    const string hit = "enemy_hit";
    const string attack = "enemy_attack";
    const string death = "enemy_death";

    //Move
    [Header("Movement")]
    public int speed = 2;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    Vector3 movement;
    bool Moveright = true;
    float step = 2;
    

    //Hit
    [Header("Health")]
    public float maxHealth = 100, currentHealth;
    bool hurt = false, alive = true;

    //Attack
    [Header("Attack")]
    public float TimeBtwEachShot;
    public float minimumFiringDistance, damage = 12.5f;
    float target, cr;
    bool collision = false, playerOnline = false, playerAlive = true, ifAttack = false;
    Transform PlayerPosition;
    private GameObject player;


    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        sr =GetComponent<SpriteRenderer>(); 
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        target = PlayerPosition.position.x - transform.position.x;
        moveDecision();
        CheckPlayerDead();
    }
    void moveDecision()
    {
        if (!playerOnline && !collision)
        {
            if (!ifAttack)
            {
                speed = 2;
                AutoMove();
                CheckAttack();
            }
        }
        if (!collision && playerOnline)
        {
            flip();
            Enemy_Move(target);
        }
        else if (collision && playerOnline)
            attackDirection();
        if (collision && !playerOnline) //player hortlamýþ
            attackDirection();
    }
    void attackDirection()
    {
        flip();
        if (!Moveright)
        {
            turnRight();   
            if (transform.position.x >= cr)
            {
                playerOnline = false;
                collision = false;
                ifAttack = true;
                turnLeft();
                waitForAttack();
            }
            else
                Enemy_Move(cr);
        }
        else
        {
            turnLeft();
            if (transform.position.x < cr)
            {
                playerOnline = false;
                collision = false;
                ifAttack = true;
                turnRight();
                waitForAttack();
            }
            else
                Enemy_Move(-cr);
        }
    }
    void waitForAttack()
    {
            Enemy_Move(0f);
            StartCoroutine(waitdarling());
    }
    IEnumerator waitdarling()
    {
        yield return new WaitForSeconds(TimeBtwEachShot);
        speed = 2;
        ifAttack = false;
        CheckAttack();
    }
    void Enemy_Move(float count)
    {
        Vector3 direction;
        direction = new Vector3(count, 0f, 0f);
        direction.Normalize();
        movement = direction;
        moveCharacter(movement);
    }
    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
    void turnRight()
    {
        sr.flipX = true;
        Moveright = true;
    }
    void turnLeft()
    {
        sr.flipX = false;
        Moveright = false;
    }
    void flip()
    {
        if (PlayerPosition.position.x > (transform.position.x + 0.5f))
        {
            if (!Moveright)
                turnRight();
        }
        else
        {
            if (Moveright)
                turnLeft();
        }
    }
    void AutoMove()
    {
        speed = 2;
        if (Moveright)
            Enemy_Move(step);
        else
            Enemy_Move(-step);
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.CompareTag("turn") && !playerOnline)
        {
            if (Moveright) Moveright = false;
            else Moveright = true;
            transform.Rotate(0f, 180f, 0f);
        }
        if (trig.CompareTag("Player"))
        {
            ChangeAnimations();
            trig.transform.SendMessage("DamagePlayer", damage);
            collision = true;
            flip();
        }

    }
    void CheckAttack()
    {
        if (!playerOnline)
        {
            if (Vector2.Distance(transform.position, PlayerPosition.position) <= minimumFiringDistance)
            {
                speed *= 2;
                flip();
                cr = transform.position.x;
                playerOnline = true;
            }
        }
    }
    void CheckPlayerDead()
    {
        if (player.GetComponent<PlayerManager>().dead == true)
        {
            playerOnline = false;
            playerAlive = false;
        }
        else
        {
            playerAlive = true;
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
        //attack
        if (playerOnline /*&& CalculatedTime <= 0*/)
        {
            ChangeAnimationState(attack);
            StartCoroutine(backtoIdle());
        }

        //hit
        if (hurt && alive)
        {
            ChangeAnimationState(hit);
            StartCoroutine(backtoIdle());
        }

    }
    IEnumerator backtoIdle()
    {

        yield return new WaitForSeconds(0.5f);
        if (alive)
            ChangeAnimationState(idle);
        hurt = false;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            alive = false;
            hurt = false;
            Die();
        }
        else
        {
            hurt = true;
            ChangeAnimations();
        }
    }
    void Die()
    {
        ChangeAnimationState(death);
        movement = new Vector3(transform.position.x, -3.17f, transform.position.z);
        transform.position = movement;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

    }

}
