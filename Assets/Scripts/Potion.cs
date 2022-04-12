using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject[] potions;
    public int cure;
    public int maxCure = 3;

    public static Potion instance;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        cure = maxCure;
        potions[3].gameObject.SetActive(false);
    }

    void Update()
    {

        //checkpoint fonksiyonu
        //farklý scriptte çaðýrmak için
        //Potion.instance.CheckPoint();
        if (Input.GetKeyDown(KeyCode.K))
        {
            CheckPoint();            
        }
        //iksir ekleme fonksiyonu
        //farklý scriptte çaðýrmak için
        //Potion.instance.AddCure();
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddCure();
        }
        
        //iksir kullanma fonksiyonu
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Potion.instance.cure > 0)
            {
                PlayerManager.instance.HealthPotion(40);
                Potion.instance.UsePotions(1);
            }
            else
            {
                Debug.Log("potions are over");
            }
        }
    }

    public void UsePotions(int p)
    {
        if (cure >= 1)
        {
            cure -= p;
            potions[cure].gameObject.SetActive(false);
            if (cure < 1)
            {
                Debug.Log("potions are over");
            }
        }

    }

    public void CheckPoint()
    {
        cure = maxCure;
        potions[0].gameObject.SetActive(true);
        potions[1].gameObject.SetActive(true);
        potions[2].gameObject.SetActive(true);
        if (maxCure == 4)
        {
            potions[3].gameObject.SetActive(true);
        }

    }
    public void AddCure()
    {
        maxCure = 4;
        potions[3].gameObject.SetActive(true);
        if (cure != 3)
        {
            cure = maxCure;
            potions[0].gameObject.SetActive(true);
            potions[1].gameObject.SetActive(true);
            potions[2].gameObject.SetActive(true);
            potions[3].gameObject.SetActive(true);
        }
    }

}