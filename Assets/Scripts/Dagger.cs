using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    [SerializeField] private float daggerSpeed;
    private Rigidbody2D rb;
    private Vector2 direction;
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            collision.GetComponent<Minion_wfireball>().TakeDamage(5);
            this.gameObject.SetActive(false);
        }
        if(collision.CompareTag("Villager"))
        {
            collision.GetComponent<VillagerHealthManager>().TakeDamage(5);
            this.gameObject.SetActive(false);
        }
        
        if(collision.CompareTag("Sword"))
        {
            collision.GetComponent<Sword_Behaviour>().TakeDamage(5);
            this.gameObject.SetActive(false);
        }
        if(collision.CompareTag("MinionwPoke"))
        {
            collision.GetComponent<Minion_wpoke>().TakeDamage(5);
            this.gameObject.SetActive(false);
        }
        if(collision.CompareTag("Legolas"))
        {
            collision.GetComponent<Legolas>().TakeDamage(5);
            this.gameObject.SetActive(false);
        }
    }
}
