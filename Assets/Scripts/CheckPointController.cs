using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public bool checkpointReached;

    //Animations
    private Animator animator;
    private string currentState;
    const string idle = "CheckPoint";
    const string activated = "CheckPointActivated";
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAnimations();
    }

    void ChangeAnimations()
    {

        if (checkpointReached == true)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeAnimationState(activated);
            }

        }
        /*else if (checkpointReached == false)
        {
            ChangeAnimationState(idle);
        }*/
    }


    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    /* void OnTriggerEnter2D(Collider2D other)
     {
         if (other.tag == "Player")
         {
            checkpointReached = false;
         }
     }*/

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointReached = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointReached = false ;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" )
        {
            
            levelManager.currentCheckPoint = gameObject; 
        }
    }


}
