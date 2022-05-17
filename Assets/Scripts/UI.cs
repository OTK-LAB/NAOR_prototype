using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitleText = default;
    // Start is called before the first frame update
    public static UI instance;
    private void Awake()
    {
        instance = this;
        clear();
    }
    public void SetSubtitle(string subtitle,float delay)
    {
        //subtitleText.text = subtitle;
        StartCoroutine(thesequence());
        StartCoroutine(ClearAfterSecond(20));
    }
    public void clear()
    {
        subtitleText.text = "";
    }
    private IEnumerator ClearAfterSecond(float delay)
    {
        yield return new WaitForSeconds(21);
        clear();
    }
    IEnumerator thesequence()
    {
        yield return new WaitForSeconds(0);
        subtitleText.text = "Kaizer: Yes, I can feel you boy.";
        yield return new WaitForSeconds(4);
        subtitleText.text = "You're the one coming out of the Maze.";
        yield return new WaitForSeconds(3);
        subtitleText.text = "How a pity child can find a way to break our Realm I wonder?";
        yield return new WaitForSeconds(5);
        subtitleText.text = "";
        yield return new WaitForSeconds(1);
        subtitleText.text = "Maybe he is not a regular soul,";
        yield return new WaitForSeconds(3);
        subtitleText.text = "maybe he is the new one. ";
        yield return new WaitForSeconds(2);
        subtitleText.text = "I KNOW WHO YOU ARE ";
        yield return new WaitForSeconds(3);
    }
}
