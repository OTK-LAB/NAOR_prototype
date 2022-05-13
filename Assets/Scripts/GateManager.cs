using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public HealthGateListSO healthGateListSo;
    [SerializeField]private PlayerManager _playerManager;
    [SerializeField]private PlayerController _playerController;
    private float percentage;

    private void OnEnable()
    {
        Actions.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        Actions.OnHealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        ResetBuffs();
        OnHealthChanged();
    }

    private void OnHealthChanged()
    {
        
        percentage = _playerManager.CurrentHealth*100 / _playerManager.MaxHealth;
        Debug.Log("%" +percentage);
        
        if (percentage>= healthGateListSo.HealthGateList[3].percentage)
        {
            ResetBuffs();
            foreach (var gems in  healthGateListSo.HealthGateList[3].activeGems)
            {
                if (gems != null)
                {
                    GiveBuff(gems,gems.gemtype); 
                }
                
            }
            
        }
        else if (percentage>= healthGateListSo.HealthGateList[2].percentage)
        {
            ResetBuffs();
            foreach (var gems in  healthGateListSo.HealthGateList[2].activeGems)
            {
                if (gems != null)
                {
                    GiveBuff(gems,gems.gemtype); 
                }
            }
        }
        else if (percentage>= healthGateListSo.HealthGateList[1].percentage)
        {
            ResetBuffs();
            foreach (var gems in  healthGateListSo.HealthGateList[1].activeGems)
            {
                if (gems != null)
                {
                    GiveBuff(gems,gems.gemtype); 
                }
            }
        }
        else if (percentage>= healthGateListSo.HealthGateList[0].percentage)
        {
            ResetBuffs();
            foreach (var gems in  healthGateListSo.HealthGateList[0].activeGems)
            {
                if (gems != null)
                {
                    GiveBuff(gems,gems.gemtype); 
                }
            }
        }
    }
    private void GiveBuff(GemSO gem ,GemSO.Gemtype gemtype)
    {
        switch (gemtype)
        {
            case GemSO.Gemtype.AttackBuff:
                Debug.Log("eski attack degeri " +_playerController.attackDamage);
                _playerController.attackDamage += _playerController.attackDamage * gem.buffRate / 100;
                Debug.Log(gem.buffRate + "Attack buff alindi");
                Debug.Log("yeni attack degeri " +_playerController.attackDamage);
                break;
            case GemSO.Gemtype.SpeedBuff:
                Debug.Log("eski runspeed degeri " +_playerController.runSpeed);
                Debug.Log("eski walkspeed degeri " +_playerController.walkSpeed);
                _playerController.runSpeed +=_playerController.runSpeed * gem.buffRate / 100;
                _playerController.walkSpeed +=_playerController.walkSpeed * gem.buffRate / 100;
                
                Debug.Log(gem.buffRate + "speed buff alindi");
                Debug.Log("yeni runspeed degeri " +_playerController.runSpeed);
                Debug.Log("yeni walkspeed degeri " +_playerController.walkSpeed);
                break;
        }
    }

    private void ResetBuffs()
    {
        _playerController.attackDamage = 10;
        _playerController.runSpeed = 4;
        _playerController.walkSpeed = 1.4f;
    }
}
