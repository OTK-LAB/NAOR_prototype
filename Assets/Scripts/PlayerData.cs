using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int level;

    public float health;
    public float[] position;

    public PlayerData (PlayerManager playerManager)
    {
        
        if(health == 0)
        {
            health = 100;
        }
        health = playerManager.dataforhealth;

        position = new float[3];
        position[0] = playerManager.currentCheckPoint.transform.position.x + 1;
        position[1] = playerManager.currentCheckPoint.transform.position.y + (1.571746f);
        position[2] = playerManager.currentCheckPoint.transform.position.z;

    }

}
