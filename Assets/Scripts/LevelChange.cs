using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public PlayerController player;        //gotta add the player     there might be a better way
    public float timer;
    public float stayInZone = 2f;

    public GameObject zoneCam;
    public GameObject nextZoneCam;

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

    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerinside = true;

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

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = stayInZone;
            playerinside = false;
            zoneCam.SetActive(false);
            nextZoneCam.SetActive(true);
            StartCoroutine(ExitTeleport());
        }

    }

    IEnumerator ExitTeleport()
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
        yield return new WaitForSeconds(1f);
        player.disabled = false;
    }
}
