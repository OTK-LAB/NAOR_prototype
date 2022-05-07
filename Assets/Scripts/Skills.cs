using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController playerController;
    PlayerManager playerManager;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerManager = GetComponent<PlayerManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CanUseSkillA()
    {
        return playerManager.levelSystem.IsSkillUnlocked(LevelAndSkillManager.SkillType.skillA);
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        playerController.runSpeed = movementSpeed;
    }

}
