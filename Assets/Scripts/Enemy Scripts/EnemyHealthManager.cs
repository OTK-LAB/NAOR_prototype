using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public class Enemy
    {
        public float MaxHealth;
        public float CurrentHealth;
        public float Damage;
        public float Regen = 1;

        bool hurt = false;
        bool alive = true;

        Rigidbody rgb;
        


        public void TakeDamage(float Damage)
        {
            CurrentHealth -= Damage;
            if (CurrentHealth <= 0)
            {
                alive = false;
                hurt = false;
                
            }
            else
            {
                hurt = true;
            }
        }

        public void regen()
        {
            if (CurrentHealth < MaxHealth)
                CurrentHealth += Regen * Time.deltaTime;
        }


        /*public Enemy (float GivenMaxHealth , Rigidbody Givenrgb )
        {
            MaxHealth = GivenMaxHealth;
            rgb = Givenrgb;
        }

        public Enemy(string x)
        {
            
            switch (x)
            {
                case minion:
                    MaxHealth = 1;
                    

                case minion2:
                    MaxHealth = 1;

                case preacher:
                    MaxHealth = 1;

                case legolas:
                    MaxHealth = 1;

                case þovalye:
                    MaxHealth = 1;

                case ultimateþovalye:
                    MaxHealth = 1;

                case villager:
                    MaxHealth = 1;

                case mercenarie:
                    MaxHealth = 1;
            }
            

        }*/


    }


}
