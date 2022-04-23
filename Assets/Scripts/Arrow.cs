using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 fire_loc;
    public float ArrowDamage = 20f;
    private Rigidbody2D rb;
    private float travelDistance;
    private float xStartPos;
    private bool isGravityOn;
    private bool hasItGround=false;
    public GameObject fire;  
    public GameObject legolas;  
    public static bool disabled = false;

    [SerializeField]
    private float gravity;
    [SerializeField]
    private float damageRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField]
    private Transform damagePosition;
    void Start()
    {
        //target = new Vector2(PlayerPosition.position.x - transform.position.x, PlayerPosition.position.y - transform.position.y);
        rb = GetComponent<Rigidbody2D>();
        legolas = GameObject.FindGameObjectWithTag("Legolas");
        Destroy(gameObject, 10f);
        rb.velocity = Vector3.Normalize(legolas.GetComponent<Legolas>().target);
        // rb.velocity = Vector3.Normalize(target) * arrowSpeed;
        //flip();
        rb.rotation = 0;
        xStartPos = transform.position.x;
        isGravityOn = false;
    }
    public void flip()
    {
        //if (PlayerPosition.position.x > (transform.position.x + 0.5f))
        //{
        //    transform.Rotate(0f, 180f, 0f);
        //}
    }
    void Update()
    {
        if (!hasItGround)
        {
            rb.position = transform.position;
            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           Destroy(gameObject);
           collision.transform.SendMessage("DamagePlayer", ArrowDamage);
        }
        if (collision.CompareTag("Ground"))
        {            
            fire_loc = new Vector3(transform.position.x, (transform.position.y  + 0.16f), 0);
            Instantiate(fire, fire_loc, Quaternion.LookRotation(Vector3.forward, fire_loc));
            hasItGround = true;
            rb.gravityScale = 0.0f;
            rb.simulated = false;
            Destroy(gameObject, 3f);
            rb.velocity = Vector2.zero;
        }
    }
    public void Fire(float speed,float travelDistance,float damage)
    {
        //arrowSpeed = speed;
        legolas.GetComponent<Legolas>().LaunchForce = speed;
        this.travelDistance = travelDistance;
        ArrowDamage = damage;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
