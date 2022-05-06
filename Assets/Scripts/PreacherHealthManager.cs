using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreacherHealthManager : MonoBehaviour
{
    Animator animator;
    string currentState;
    const string hurt = "PreacherHurtAnimation";
    const string death = "PreacherDeathAnimation";
    public float maxHealth = 100;
    [SerializeField]
    float currentHealth;
    [HideInInspector] public bool isHurting;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        ChangeAnimationState(hurt);
        isHurting = true;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<PreacherRunning>().enabled = false;
        GetComponent<PreacherHealthManager>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
