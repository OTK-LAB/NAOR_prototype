using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public PlayerController player;        //gotta add the player     there might be a better way
    public float timer;
    public float stayInZone = 2f;

    public float corX, corY;

    bool playerinside;

    private void Start()
    {
        timer = stayInZone;
    }
    // Update is called once per frame
    void Update()
    {
        if (timer > 0 && playerinside)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && playerinside)
        {
            player.transform.position = new Vector2(corX, corY);
        }

        if (timer <= stayInZone / 2 && playerinside)
        {
            player.disabled = true;
            if (player.facingRight)
            {
                player.xAxis = 1;
            }
            else
            {
                player.xAxis = -1;
            }

        }
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerinside = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = stayInZone;
            player.disabled = false;
            playerinside = false;
        }

    }
}
