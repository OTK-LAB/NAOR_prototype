using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGap : MonoBehaviour
{

    private PlayerController player;
    private int random;
    private int AttackCount = 0;
    private int newCount;
    int damage = 3;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public Transform attackPoint;


    void Start()
    {
        player = GetComponent<PlayerController>();
        
    }

    
    void Update()
    {
        Soul_Gap();
    }


    public void Soul_Gap()
    {
        
        newCount = player.GetComponent<PlayerController>().attackCount;

        if(newCount != AttackCount)
        {
            random = Random.Range(1, 5);
            if (random == 1)
            {
                StartCoroutine(CoroutineAttack(1));
            }
            AttackCount = newCount;
            
        }

        
        
    }

    public void hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
                enemy.GetComponent<Minion_wfireball>().TakeDamage(damage);
            if (enemy.CompareTag("Villager"))
                enemy.GetComponent<VillagerHealthManager>().TakeDamage(damage);
            if (enemy.CompareTag("Sword"))
                enemy.GetComponent<Sword_Behaviour>().TakeDamage(damage);
            if (enemy.CompareTag("MinionwPoke"))
                enemy.GetComponent<Minion_wpoke>().TakeDamage(damage);
            if (enemy.CompareTag("Legolas"))
                enemy.GetComponent<Legolas>().TakeDamage(damage);
        }


        IEnumerator CoroutineAttack(float sure)
        {
            hit();
            yield return new WaitForSeconds(sure);
            hit();
            yield return new WaitForSeconds(sure);
            hit();
        }



    }
}
