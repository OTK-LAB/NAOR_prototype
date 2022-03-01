using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public int lives = 2;
    public float MaxHealth = 100;
    public float CurrentHealth;
    public bool dead=false;
    public LevelManager levelManager; 

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        levelManager = FindObjectOfType<LevelManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetDamage(float damage)
    {
        if ((CurrentHealth - damage) >= 0)
        {
            CurrentHealth -= damage;
        }
        else
        {
            CurrentHealth = 0;
        }
        AmIDead();
    }

    void AmIDead()
    {
        if (CurrentHealth <= 0)
        {
            lives--;
            if(lives == 1)
            {
                levelManager.DeathDefiance();
                CurrentHealth = (MaxHealth * 40) / 100;
            }
            if (lives == 0)
            {
                levelManager.RespawnPlayer();
                CurrentHealth = MaxHealth;
                lives = 2;
            }

        }
    }

}
