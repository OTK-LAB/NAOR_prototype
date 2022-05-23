using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointText : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckPoint_1"))
        {
            //StartCoroutine(thesequence());
            textbox.GetComponent<TextMeshProUGUI>().text = "Press c button";
        }
    }
    /*IEnumerator thesequence()
    {
        yield return new WaitForSeconds(2);
        
        
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckPoint_1"))
        {
            //StartCoroutine(thesequence());
            textbox.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
