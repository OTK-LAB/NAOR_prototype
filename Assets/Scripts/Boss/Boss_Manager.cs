using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Manager : MonoBehaviour
{

    public PlayerManager player;  //do not try this at home
    public int health = 500;

	public GameObject deathEffect;

	private bool isInvulnerable = false;

	[HideInInspector] public float attackTimer;
	public float autoAttackTimer;

	public int attackDamage = 35;
	public int shieldDamage;
	public int enragedAttackDamage = 40;

	public Vector3 attackOffset;
	public float attackRange = 1f;
	public LayerMask attackMask;

    public bool inRange;
	public GameObject shieldcoll;
	Animator anim;


	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
    {
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
    }



	public void Attack()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
			player.DamagePlayer(attackDamage);
			//colInfo.GetComponent<PlayerManager>().DamagePlayer(attackDamage);
		}
	}

    	void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Gizmos.DrawWireSphere(pos, attackRange);
	}


	public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

		health -= damage;
		

		if (health <= 130)
		{
			anim.SetBool("IsEnraged", true);
		}

		if (health <= 0)
		{
			Die();
		}else
        {
			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Miniboss_hit"))
				anim.Play("Miniboss_hit");

        }
	}

	public void Parry()
	{
		Vector3 pos2 = transform.position;
		pos2 += transform.right * attackOffset.x;
		pos2 += transform.up * attackOffset.y;

		Collider2D colInfo2 = Physics2D.OverlapCircle(pos2, attackRange, attackMask);
		if (colInfo2 != null)
		{
			anim.Play("Miniboss_shield");
            player.DamagePlayer(shieldDamage);
			player.StunPlayer(2f);
			//colInfo2.GetComponent<PlayerManager>().DamagePlayer(shieldDamage);
			//colInfo2.GetComponent<PlayerManager>().StunPlayer(2f);
		}
	}

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        anim.Play("Miniboss_dead");
        Invoke("Eliminate", 5f);     
    }
    private void Eliminate()
    {
         Destroy(gameObject);
    }
}
