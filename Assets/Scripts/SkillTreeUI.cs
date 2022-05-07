using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    private LevelAndSkillManager levelSystem;
    private bool tryunlock;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("sonuc Current level: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().levelSystem.GetLevelNumber());
            Debug.Log("sonuc Current Exp: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().levelSystem.GetExperience());
            Debug.Log("sonuc Current Skillpoint: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().levelSystem.GetSkillpoint());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            tryunlock = levelSystem.TryUnlockSkill(LevelAndSkillManager.SkillType.skillA);
            
           
            
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            tryunlock = levelSystem.TryUnlockSkill(LevelAndSkillManager.SkillType.skillB);
            
            

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            tryunlock = levelSystem.TryUnlockSkill(LevelAndSkillManager.SkillType.skillC);
            
            

        }
    }

    public void SetPlayerSkills(LevelAndSkillManager levelSystem)
    {
        this.levelSystem = levelSystem;
    }
}
