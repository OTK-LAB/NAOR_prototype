/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public class minion
    {
        public float MinionMaxHealth = 100;
        public float MinionCurrentHealth;
        public float MinionDamage = 15f;
        public float regeneration = 1;
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (MinionCurrentHealth <= 0)
            {
                alive = false;
                hurt = false;
                Die();
            }
            else
            {
                hurt = true;
                ChangeAnimations();
            }
        }
        public void regen (float MinionCurrentHealth) 
            {
            if (MinionCurrentHealth < MinionMaxHealth)
                MinionCurrentHealth += regeneration * Time.deltaTime;
        }
               
    }

    public class preacher
    {
        public float PreacherMaxHealth = 100;
        public float PreacherCurrentHealth;
        public float PreachorDamage = 15f;
        public float regeneration = 1;
        public void TakeDamage(float damage)
        {
            PreacherCurrentHealth -= damage;
            if (PreacherCurrentHealth <= 0)
            {
                alive = false;
                hurt = false;
                Die();
            }
            else
            {
                hurt = true;
                ChangeAnimations();
            }
        }
        public void regen(float PreacherCurrentHealth)
        {
            if (PreacherCurrentHealth < PreacherMaxHealth)
                PreacherCurrentHealth += regeneration * Time.deltaTime;
        }
}

    public class legolas
    {
        public float LegolasMaxHealth = 100;
        public float LegolasCurrentHealth;
        public float LegolasDamage = 15f;
        public float regeneration = 1;
        public void TakeDamage(float damage)
        {
            LegolasCurrentHealth -= damage;
            if (LegolasCurrentHealth <= 0)
            {
                alive = false;
                hurt = false;
                Die();
            }
            else
            {
                hurt = true;
                ChangeAnimations();
            }
        }
        public void regen(float LegolasCurrentHealth)
        {
            if (LegolasCurrentHealth < LegolasMaxHealth)
                LegolasCurrentHealth += regeneration * Time.deltaTime;
        }
}

    public class swordEnemy
    {
        public float SwordEnemyMaxHealth = 100;
        public float SwordEnemyCurrentHealth;
        public float SwordEnemyDamage = 15f;
        public float regeneration = 1;
        public void TakeDamage(float damage)
        {
            SwordEnemyCurrentHealth -= damage;
            if (SwordEnemyCurrentHealth <= 0)
            {
                alive = false;
                hurt = false;
                Die();
            }
            else
            {
                hurt = true;
                ChangeAnimations();
            }
        }
        public void regen(float SwordEnemyCurrentHealth)
        {
            if (SwordEnemyCurrentHealth < SwordEnemyMaxHealth)
                    SwordEnemyCurrentHealth += regeneration * Time.deltaTime;
        }
}

    public class villager
    {
        public float VillagerMaxHealth = 100;
        public float VillagerCurrentHealth;
        public float VillagerDamage = 15f;
        public float regeneration = 1;
        public void TakeDamage(float damage)
        {
            VillagerCurrentHealth -= damage;
            if (VillagerCurrentHealth <= 0)
            {
                alive = false;
                hurt = false;
                Die();
            }
            else
            {
                hurt = true;
                ChangeAnimations();
            }
        }
        public void regen(float VillagerCurrentHealth)
        {
            if (VillagerCurrentHealth < VillagerMaxHealth)
                    VillagerCurrentHealth += regeneration * Time.deltaTime;
        }
}


} */
