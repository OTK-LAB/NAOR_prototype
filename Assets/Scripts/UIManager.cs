using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.ComponentModel.Design;

public class UIManager : MonoBehaviour
{
    public GemTypeListSO gemTypeListSO;
    public GateManager _gateManager;
    public Button CommonGemsUI;
    public Button RareGemsUI;
    public Button LegendaryGemsUI;
    public Button OKButton;

    public GameObject CommonSelectionUI;
    public GameObject RareSelectionUI;
    public GameObject LegendarySelectionUI;
    //public GameObject DescriptionUI;

    public TMP_Text RelicTitleText;
    public TMP_Text PowerText;
    public TMP_Text DescriptionText;
    
    
    
    #region ButtonLists
    public List<Button> CommonButtonList;//common gem slotlarinin listesi
    public List<Button> CommonGemsButtonList;//common gem butonlarinin listesi
    public List<Button> RareButtonList;
    public List<Button> RareGemsButtonList;
    public List<Button> LegendaryButtonList1per;
    public List<Button> LegendaryButtonList99per;
    public List<Button> LegendaryGemsButtonList;
    
    #endregion

    


    private int buttonIndex;// bastigimiz slot butonunun indexini tutan deger
    int gemButtonIndex; // tikladigimiz gem butonunun indexi
    private int healthGateIndex; // healthgate indexini tutar deger
    private GemSO selectedGem;  // tikladigimiz butona gore secili gemi tutan degisken

    

    public void OnCommonGemsButtonPressed()
    {
        CommonSelectionUI.SetActive(true);
        RareSelectionUI.SetActive(false);
        LegendarySelectionUI.SetActive(false);
        PowerText.text = "Common";
        PowerText.color = Color.blue;
        //DescriptionUI.SetActive(true);
    }
    public void OnRareGemsButtonPressed()
    {
        RareSelectionUI.SetActive(true);
        LegendarySelectionUI.SetActive(false);
        CommonSelectionUI.SetActive(false);
        PowerText.text = "Rare";
        PowerText.color = Color.magenta;
        //DescriptionUI.SetActive(true);
    }
    public void OnLegendaryGemsButtonPressed()
    {
        LegendarySelectionUI.SetActive(true);
        CommonSelectionUI.SetActive(false);
        RareSelectionUI.SetActive(false);
        PowerText.text = "Legendary";
        PowerText.color = Color.yellow;
        //DescriptionUI.SetActive(true);
    }
    
    public void OnOKButtonPressed()
    {
        
        if (_gateManager.healthGateListSo.HealthGateList[healthGateIndex].activeGems[buttonIndex] != null)
        {
            _gateManager.healthGateListSo.HealthGateList[healthGateIndex].activeGems[buttonIndex].isActive = false;
        }
        _gateManager.healthGateListSo.HealthGateList[healthGateIndex].activeGems[buttonIndex] = selectedGem;
        selectedGem.isActive = true;
    }

    #region CommonButtonFunctions
    public void OnCommonButtonPressed(Button button)
    {
        healthGateIndex = 1;
        if (!CommonSelectionUI.activeSelf)
        {
            OnCommonGemsButtonPressed();
        }
        foreach (var a in CommonButtonList)
        {
            if (a == button)
            {
                button.image.color = Color.yellow;
                buttonIndex = CommonButtonList.IndexOf(a);
               
            }
        }
    }// slotu secmemize yarayan fonksiyon
    public void OnCommonGemsPressed(Button button)
    {
        
        foreach (var a in CommonGemsButtonList)
        {
            if (a == button)
            {
                
                gemButtonIndex = CommonGemsButtonList.IndexOf(a);
               
            }
        }
        RelicTitleText.text =gemTypeListSO.gemTypeList[0].gemList[gemButtonIndex].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[0].gemList[gemButtonIndex].description;
        selectedGem=gemTypeListSO.gemTypeList[0].gemList[gemButtonIndex];
        if (selectedGem.isActive == true)
        {
            OKButton.gameObject.SetActive(false);
        }
        else
        {
            OKButton.gameObject.SetActive(true);
        }
        Debug.Log(selectedGem.name);
    }//gemlere tikladigimizda gerceklesen olaylar

    #endregion

    public void SlotsColors()
    {
        
        foreach (var healthGates in _gateManager.healthGateListSo.HealthGateList)
        {
            foreach (var activeGem in healthGates.activeGems)
            {
                if (activeGem != null)
                {
                   
                    switch (healthGates.percentage)
                    {
                        case 1:
                            LegendaryButtonList1per[healthGates.activeGems.IndexOf(activeGem)].image.color =
                                Color.green;
                            break;
                        case 33:
                            CommonButtonList[healthGates.activeGems.IndexOf(activeGem)].image.color =
                                Color.green;
                            break;
                        case 66:
                            RareButtonList[healthGates.activeGems.IndexOf(activeGem)].image.color =
                                Color.green;
                            break;
                        case 99:
                            LegendaryButtonList99per[healthGates.activeGems.IndexOf(activeGem)].image.color =
                                Color.green;
                            break;
                    }
                   
                }
            }
        }
        IsGemsActive();
    }

    public void IsGemsActive()
    {
        foreach (var gems in gemTypeListSO.gemTypeList)
        {
            foreach (var gemss in gems.gemList)
            {
                if (gemTypeListSO.gemTypeList.IndexOf(gems)== 0 )
                {
                    if (gemss.isActive == true)
                    {
                        Color color =CommonGemsButtonList[gems.gemList.IndexOf(gemss)].image.color ;
                        color.a = 0.5f;
                        CommonGemsButtonList[gems.gemList.IndexOf(gemss)].image.color = color;
                    }
                    else
                    {
                        Color color =CommonGemsButtonList[gems.gemList.IndexOf(gemss)].image.color ;
                        color.a = 1f;
                        CommonGemsButtonList[gems.gemList.IndexOf(gemss)].image.color = color;
                    }
                    
                }
                else if (gemTypeListSO.gemTypeList.IndexOf(gems)== 1 )
                {
                    if (gemss.isActive == true)
                    {
                        Color color =RareGemsButtonList[gems.gemList.IndexOf(gemss)].image.color ;
                        color.a = 0.5f;
                        RareGemsButtonList[gems.gemList.IndexOf(gemss)].image.color = color; 
                    }
                    else
                    {
                        Color color =RareGemsButtonList[gems.gemList.IndexOf(gemss)].image.color ;
                        color.a = 1f;
                        RareGemsButtonList[gems.gemList.IndexOf(gemss)].image.color = color;
                    }
                    
                }
                else if (gemTypeListSO.gemTypeList.IndexOf(gems)== 2 )
                {
                    if (gemss.isActive == true)
                    {
                        Color color =LegendaryGemsButtonList[gems.gemList.IndexOf(gemss)].image.color ;
                        color.a = 0.5f;
                        LegendaryGemsButtonList[gems.gemList.IndexOf(gemss)].image.color = color;   
                    }
                    else
                    {
                        Color color =LegendaryGemsButtonList[gems.gemList.IndexOf(gemss)].image.color ;
                        color.a = 1f;
                        LegendaryGemsButtonList[gems.gemList.IndexOf(gemss)].image.color = color;
                    }
                    
                }
            }
        }
    }
    #region RareButtonFunctions

    public void OnRareButtonPressed(Button button)
    {
        healthGateIndex = 2;
        if (!RareSelectionUI.activeSelf)
        {
            OnRareGemsButtonPressed();
        }
        foreach (var a in RareButtonList)
        {
            if (a == button)
            {
                button.image.color = Color.yellow;
                buttonIndex = RareButtonList.IndexOf(a);
               
            }
        }
    }// slotu secmemize yarayan fonksiyon
    public void OnRareGemsPressed(Button button)
    {
        
        foreach (var a in RareGemsButtonList)
        {
            if (a == button)
            {
                
                gemButtonIndex = RareGemsButtonList.IndexOf(a);
               
            }
        }
        RelicTitleText.text =gemTypeListSO.gemTypeList[1].gemList[gemButtonIndex].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[1].gemList[gemButtonIndex].description;
        selectedGem=gemTypeListSO.gemTypeList[1].gemList[gemButtonIndex];
        if (selectedGem.isActive == true)
        {
            OKButton.gameObject.SetActive(false);
        }
        else
        {
            OKButton.gameObject.SetActive(true);
        }
        Debug.Log(selectedGem.name);
    }//gemlere tikladigimizda gerceklesen olaylar

    #endregion

    #region LegendaryButtonFunctions
    public void OnLegendaryButtonPressed1per(Button button)
    {
        healthGateIndex = 0;
        if (!LegendarySelectionUI.activeSelf)
        {
            OnLegendaryGemsButtonPressed();
        }
        foreach (var a in LegendaryButtonList1per)
        {
            if (a == button)
            {
                button.image.color = Color.yellow;
                buttonIndex = LegendaryButtonList1per.IndexOf(a);
               
            }
        }
    }// %1 lik slotu secmemize yarayan fonksiyon
    
    public void OnLegendaryButtonPressed99per(Button button)
    {
        healthGateIndex = 3;
        if (!LegendarySelectionUI.activeSelf)
        {
            OnLegendaryGemsButtonPressed();
        }
        foreach (var a in LegendaryButtonList99per)
        {
            if (a == button)
            {
                button.image.color = Color.yellow;
                buttonIndex = LegendaryButtonList99per.IndexOf(a);
               
            }
        }
    }// %99 luk slotu secmemize yarayan fonksiyon
    public void OnLegendaryGemsPressed(Button button)
    {
        
        foreach (var a in LegendaryGemsButtonList)
        {
            if (a == button)
            {
                
                gemButtonIndex = LegendaryGemsButtonList.IndexOf(a);
               
            }
        }
        RelicTitleText.text =gemTypeListSO.gemTypeList[2].gemList[gemButtonIndex].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[2].gemList[gemButtonIndex].description;
        selectedGem=gemTypeListSO.gemTypeList[2].gemList[gemButtonIndex];
        if (selectedGem.isActive == true)
        {
            OKButton.gameObject.SetActive(false);
        }
        else
        {
            OKButton.gameObject.SetActive(true);
        }
        Debug.Log(selectedGem.name);
    }//gemlere tikladigimizda gerceklesen olaylar
    

    #endregion
    
}
