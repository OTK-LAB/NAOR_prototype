using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    Transform PlayerPosition;
    private Vector2 target;
    public float ArrowDamage = 20f;
    void Start()
    {
        
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(PlayerPosition.position.x - transform.position.x, PlayerPosition.position.y - transform.position.y);
        Destroy(gameObject, 4f);
        GetComponent<SpriteRenderer>().flipX = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(target) * arrowSpeed;
        GetComponent<Rigidbody2D>().rotation = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroyArrow()
    {
        Destroy(gameObject);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DestroyArrow();
            collision.transform.SendMessage("DamagePlayer", ArrowDamage);
        }
        if (collision.CompareTag("Ground"))
        {
            //Fire scripti çalýþmalý 
            DestroyArrow();
        }
    }
}
