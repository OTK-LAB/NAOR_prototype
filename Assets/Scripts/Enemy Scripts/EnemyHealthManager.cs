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


        public Enemy (float GivenMaxHealth , Rigidbody Givenrgb )
        {
            MaxHealth = GivenMaxHealth;
            rgb = Givenrgb;
        }



        
    }

   /* public class Minion : Enemy
    {       
        MaxHealth = 40;
        Regen = 1;
    }

    public class Minion2 : Enemy
    {
        MaxHealth = 20;
        Regen = 1;
    }

    public class preacher : Enemy
    {
        MaxHealth = 60;
        Regen = 1;
    }

    public class Legolas : Enemy
    {
        MaxHealth = 60;
        Regen = 1;
    }

    public class Sovalye : Enemy
    {
        MaxHealth = 110;
        Regen = 1;
    }

    public class UltimateSovalye : Enemy
    {
        MaxHealth = 350;
        Regen = 1;
    }

    public class Villager : Enemy
    {
        MaxHealth = 10;
        Regen = 1;
    }

    public class Mercenarie : Enemy
    {
        MaxHealth = 110;
        Regen = 1;
    }
*/

}
