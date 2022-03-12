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

    private int lives = 2;
    public float MaxHealth = 100;
    public float CurrentHealth = 100f;
    //[HideInInspector] 
    public bool damageable = true;
    //[HideInInspector] 
    public bool dead = false;
    [HideInInspector] public bool isReviving;


    //Animations
    const string hit = "PlayerHit";
    const string death = "PlayerDeath";
    const string revive = "PlayerRevive";
    [HideInInspector] public bool hitAnimRunning;





    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        player = GetComponent<PlayerController>(); 
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(flickering)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.PingPong(Time.time * flickerSpeed, 1));
        else
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, 1);    
    }

    public virtual void DamagePlayer(float damage)
    {
        if (damageable == true) 
        {
            if ((CurrentHealth - damage) >= 0)
            {
                CurrentHealth -= damage;
                player.ChangeAnimationState(hit);
                hitAnimRunning = true;
                Invoke("CancelHitState", .33f); 
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
                rb.simulated = false;
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
                rb.simulated = false;
                player.enabled = false;
                StartCoroutine(RespawnPlayer());
            }
        }
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
        rb.simulated = true;
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
        rb.simulated = true;
        player.enabled = true;
    }

}
