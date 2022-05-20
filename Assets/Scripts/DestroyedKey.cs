using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.gameObject.tag == "Player") { 
            ScoreText.coinAmount += 1;
        Destroy(gameObject);
    }
    }
}
