using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMenuScript : MonoBehaviour
{
    public GameObject CheckPointMenu;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            OpenCheckPointMenu();
        }
    }
    void OpenCheckPointMenu()
    {
        CheckPointMenu.SetActive(true);
        TogglePause();
    }

    public void CloseCheckPointMenu()
    {
        TogglePause();
        CheckPointMenu.SetActive(false);  
    }
    public void TogglePause()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        else if(Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
    }
}
