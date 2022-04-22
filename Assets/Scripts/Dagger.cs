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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy dagger after hitting an enemy
            collision.GetComponent<Minion_wfireball>().TakeDamage(5);
        }
        if(collision.gameObject.CompareTag("Villager"))
        {
            //Destroy dagger after hitting an enemy
            collision.GetComponent<VillagerHealthManager>().TakeDamage(5);
        }
    }
}
