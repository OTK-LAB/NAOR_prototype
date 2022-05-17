using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";

	// Use this for initialization
	void OnEnable() {
		fullText = this.GetComponent<TextMeshProUGUI>().text;

		StartCoroutine(ShowText());
	}
	
	IEnumerator ShowText(){
		for(int i = 0; i < fullText.Length; i++){
			while(fullText[i]==' ')
            {
				i++;
            }
			currentText = fullText.Substring(0,i);
			this.GetComponent<TextMeshProUGUI>().text = currentText;
			yield return new WaitForSeconds(delay);
		}
	}
}
