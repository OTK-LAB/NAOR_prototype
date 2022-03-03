using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public GameObject IntroductionMenu;
    public GameObject MainMenu;

    public void Start()
    {
        
    }


    public void Update()
    {
        if (Input.anyKeyDown && IntroductionMenu.activeInHierarchy)
        {
            IntroductionMenu.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads the next scene in line
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
