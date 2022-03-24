using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerHealthManager : MonoBehaviour
{

    public Animator animator;
    
    public float maxHealth = 100;
    [SerializeField]
    float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void TakeDamage(int damage)
    {

        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<VillagerRunning>().enabled = false;
        GetComponent<VillagerHealthManager>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        
    }
}
