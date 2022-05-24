using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreacherRunning : MonoBehaviour
{
    private bool playerDetected;
    public float detectedAreaRadius;

    public LayerMask WhatIsPlayer;

    public float speed;

    private Transform playerPos;

    public bool facingRight = true;

    private int b = 0;



    public float distance;


    private Animator animator;
    private string currentState;
    const string idle = "PreacherIdle";
    const string run = "PreacherMoveAnimation";





    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputDirection();
        Detected();
    }

    void CheckInputDirection()
    {
        if (playerPos.position.x > gameObject.transform.position.x && facingRight)
            Flip();
        if (playerPos.position.x < gameObject.transform.position.x && !facingRight)
            Flip();
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }


    void RunAway()
    {
        if (playerPos.position.x > gameObject.transform.position.x)
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        if (playerPos.position.x < gameObject.transform.position.x)
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectedAreaRadius);

    }

    void Detected()
    {


        playerDetected = Physics2D.OverlapCircle(gameObject.transform.position, detectedAreaRadius, WhatIsPlayer);






        if (playerDetected == true || b == 1)
        {

            RunAway();
            ControlOn();
            if (!GetComponent<PreacherHealthManager>().isHurting)
                ChangeAnimationState(run);
        }
        else if (playerDetected == false && b == 1)
        {

            RunAway();
            if (!GetComponent<PreacherHealthManager>().isHurting)
                ChangeAnimationState(run);
        }

        else if (playerDetected == false)
        {

            if (!GetComponent<PreacherHealthManager>().isHurting)
                ChangeAnimationState(idle);
        }


    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    void ControlOn()
    {
        b = 1;
    }

}






   /* void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "regen")
        {
            if (curHealth < maxHealth)
                curHealth += regeneration * Time.deltaTime; ;
        }
    }*/
