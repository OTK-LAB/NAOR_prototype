using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    [SerializeField] private float daggerSpeed;
    private Rigidbody2D rb;
    private GameObject player;
    public bool active = false;
    int hitEnemies = 0;
    private Vector2 direction;
    public float daggerDamage;
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        rb.velocity = direction * daggerSpeed;
    }
    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Minion_wfireball>().TakeDamage(daggerDamage);
            active = true;
            sendHit();
        }
       if (collision.CompareTag("Villager"))
        {
            collision.GetComponent<VillagerHealthManager>().TakeDamage(daggerDamage);
            active = true;
            sendHit();
        }
         if (collision.CompareTag("Sword"))
        {
            collision.GetComponent<Sword_Behaviour>().TakeDamage(daggerDamage);
            active = true;
            sendHit();
        }
         if (collision.CompareTag("MinionwPoke"))
        {
            collision.GetComponent<Minion_wpoke>().TakeDamage(daggerDamage);
            active = true;
            sendHit();
        }
        if (collision.CompareTag("Legolas"))
        {
            collision.GetComponent<Legolas>().TakeDamage(daggerDamage);
            active = true;
            sendHit();
        }



    }
    public void sendHit()
    {
         hitEnemies++;
         player.GetComponent<PlayerController>().speedSkill(hitEnemies);
    }
}
