using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillegarRunning : MonoBehaviour
{
    private bool playerDetected;
    public float detectedarearadius;

    public LayerMask WhatIsPlayer;
    
    public float speed;
    
    public float EnemyDistanceRun = 4.0f;

    private Transform playerPos;
    
    public bool PlayerFacingRight = true;

    private int b = 0;
    private float a = 0;

    private float timer = 0.0f;
    public float resettime = 5.0f;



    

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputDirection();
        //RunningTiming();
        //RunAway();
        Timer();
        Detected();

    }

    void CheckInputDirection()
    {
        if (playerPos.position.x > gameObject.transform.position.x && PlayerFacingRight) 
            Flip();
        if (playerPos.position.x < gameObject.transform.position.x && !PlayerFacingRight)
            Flip();

        Debug.Log(PlayerFacingRight);
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        PlayerFacingRight = !PlayerFacingRight;
    }


    void RunAway()
    {

        /*
        float distance = Vector2.Distance(transform.position, playerPos.position);
        //Debug.Log("Distance : " + distance);
        Vector2 position = transform.position;
        Vector2 dirToPlayer = transform.position - playerPos.position;
        Vector2 newPos = position + dirToPlayer;
        */
        //if (distance < EnemyDistanceRun)
        /*
        {
            //transform.position = Vector2.MoveTowards(transform.position, newPos, speed * Time.deltaTime);

           
        }
        */

        if (playerPos.position.x > gameObject.transform.position.x )
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);

        if (playerPos.position.x < gameObject.transform.position.x )
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, EnemyDistanceRun);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectedarearadius);
    }

    /*
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerDetected = Physics2D.OverlapBox(gameObject.transform.position, new Vector2(width, height), 0, WhatIsPlayer);

        if (playerDetected == true) 
        {
            if (collision.gameObject.tag == "Player")
            {
                RunAway();
            }
        }
    }
    */

    
    void Detected()
    {
        playerDetected = Physics2D.OverlapCircle(gameObject.transform.position, detectedarearadius, WhatIsPlayer);



        if (playerDetected == true )
        {
            Debug.Log("Detected");
            RunAway();           
            ControlOn();
        }
        else if (playerDetected == false && b == 1)
        {
            Debug.Log("Not Detected but run");
            RunAway();
        }
        else if(playerDetected == false)
        {
            Debug.Log("Not Detected");
        }
        

        
    }

    void ControlOn()
    {
        b = 1;
    }

    void Timer()
    {
        if (playerDetected == true) 
        {
            timer = Time.time;
            Debug.Log(timer);
            
        }
        a = timer;
        if (playerDetected == false)
        {
            if (a < resettime)
            {
                timer = Time.time;
                Debug.Log(timer);
            }
            if (a >= resettime)
            {
                timer = 0.0f;
                
                b = 0;
                Debug.Log(timer);
            }
        }

        
    }
}
