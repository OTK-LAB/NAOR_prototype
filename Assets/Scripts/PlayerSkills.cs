using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills {

    public event EventHandler OnSkillPointChanged;
    private int skillPoints;
    public enum SkillType
    {
        skillA,
        skillB,
        skillC,
        skillD,
        skillE,
        skillF,

    }

    private List<SkillType> unlockedSkillTypeList;
   

    public void AddSkillPoint()
    {
        
        skillPoints++;
        OnSkillPointChanged?.Invoke(this, EventArgs.Empty);
        
    }

   

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypeList.Add(skillType);
            
        }
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public bool CanUnlock(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            if(skillPoints > 0)
            {
                skillPoints--;
                OnSkillPointChanged?.Invoke(this, EventArgs.Empty);
                UnlockSkill(skillType);
                return true;
            }
            else {
                return false; 
            }
        }
        else { 
            return false; 
        }
        
    }





}

