using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    public GameObject IntroductionMenu;
    public GameObject MainMenu;
    public GameObject Menu;
    public GameObject StartMenu;
    public GameObject ContinueMenu;
    public GameObject SettingsMenu;
    public GameObject ExitMenu;
    public GameObject QuoteText;
    int RandNum;
    public void Start()
    {
        GenerateQuote();
    }


    public void Update()
    {
        if (Input.anyKeyDown && IntroductionMenu.activeInHierarchy)
        {
            IntroductionMenu.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    public void StartScreen()
    {
        Menu.SetActive(false);
        StartMenu.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads the next scene in line
    }

    public void Continue()
    {
        ContinueMenu.SetActive(true);
        Menu.SetActive(false);
    }
    public void Settings()
    {
        SettingsMenu.SetActive(true);
        Menu.SetActive(false);
    }

    public void ExitScreen()
    {
        ExitMenu.SetActive(true);
        Menu.SetActive(false);
    }
    public void EnlargeButton(Button button)
    {
        RectTransform rt = button.GetComponent<RectTransform>();
        float y = rt.offsetMax.y;
        rt.offsetMax = new Vector2(240f, y);
    }

    public void ResetButton(Button button)
    {
        RectTransform rt = button.GetComponent<RectTransform>();
        float y = rt.offsetMax.y;
        rt.offsetMax = new Vector2(160f, y);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GenerateQuote()
    {
        RandNum = Random.Range(0, 3);
        string[] Quotes = { "Face the bars.Solve the lock. Defeat the cage.\n\tRun to hands of freedom.(laughs)\nKahve",
                            "But we are different kid. We are nnot fighting with cages.\n\tOur revenge oath is to destroy the cagemaker.\nKahve",
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit,\n\tsed do eiusmod tempor incididunt ut labore et Kahve" };
        QuoteText.GetComponent<TMPro.TextMeshProUGUI>().text = Quotes[RandNum];
    }
}
