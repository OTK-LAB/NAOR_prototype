using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DrillSkill
{
    public GameObject playerObject;
    public float skillRange;
    private int skillDamage;
    List<RaycastHit2D> hits;

    int GetSkillDamage()
    {
        return skillDamage;
    }

    void UseSkill()
    { //LAYERMASK EKLENECEK
        if(playerObject.GetComponent<PlayerController>().facingRight)
        {
            hits = Physics2D.RaycastAll(playerObject.transform.position, playerObject.transform.right, skillRange).ToList();
        }
        if(!playerObject.GetComponent<PlayerController>().facingRight)
        {
            hits = Physics2D.RaycastAll(playerObject.transform.position, -playerObject.transform.right, skillRange).ToList();
        }
        //DEGEN HER ENEMY ICIN GETDAMAGE() FONKSIYONUNU CALISTIR
        //HER DUSMANIN SCRIPTI KENDINE OZEL OLDUGU ICIN SORUN OLABILIR BURASI
        foreach(RaycastHit2D hit in hits)
        {
            hit.collider.GetComponent<Legolas>().TakeDamage(20);
        }
    }
}
