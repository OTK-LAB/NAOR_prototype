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
    Vector3 movement;
    bool Moveright = true;
    private Rigidbody2D rb;
    float step = 2;
    public int speed = 2;

    //Hit
    public float maxHealth = 100;
    public float currentHealth;
    bool hurt = false;
    bool alive = true;


    //Attack
    float target;
    bool collision = false;
    float cr;
    public float CalculatedTime;
    public float TimeBtwEachShot;
    bool playerOnline = false;
    Transform PlayerPosition;
    public float minimumFiringDistance;
    public float damage = 12.5f;
    private GameObject player;
    bool playerAlive = true;
    bool deneme = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        CalculatedTime = TimeBtwEachShot;
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        target = Vector2.Distance(transform.position, PlayerPosition.position);
        if (!playerOnline && !collision)
        {
            if (!deneme)
            {
                speed = 2;
                Debug.Log("girdim");
                AutoMove();
                CheckAttack();
            }
        }
        if (!collision && playerOnline)
        {
            Debug.Log(playerOnline+ "saldýr" + collision);
            Enemy_Move(PlayerPosition.position.x);
        }

        else if (collision && playerOnline)
        {
            Debug.Log(playerOnline + "geri çekil " + collision);
            if (transform.position.x>=cr)
            {
                playerOnline = false;
                collision = false;
                deneme = true;
                waitForAttack();
            }   
            else
            {
                Enemy_Move(cr);
            }
               
        }
        CheckPlayerDead();

    }
    void waitForAttack()
    {
            Enemy_Move(0f);
            StartCoroutine(waitdarling());
    }
    IEnumerator waitdarling()
    {
        yield return new WaitForSeconds(TimeBtwEachShot);
        deneme = false;
        CheckAttack();

    }
    void Enemy_Move(float count)
    {
        Vector3 direction = new Vector3(count, 0f, 0f);
        direction.Normalize();
        movement = direction;
        moveCharacter(movement);
    }
    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
    void flip()
    {
        if (PlayerPosition.position.x > (transform.position.x + 0.5f))
        {
            if (!Moveright)
            {
                transform.Rotate(0f, 180f, 0f);
                Moveright = true;
            }
        }
        else
        {
            if (Moveright)
            {
                transform.Rotate(0f, 180f, 0f);
                Moveright = false;
            }
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
            trig.transform.SendMessage("DamagePlayer", damage);
            collision = true;
            Debug.Log("carptim");
            flip();
        }

    }
    void CheckAttack()
    {
        if (!playerOnline)
        {

            if (target <= minimumFiringDistance)
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
            playerAlive = true;
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
        if (playerOnline && CalculatedTime <= 0)
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
