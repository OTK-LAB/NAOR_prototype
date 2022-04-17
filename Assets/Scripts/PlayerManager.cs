using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private VerticalPlatform verticalPlatform;
    [HideInInspector] public GameObject currentCheckPoint;
    private PlayerController player;
    private Rigidbody2D rb;
    public GameObject reviveEffect;
    private SpriteRenderer spriteRenderer;
    public float flickerSpeed;
    private bool flickering;

    public int lives = 2;
    public float MaxHealth = 100;
    public float CurrentHealth = 100f;
    //[HideInInspector] 
    public bool damageable = true;
    //[HideInInspector] 
    public bool dead = false;
    [HideInInspector] public bool isReviving;
    [HideInInspector] public int status;


    //Animations
    const string hit = "PlayerHit";
    const string death = "PlayerDeath";
    const string revive = "PlayerRevive";
    const string counter = "PlayerCounter";
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

        if (lives == 2)
        {
            if (CurrentHealth >= 100)
            {
                CurrentHealth = 100;
            }
        }

        if (lives == 1)
        {
            if (CurrentHealth >= 40)
            {
                CurrentHealth = 40;
            }
        }
    }

    public virtual void DamagePlayer(float damage)
    {
        if (damageable == true) 
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
                        CurrentHealth -= damage / 2;
                        player.ChangeAnimationState(hit);
                        hitAnimRunning = true;
                        Invoke("CancelHitState", .33f);
                        break;
                    //parry status
                    case 3:
                        player.ChangeAnimationState(counter);
                        //invoke?
                        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackPoint.position, player.attackRange, player.enemyLayers);
                        foreach (Collider2D enemy in hitEnemies)
                        {
                            enemy.GetComponent<Minion_wfireball>().TakeDamage(player.attackDamage * 3);    //parry dealt damage
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
        isReviving = false;
        dead = false;
        //rb.simulated = true; character stays in air when he dies if these lines are active
        player.enabled = true;
        flickering = true;
        yield return new WaitForSeconds(3f);
        damageable = true;
        flickering = false;
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(currentCheckPoint.transform.position.x + 1, transform.position.y, currentCheckPoint.transform.position.z);
        dead = false;
        //rb.simulated = true; character stays in air when he dies if these lines are active
        player.enabled = true;
    }

    //deneme
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "health")
        {
            CurrentHealth = CurrentHealth + 10;
            
        }
        /*One Way Platform*/
        if (other.gameObject.CompareTag("GroundX"))
        {
            verticalPlatform = other.GetComponent<VerticalPlatform>();
            verticalPlatform.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /*One Way Platform*/
        if (other.gameObject.CompareTag("GroundX"))
        {
            verticalPlatform = other.GetComponent<VerticalPlatform>();
            verticalPlatform.enabled = false;
        }
    }
}
