using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillegarRunning : MonoBehaviour
{
    private bool playerDetected;
    public float detectedarearadius;

    public LayerMask WhatIsPlayer;
    
    public float speed;

    private Transform playerPos;
    
    public bool PlayerFacingRight = true;

    private int b = 0;

    private bool playerGroundDetected;
    public Transform groundDetection;
    public float distance;
    public LayerMask WhatIsGround;





    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputDirection();
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
        if (playerPos.position.x > gameObject.transform.position.x )
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);

        if (playerPos.position.x < gameObject.transform.position.x )
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectedarearadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundDetection.position, distance);
    }

    void Detected()
    {
        playerDetected = Physics2D.OverlapCircle(gameObject.transform.position, detectedarearadius, WhatIsPlayer);

        playerGroundDetected = Physics2D.OverlapCircle(groundDetection.transform.position, distance, WhatIsGround);

        if (playerGroundDetected == true)
        {
            if (playerDetected == true)
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

            else if (playerDetected == false)
            {
                Debug.Log("Not Detected");
            }
        }
        else if(playerGroundDetected == false)
        {

        }
    }

    void ControlOn()
    {
        b = 1;
    }

}
