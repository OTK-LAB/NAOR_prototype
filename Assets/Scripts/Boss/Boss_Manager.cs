using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Manager : MonoBehaviour
{


    public int health = 500;

	public GameObject deathEffect;

	private bool isInvulnerable = false;

	[HideInInspector] public float attackTimer;
	public float autoAttackTimer;

	public int attackDamage = 35;
	public int enragedAttackDamage = 40;

	public Vector3 attackOffset;
	public float attackRange = 1f;
	public LayerMask attackMask;

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
			colInfo.GetComponent<PlayerManager>().DamagePlayer(attackDamage);
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
			anim.Play("Miniboss_hit");
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
