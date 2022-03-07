using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject currentCheckPoint;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeathDefiance()
    {
        player.transform.position = player.transform.position;
        player.GetComponent<PlayerController>().dead = false;
        Invoke("CancelDamageAble", 0f);
        Invoke("ActiveDamageAble", 3f);

    }

    public void RespawnPlayer()
    {
        player.transform.position = currentCheckPoint.transform.position;
        player.GetComponent<PlayerController>().dead = false;
        player.GetComponent<Rigidbody2D>().simulated = true;
        player.GetComponent<PlayerController>().enabled = true;


    }
    void CancelDamageAble()
    {
        player.GetComponent<PlayerController>().damageable  = false;
    }
    void ActiveDamageAble()
    {
        player.GetComponent<PlayerController>().damageable = true;
    }

}
