using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PrologueManager : MonoBehaviour
{
    
    int index;
    //public Transform[] allChildren;
    //List<GameObject> childObjects = new List<GameObject>();
    void Start()
    {
        index = 0;
        /*allChildren = GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allChildren)
        {
            childObjects.Add(child.gameObject);
        }*/

    }
  
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (index < 5)
            {
                
                transform.GetChild(index).gameObject.SetActive(false);
                index++;
                transform.GetChild(index).gameObject.SetActive(true);
                
            }
            else if (index == 5)
            {
                transform.GetChild(index + 1).gameObject.SetActive(true);
                index++;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads the next scene in line
            }
            
        }

        /* if (Input.anyKeyDown)
         {
             childObjects[index].active = false;
             if (index <allChildren.Length-1)
             {          
                     childObjects[index].SetActive(true);             
             }
         }
     }*/
    }

   
}
