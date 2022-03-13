using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villegardestroyerdoor : MonoBehaviour
{
    private bool doorDetected;

    public float width;
    public float height;

    public LayerMask WhatIsEnemy;
    public GameObject DestroyEnemy;

    public float DestroyTime;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Door();
    }

    void Door()
    {
        doorDetected = Physics2D.OverlapBox(gameObject.transform.position, new Vector2(width, height), 0, WhatIsEnemy);

        if (doorDetected == true)
        {
            Destroy(DestroyEnemy);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(width, height, 1));
        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(gameObject.transform.position, new Vector3(width, height, 1));
    }

}
