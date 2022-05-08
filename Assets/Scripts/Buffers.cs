using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffers : MonoBehaviour
{
   
   private int live;
   private float health;
   
   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        live = PlayerManager.instance.lives;
        health = PlayerManager.instance.CurrentHealth;
        health /= 33.1f;
        var rakam = (int)Mathf.FloorToInt(health);
        Debug.Log(rakam);
    }
    /*if(lives==4){
        switch(rakam){
            case 0:
            
            case 1:

            case 2:

            case 3:

        }
    }*/


}

