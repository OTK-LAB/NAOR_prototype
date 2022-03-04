using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{

    private PlayerController Player;         //ground check pls

    public bool isRolling = false;

    private Rigidbody2D rigidbdy;

    private Animator anim;

    public BoxCollider2D regularColl;

    public BoxCollider2D worldColl;

    public float dodgeSpeed = 5f;

    public bool IsAvailable = true;
    public float CooldownDuration = 6.0f;
    private float Cooldown = 0f;

    public float iFrame = 0.3f;

    public void StartCooldown()        //roll cooldown
    {
        Cooldown += CooldownDuration / 2;
    }



    public IEnumerator FrameTimer()        //iframe cooldown
    {
        yield return new WaitForSeconds(iFrame);

        regularColl.enabled = true;                //after I frame 
    }

    void Start()
    {
        Player = GetComponent<PlayerController>();
        rigidbdy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Player.isGrounded)       //key bind here
        {
            performDodge();
        }
    }

    private void FixedUpdate()
    {
        if (Cooldown <= CooldownDuration / 2)
        {
            IsAvailable = true;
        }
        else if (Cooldown > CooldownDuration / 2)
        {
            IsAvailable = false;
        }


        if (Cooldown > 0)
        {
            Cooldown -= 1 * Time.deltaTime;
        }
    }

    private void performDodge()
    {

        // if not available to use (still cooling down) just exit    
        if (!IsAvailable)     //ground check here tooo
        {
            return;
        }

        if (!isRolling)
        {
            // made it here then ability is available to use...
            isRolling = true;
            // start the cooldown timer
            StartCooldown();



            if (Player.facingRight)
            {
                rigidbdy.velocity = new Vector2(dodgeSpeed, rigidbdy.velocity.y);
            }
            else
            {
                rigidbdy.velocity = new Vector2(-dodgeSpeed, rigidbdy.velocity.y);
            }

            regularColl.enabled = false;

            StartCoroutine(FrameTimer());     //iframe
            StartCoroutine(stopDodge());
        }
    }

    public IEnumerator stopDodge()
    {
        yield return new WaitForSeconds(0.4f);            //dodge duration
        //anim.Play("Idle");                                
        regularColl.enabled = true;

        isRolling = false;
    }


}
