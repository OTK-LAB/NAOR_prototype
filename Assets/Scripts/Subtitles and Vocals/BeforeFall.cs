using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeforeFall : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitleText = default;
    // Start is called before the first frame update
    public static BeforeFall instance;
    private void Awake()
    {
        instance = this;
        clear();
    }
    public void SetSubtitle(string subtitle, float delay)
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
        yield return new WaitForSeconds(88);
        clear();
    }
    IEnumerator thesequence()
    {
        yield return new WaitForSeconds(0);
        subtitleText.text = "Do you get hurt kid?";
        yield return new WaitForSeconds(2);
        subtitleText.text = "Are you feeling bad,";
        yield return new WaitForSeconds(2);
        subtitleText.text = "you seem okay to me.";
        yield return new WaitForSeconds(2);
        subtitleText.text = "You have no idea who's city you are set foot on";
        yield return new WaitForSeconds(4);
        subtitleText.text = "do you?";
        yield return new WaitForSeconds(1);
        subtitleText.text = "The city of an Aspect,";
        yield return new WaitForSeconds(2);
        subtitleText.text = "the city of Kaizer,";
        yield return new WaitForSeconds(2);
        subtitleText.text = "with all the knowledge,";
        yield return new WaitForSeconds(2);
        subtitleText.text = "experience,";
        yield return new WaitForSeconds(1);
        subtitleText.text = "and power that only the Cage Maker remembers";
        yield return new WaitForSeconds(4);
        subtitleText.text = "from years gone by.";
        yield return new WaitForSeconds(2);
        subtitleText.text = "Don't be misled by the fact that";
        yield return new WaitForSeconds(2);
        subtitleText.text = "it is ruled by a khan";
        yield return new WaitForSeconds(2);
        subtitleText.text = "that does not suit such a magnificent city.";
        yield return new WaitForSeconds(4);
        subtitleText.text = "You are now in a place";
        yield return new WaitForSeconds(2);
        subtitleText.text = "where pain will follow you ";
        yield return new WaitForSeconds(2);
        subtitleText.text = "like a shadow on the streets.";
        yield return new WaitForSeconds(3);
        subtitleText.text = "I'll talk about the ruler later,";
        yield return new WaitForSeconds(3);
        subtitleText.text = "but you should know that ";
        yield return new WaitForSeconds(2);
        subtitleText.text = "he is a khan who owes all his power to the Kaizer,";
        yield return new WaitForSeconds(5);
        subtitleText.text = "by the way";
        yield return new WaitForSeconds(1);
        subtitleText.text = "do you afraid of height?";
        yield return new WaitForSeconds(3);
        subtitleText.text = "";
    }
    
      
    
     
}
