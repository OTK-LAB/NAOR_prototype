using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DrillSkill
{
    public GameObject playerObject;
    public float skillRange;
    private float skillDamage;
    public LayerMask enemyLayer;
    List<RaycastHit2D> hits;

    public DrillSkill(GameObject obj,float range, float damage, LayerMask layer){
        playerObject = obj;
        skillRange = range;
        skillDamage = damage;
        enemyLayer = layer;
    }
    float GetSkillDamage()
    {
        return skillDamage;
    }

    public void UseSkill()
    { //LAYERMASK EKLENECEK
        if(playerObject.GetComponent<PlayerController>().facingRight)
        {
            hits = Physics2D.RaycastAll(playerObject.transform.position, playerObject.transform.right, skillRange, enemyLayer).ToList();
        }
        if(!playerObject.GetComponent<PlayerController>().facingRight)
        {
            hits = Physics2D.RaycastAll(playerObject.transform.position, -playerObject.transform.right, skillRange, enemyLayer).ToList();
        }
        //DEGEN HER ENEMY ICIN GETDAMAGE() FONKSIYONUNU CALISTIR
        //HER DUSMANIN SCRIPTI KENDINE OZEL OLDUGU ICIN SORUN OLABILIR BURASI
        foreach(RaycastHit2D hit in hits)
        {   
            if(hit.collider.CompareTag("Legolas"))
            {
                hit.collider.GetComponent<Legolas>().TakeDamage(20);
                Debug.Log(hit.collider.GetComponent<Legolas>().currentHealth);
            }
        }
    }
}
