using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman_Run : StateMachineBehaviour
{
    const string attack = "Miniboss_attack";     //tüm animation isimleri deðiþip buradan respective þekilde deðiþmeli
    const string idle = "Miniboss_idle";
    public float attackRange;
    public float speed = 1f;

    private bool isFlipped = false;

    Spearman_Manager boss; // tüm "boss"lar spear'a deðiþmeli, düzen, önemli!
    Transform player;
    Rigidbody2D rb;
    Vector3 movement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Spearman_Manager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.playerNear)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newpos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(attack))
                LookAtPlayer();

            if (!boss.shieldBroke)
                boss.shieldcoll.SetActive(true);


            if (Vector2.Distance(player.position, rb.position) <= attackRange)
            {

            }

            if (Vector2.Distance(player.position, rb.position) <= attackRange)
            {
                boss.inRange = true;
                if (boss.attackTimer <= 0)
                {
                    animator.Play(attack);
                    boss.attackTimer = boss.autoAttackTimer;                      //attacks are checked from the animation events
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(attack))
                    {
                        animator.Play(idle);
                        LookAtPlayer();
                    }
                }
            }
            else
            {
                boss.inRange = false;
                rb.MovePosition(newpos);
            }
        }
        else
        {
            CheckAttack();
            AutoMove();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.shieldcoll.SetActive(false);
    }

    public void LookAtPlayer()
    {

        if (rb.transform.position.x < player.position.x && isFlipped)
        {

            rb.transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (rb.transform.position.x > player.position.x && !isFlipped)
        {

            rb.transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    void AutoMove()
    {
        if (!boss.playerNear)
        {
            if (boss.Moveright)
            {
                movement = new Vector3(speed, 0f, 0f);
                rb.transform.position = rb.transform.position + movement * Time.deltaTime;
            }
            else
            {
                movement = new Vector3(-speed, 0f, 0f);
                rb.transform.position = rb.transform.position + movement * Time.deltaTime;
            }
        }
    }

    void flip()
    {
        if (player.position.x > (rb.transform.position.x + 0.5f))
        {
            if (!boss.Moveright)
            {
                rb.transform.Rotate(0f, 180f, 0f);
                boss.Moveright = true;
            }
        }
        else
        {
            if (boss.Moveright)
            {
                rb.transform.Rotate(0f, 180f, 0f);
                boss.Moveright = false;
            }
        }
    }
    void CheckAttack()
    {
        if (Vector2.Distance(rb.transform.position, player.position) <= boss.triggerRange)
        {
            boss.playerNear = true;
        }
        else
            boss.playerNear = false;
    }
}
