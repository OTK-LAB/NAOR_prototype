using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAndSkillManager {
    //LEVEL SYSTEM START//
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    

    private static readonly int[] experiencePerLevel = new[] { 100, 120, 140, 160, 180, 200, 220, 250, 300, 400 };
    private int level;
    private int experience;

    public void AddExperience(int amount) {
        if (!IsMaxLevel()) {
            experience += amount;
            while (!IsMaxLevel() && experience >= GetExperienceToNextLevel(level)) {
                // Enough experience to level up
                experience -= GetExperienceToNextLevel(level);
                level++;
                AddSkillPoint();
                
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }
            if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        }
    }

    

    public float GetExperienceNormalized() {
        if (IsMaxLevel()) {
            return 1f;
        } else {
            return (float)experience / GetExperienceToNextLevel(level);
        }
    }

    

    public int GetExperienceToNextLevel(int level) {
        if (level < experiencePerLevel.Length) {
            return experiencePerLevel[level];
        } else {
            // Level Invalid
            Debug.LogError("Level invalid: " + level);
            return 100;
        }
    }

    public bool IsMaxLevel() {
        return IsMaxLevel(level);
    }

    public bool IsMaxLevel(int level) {
        return level == experiencePerLevel.Length - 1;
    }
    //LEVEL SYSTEM ENDS

    //SKILL SYSTEM STARTS


    public event EventHandler OnSkillPointChanged;
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }
    private int skillPoints;
    public enum SkillType
    {
        None,
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

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            skillPoints--;
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
        }
       
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    
    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.skillC: return SkillType.skillB;
        }
        return SkillType.None;
    }
    public bool TryUnlockSkill(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if(skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                if (skillPoints > 0)
                {
                    UnlockSkill(skillType);
                    Debug.Log("sonuc SKILL ACILDI ONCUL SKILL ACIK");
                    return true;
                }
                else
                {
                    Debug.Log("sonuc SKILL ACILMADI ONCUL SKILL ACIK AMA SKILLSPOINT YETERSIZ");
                    return false;
                }
            }
            else
            {
                Debug.Log("sonuc ONCUL SKILL KAPALI");
                return false;
            }
        }
        else
        {
            if(skillPoints > 0)
            {
                UnlockSkill(skillType);
                Debug.Log("sonuc SKILL ACILDI ONCUL SKILL YOK");
                return true;
            }
            else
            {
                Debug.Log("sonuc SKILL ACILMADI ONCUL SKILL YOK SKILLPOINT YETERSIZ");
                return false;
            }
        }
        /*
        if (CanUnlock(skillType))
        {
            Debug.Log("AÇABÝLÝYOR");
            if (skillPoints > 0)
            {
                Debug.Log("AÇABÝLÝYOR SKILL POINT VAR");
                skillPoints--;
                OnSkillPointChanged?.Invoke(this, EventArgs.Empty);
                UnlockSkill(skillType);
                return true;
            }
            else
            {
                Debug.Log("AÇABÝLÝYOR SKILl POINT YOK");
                return false;
            }
        }
        else
        {
            Debug.Log("AÇAMIYOR");
            return false;
        }
        */
    }

 
    //SKILL SYSTEM ENDS

    //CONSTRUCTOR
    public LevelAndSkillManager()
    {
        level = 0;
        experience = 0;
        skillPoints = 0;
        unlockedSkillTypeList = new List<SkillType>();
    }
    //CONSTRUCTOR


    //GETTER-SETTER
    public int GetSkillpoint()
    {
        return skillPoints;
    }
    public int GetExperience()
    {
        return experience;
    }
    public int GetLevelNumber()
    {
        return level;
    }
    //GETTER-SETTER
}
