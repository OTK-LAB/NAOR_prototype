using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    private SpringJoint2D spring;
    

    void Start()
    {
        spring = GetComponent<SpringJoint2D>();
        spring.enabled = false;
        GameObject backpack = GameObject.FindWithTag("Backpack");
        spring.connectedBody = backpack.GetComponent<Rigidbody2D>();


    }
    private void OnTriggerEnter2D(Collider2D collision)

    {
       
        if (collision.gameObject.tag == "Player") { 
        spring.enabled = true;
    }
    }
}