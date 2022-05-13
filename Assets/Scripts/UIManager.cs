using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public GemTypeListSO gemTypeListSO;
    public GateManager _gateManager;
    public Button CommonGemsUI;
    public Button RareGemsUI;
    public Button LegendaryGemsUI;
    
    public GameObject CommonSelectionUI;
    public GameObject RareSelectionUI;
    public GameObject LegendarySelectionUI;
    //public GameObject DescriptionUI;

    public TMP_Text RelicTitleText;
    public TMP_Text PowerText;
    public TMP_Text DescriptionText;

    public List<Button> CommonButtonList;

    private int buttonIndex;
    private int healthGateIndex;
    private GemSO selectedGem;

    public void OnCommonGemsButtonPressed()
    {
        CommonSelectionUI.SetActive(true);
        RareSelectionUI.SetActive(false);
        LegendarySelectionUI.SetActive(false);
        //DescriptionUI.SetActive(true);
    }
    public void OnRareGemsButtonPressed()
    {
        RareSelectionUI.SetActive(true);
        LegendarySelectionUI.SetActive(false);
        CommonSelectionUI.SetActive(false);
        //DescriptionUI.SetActive(true);
    }
    public void OnLegendaryGemsButtonPressed()
    {
        LegendarySelectionUI.SetActive(true);
        CommonSelectionUI.SetActive(false);
        RareSelectionUI.SetActive(false);
        //DescriptionUI.SetActive(true);
    }

    #region LegendaryButtons

    public void OnLegendaryGem1Pressed()
    {
        
        RelicTitleText.text =gemTypeListSO.gemTypeList[2].gemList[0].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[2].gemList[0].description;
        if ( _gateManager.healthGateListSo.HealthGateList[0].activeGems.Count==_gateManager.healthGateListSo.HealthGateList[0].maxGemCount)
        {
            _gateManager.healthGateListSo.HealthGateList[0].activeGems[0]=gemTypeListSO.gemTypeList[2].gemList[0];
            
        }
        else
        {
            _gateManager.healthGateListSo.HealthGateList[0].activeGems.Add(gemTypeListSO.gemTypeList[2].gemList[0]);
        }

    }
    public void OnLegendaryGem2Pressed()
    {
        RelicTitleText.text =gemTypeListSO.gemTypeList[2].gemList[1].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[2].gemList[1].description;
        if ( _gateManager.healthGateListSo.HealthGateList[0].activeGems.Count==_gateManager.healthGateListSo.HealthGateList[0].maxGemCount)
        {
            _gateManager.healthGateListSo.HealthGateList[0].activeGems[0]=gemTypeListSO.gemTypeList[2].gemList[1];
            
        }
        else
        {
            _gateManager.healthGateListSo.HealthGateList[0].activeGems.Add(gemTypeListSO.gemTypeList[2].gemList[1]);
        }
    }

    #endregion
    #region CommonButtons

    public void OnCommonGem1Pressed()
    {
        RelicTitleText.text =gemTypeListSO.gemTypeList[0].gemList[0].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[0].gemList[0].description;
        selectedGem=gemTypeListSO.gemTypeList[0].gemList[0];
        //_gateManager.healthGateListSo.HealthGateList[1].activeGems[index]=gemTypeListSO.gemTypeList[0].gemList[1];
        /*if ( _gateManager.healthGateListSo.HealthGateList[2].activeGems.Count==_gateManager.healthGateListSo.HealthGateList[2].maxGemCount)
        {
            _gateManager.healthGateListSo.HealthGateList[2].activeGems[0]=gemTypeListSO.gemTypeList[0].gemList[1];
            
        }
        else
        {
            _gateManager.healthGateListSo.HealthGateList[2].activeGems.Add(gemTypeListSO.gemTypeList[0].gemList[1]);
        }*/
    }
    public void OnCommonGem2Pressed()
    {
        RelicTitleText.text =gemTypeListSO.gemTypeList[0].gemList[1].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[0].gemList[1].description;
        selectedGem=gemTypeListSO.gemTypeList[0].gemList[1];
    }

    #endregion
    #region RareButtons

    public void OnRareGem1Pressed()
    {
        RelicTitleText.text =gemTypeListSO.gemTypeList[1].gemList[0].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[1].gemList[0].description;
    }
    public void OnRareGem2Pressed()
    {
        RelicTitleText.text =gemTypeListSO.gemTypeList[1].gemList[1].title;
        DescriptionText.text=gemTypeListSO.gemTypeList[1].gemList[1].description;
    }

    #endregion

    public void OnOKButtonPressed()
    {
        _gateManager.healthGateListSo.HealthGateList[healthGateIndex].activeGems[buttonIndex] = selectedGem;
    }

    public void OnCommonButtonPressed(Button button)
    {
        healthGateIndex = 1;
        foreach (var a in CommonButtonList)
        {
            if (a == button)
            {
                button.image.color = Color.yellow;
                buttonIndex = CommonButtonList.IndexOf(a);
               
            }
        }
    }
}
