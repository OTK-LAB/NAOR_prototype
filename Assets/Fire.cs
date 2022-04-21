using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage = 5;
    public GameObject fire;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {         
         if (collision.CompareTag("Player"))
         {                        
                collision.transform.SendMessage("DamagePlayer", damage);         
         }        
    }
    
}
