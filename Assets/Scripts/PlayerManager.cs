using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [HideInInspector] public GameObject currentCheckPoint;
    private PlayerController player;
    private Rigidbody2D rb;
    public GameObject reviveEffect;
    private SpriteRenderer spriteRenderer;
    public float flickerSpeed;
    private bool flickering;

    public static PlayerManager instance;

    private int lives = 2;
    public float MaxHealth = 100;
    public float CurrentHealth = 100f;
    public bool isHealing;
    //[HideInInspector] 
    public bool damageable = true;
    //[HideInInspector] 
    public bool dead = false;
    [HideInInspector] public bool isReviving;
    public int status;
    private bool revived = false;


    //Animations
    const string hit = "PlayerHit";
    const string death = "PlayerDeath";
    const string revive = "PlayerRevive";
    const string counter = "PlayerCounter";
    const string heal = "PlayerHeal";
    [HideInInspector] public bool hitAnimRunning;





    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        player = GetComponent<PlayerController>(); 
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCheckPoint = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(flickering)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.PingPong(Time.time * flickerSpeed, 1));
        else
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, 1);
        if (status == 1 || status == 2) // continue moving after a parry
            player.canMove = true;
    }

    private void Awake()
    {
        instance = this;
    }
    public void HealthPotion(float health)
    {
        if(!revived)
            CurrentHealth += health;
        player.ChangeAnimationState(heal);
        isHealing = true;
        Invoke("CancelHealState", 0.8f);
        if (CurrentHealth > 100)
        {
            CurrentHealth = 100;
        }
            
    }
    void CancelHealState()
    {
        isHealing = false;
    } 
    public virtual void DamagePlayer(float damage)
    {
        if (damageable)
        {
            if ((CurrentHealth - damage) >= 0)
            {
                switch (status)
                {
                    //normal damage status
                    case 1:
                        CurrentHealth -= damage;
                        player.ChangeAnimationState(hit);
                        hitAnimRunning = true;
                        Invoke("CancelHitState", .33f);
                        break;
                    //blocking damage status
                    case 2:
                        CurrentHealth -= damage * 0.6f;
                        player.ChangeAnimationState(hit);
                        hitAnimRunning = true;
                        Invoke("CancelHitState", .33f);
                        break;
                    //parry status
                    case 3:
                        player.canMove = false;             // stop when parrying
                        player.ChangeAnimationState(counter);
                        //invoke?
                        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackPoint.position, player.attackRange, player.enemyLayers);
                        foreach (Collider2D enemy in hitEnemies)
                        {
                            enemy.GetComponent<Minion_wfireball>().TakeDamage(player.attackDamage * 1.25f);    //parry dealt damage
                            enemy.GetComponent<Spearman_Manager>().TakeDamage(player.attackDamage * 1.25f);
                        }
                        break;
                }
            }
            else
            {
                CurrentHealth = 0;

            }
            Die();
        }
    }

    public void StunPlayer(float stuntime)
    {
        player.ChangeAnimationState(hit);
        if (player.facingRight)
              rb.AddForce(new Vector2(-100,0));
        else
              rb.AddForce(new Vector2(100,0));
        player.isStunned = true;
        StartCoroutine(Stunned(stuntime));
    }

    IEnumerator Stunned(float stuntime)
    {
        player.ChangeAnimationState(hit);
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(stuntime);
        rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        player.isStunned = false;
    }
    void CancelHitState()
    {
        hitAnimRunning = false;
    }
    void Die()
    {
        if (CurrentHealth <= 0)
        {
            lives--;
            if (lives == 1)
            {
                dead = true;
                //rb.simulated = false; character stays in air when he dies if these lines are active
                player.enabled = false;
                player.ChangeAnimationState(death);
                StartCoroutine(DeathDefiance());
                CurrentHealth = (MaxHealth * 40) / 100;

            }
            if (lives == 0)
            {
                dead = true;
                player.ChangeAnimationState(death);
                CurrentHealth = MaxHealth;
                lives = 2;
                //rb.simulated = false; character stays in air when he dies if these lines are active
                player.enabled = false;
                StartCoroutine(RespawnPlayer());
            }
        }
    }

    void StatusChanger(int a)
    {
        status = a;
    }

    IEnumerator DeathDefiance()
    {
        damageable = false;
        yield return new WaitForSeconds(1f);
        isReviving = true;
        Instantiate(reviveEffect, transform.position, Quaternion.identity);
        player.ChangeAnimationState(revive);
        yield return new WaitForSeconds(.5f);
        revived = true;
        isReviving = false;
        dead = false;
        //rb.simulated = true; character stays in air when he dies if these lines are active
        player.enabled = true;
        flickering = true;
        yield return new WaitForSeconds(2f);
        damageable = true;
        flickering = false;
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1f);
        revived = false;
        transform.position = new Vector3(currentCheckPoint.transform.position.x + 1, transform.position.y, currentCheckPoint.transform.position.z);
        dead = false;
        //rb.simulated = true; character stays in air when he dies if these lines are active
        player.enabled = true;
    }

}
