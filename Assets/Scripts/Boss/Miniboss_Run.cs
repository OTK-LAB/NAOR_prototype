using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniboss_Run : StateMachineBehaviour
{
    const string attack = "Miniboss_attack";
    const string idle = "Miniboss_idle";
    public bool isFlipped = false;
    public float attackRange;
    public float speed = 1f;

    Boss_Manager boss;
    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss_Manager>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newpos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        
        LookAtPlayer();

        boss.shieldcoll.SetActive(true);

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            boss.inRange = true;
            if (boss.attackTimer <= 0)
            {
                
                animator.Play(attack);
                boss.attackTimer = boss.autoAttackTimer;
            }else
            {
                animator.Play(idle);
            }
        }else
        {
            boss.inRange = false;
            rb.MovePosition(newpos);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       boss.shieldcoll.SetActive(false);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = rb.transform.localScale;
        flipped.z *= -1f;

        if (rb.transform.position.x < player.position.x && isFlipped)
        {
            rb.transform.localScale = flipped;
            rb.transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (rb.transform.position.x > player.position.x && !isFlipped)
        {
            rb.transform.localScale = flipped;
            rb.transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
}
