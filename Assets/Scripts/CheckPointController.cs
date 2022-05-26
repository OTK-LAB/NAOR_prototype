using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public bool checkpointReached;
    private PlayerManager playerManager;
    private PlayerController playerController;
    private CheckPointMenuScript checkPointMenuScript;
    //Animations
    private Animator animator;
    private string currentState;
    const string idle = "CheckPoint";
    const string activated = "CheckPointActivated";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        checkPointMenuScript = GameObject.FindGameObjectWithTag("CheckPointMenuManager").GetComponent<CheckPointMenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAnimations();
        SetCheckpoint();
    }
    void ChangeAnimations()
    {

        if (checkpointReached)
        {
            if (Input.GetKeyDown(KeyCode.C) && !playerController.isGuarding)
            {
                ChangeAnimationState(activated);
            }

        }
    }


    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void SetCheckpoint()
    {
        if (checkpointReached)
        {
            if (Input.GetKeyDown(KeyCode.C) && !playerController.isGuarding)
            {
                playerManager.currentCheckPoint = gameObject;
                playerManager.lives=4;
                playerManager.CurrentHealth += 100;
                Actions.OnHealthChanged();
                Debug.Log("Checkpoint Degisti");
            }
        }
        if(!checkpointReached)
        {
            if(playerManager.currentCheckPoint != gameObject)
            {
                ChangeAnimationState(idle);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointReached = true;
            playerController.inCheckpointRange = true;
            checkPointMenuScript.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointReached = false ;
            playerController.inCheckpointRange = false;
            checkPointMenuScript.enabled = false;
        }
    }

}
