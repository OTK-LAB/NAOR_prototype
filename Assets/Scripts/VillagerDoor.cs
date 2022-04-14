using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class VillagerDoor : MonoBehaviour
{
    //isTrigger necessary
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Villager"))
            Destroy(other.gameObject);
    }
}
