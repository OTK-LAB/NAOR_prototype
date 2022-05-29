using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiseumKey : MonoBehaviour
{
    public GameObject key;

    private void Start() {
        key.SetActive(false);
    }
    private void Update() {
        if(gameObject.GetComponent<Collesium>().AreEnemiesDied())
        {
            key.SetActive(true);
        }
    }
}
