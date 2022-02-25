using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dagger")]
    public Transform firePoint;
    public GameObject dagger;
    private Vector2 direction;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            Instantiate(dagger, firePoint.position, Quaternion.Euler(new Vector3(0,0,-90)));
        }
    }
}
