using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public PlatformEffector2D effector;
    private GameObject PlatformX;
    public float timeOfFirstButton;
    public bool firstButtonPressed, reset;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && firstButtonPressed)
        {
            if (Time.time - timeOfFirstButton < 0.5f)
            {
                Debug.Log("DoubleClicked");
                effector.rotationalOffset = 180f;
                StartCoroutine(ChangePlatform());
                this.enabled = false;
            }
            else
            {
                Debug.Log("Too late");
            }

            reset = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && !firstButtonPressed)
        {
            firstButtonPressed = true;
            timeOfFirstButton = Time.time;
            StartCoroutine(ChangefirstButtonPressed());
        }

        if (reset)
        {
            firstButtonPressed = false;
            reset = false;
        }

    }

    IEnumerator ChangefirstButtonPressed()
    {
        yield return new WaitForSeconds(0.5f);
        firstButtonPressed = false;
    }

    IEnumerator ChangePlatform()
    {
        yield return new WaitForSeconds(0.30f);
        effector.rotationalOffset = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ALDI");
            PlatformX = other.gameObject;
            
        }
    }
}
