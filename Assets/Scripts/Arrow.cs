using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    Transform PlayerPosition;
    private Vector2 target;
    public float ArrowDamage = 20f;
    private Rigidbody2D rb;
    private float travelDistance;
    private float xStartPos;
    private bool isGravityOn;
    private bool hasItGround=false;
    public GameObject fire;
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
        rb = GetComponent<Rigidbody2D>();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(PlayerPosition.position.x - transform.position.x, PlayerPosition.position.y - transform.position.y);
        Destroy(gameObject, 4f);
        GetComponent<SpriteRenderer>().flipX = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(target) * arrowSpeed;
        GetComponent<Rigidbody2D>().rotation = 0;
        rb.gravityScale = 0.0f;
        xStartPos = transform.position.x;
        isGravityOn = false;
    }

    // Update is called once per frame
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
        }
        if (hasItGround)
        {      
                fire.SetActive(true);
        }
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
            Destroy(gameObject, 3f);
        }
    }
    private void FixedUpdate()
    {
        if (!hasItGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);
            if (damageHit)
            {
                damageHit.transform.SendMessage("DamagePlayer", ArrowDamage);
                DestroyArrow();
            }
            if (groundHit)
            {
                Destroy(gameObject, 3f);
                hasItGround = true;
                rb.gravityScale = 0.0f;
                rb.velocity = Vector2.zero;
                
            }
            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }


        
    }
    public void Fire(float speed,float travelDistance,float damage)
    {
        arrowSpeed = speed;
        this.travelDistance = travelDistance;
        ArrowDamage = damage;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
