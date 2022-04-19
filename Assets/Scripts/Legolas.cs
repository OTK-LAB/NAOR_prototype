using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legolas : MonoBehaviour
{

    //Animations
    private Animator animator;
    private string currentState;
    const string idle = "legolas_idle";
    const string hit = "legolas_hit";
    const string attack = "legolas_attack";
    const string death = "legolas_death";

    //Move
    Vector3 movement;
    bool Moveright = true;

    //Hit
    public float maxHealth = 60;
    public float currentHealth;
    bool hurt = false;
    bool alive = true;

    //Attack
    public float CalculatedTime;
    public float TimeBtwEachShot;
    public GameObject Arrow;
    bool playerOnline = false;
    Transform PlayerPosition;
    public float minimumFiringDistance;
    public float damage = 20;
    private GameObject player;
    bool playerAlive = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth * Random.Range(1, 1.2001f);
        CalculatedTime = TimeBtwEachShot;
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAlive)
            CheckAttack();
        CheckPlayerDead();
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
    void CheckAttack()
    {
        if (Vector2.Distance(transform.position, PlayerPosition.position) <= minimumFiringDistance)
        {
           // flip();
            playerOnline = true;
            ArrowMechanism();
        }
        else
            playerOnline = false;
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    void ArrowMechanism()
    {
        if (CalculatedTime <= 0)
        {
            ChangeAnimations();
            Instantiate(Arrow, transform.position, Quaternion.LookRotation(Vector3.forward, transform.position - PlayerPosition.position));
            CalculatedTime = TimeBtwEachShot;
        }
        else
        {
            CalculatedTime -= Time.deltaTime;
            ChangeAnimations();
        }
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
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

    }
}
