using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public PlatformEffector2D effector;
    public float doubleTapTime;
    public bool FirstTap;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !FirstTap)
        {
            FirstTap = true;
            doubleTapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.S) && FirstTap)
        {
            if (Time.time - doubleTapTime < 0.4f)
            {
                effector.rotationalOffset = 180f;
                doubleTapTime = 0f;
            }
            FirstTap = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            effector.rotationalOffset = 0;
        }
    }

 
}
