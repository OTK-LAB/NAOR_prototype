using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public float daggerSpeed;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().facingRight)
            rb.velocity = new Vector2(daggerSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-daggerSpeed, rb.velocity.y);
        Destroy(this.gameObject, 3f);
    }

    void Update()
    {

        //transform.rotation = Quaternion.Euler(Vector3.forward * 2f);

    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }*/
}
