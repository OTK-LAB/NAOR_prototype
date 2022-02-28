using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillegarRunning : MonoBehaviour
{
    public float speed;
    
    public float EnemyDistanceRun = 4.0f;

    private Transform playerPos;

    public bool PlayerFacingRight = true;



    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputDirection();
        Debug.Log(PlayerFacingRight);

        
        float distance = Vector2.Distance(transform.position, playerPos.position);
        //Debug.Log("Distance : " + distance);


        Vector2 position = transform.position;
        Vector2 dirToPlayer = transform.position - playerPos.position;
        Vector2 newPos = position + dirToPlayer;

        if (playerPos.position.x > gameObject.transform.position.x && PlayerFacingRight)
            Flip();
        if (playerPos.position.x < gameObject.transform.position.x && !PlayerFacingRight)
            Flip();
    

        if (distance < EnemyDistanceRun)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        }    
    }

    void CheckInputDirection()
    {
        if (PlayerFacingRight == true && GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            Flip();
        }
        else if (PlayerFacingRight == false && GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            Flip();
        }
    }
    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        PlayerFacingRight = !PlayerFacingRight;
    }
}
