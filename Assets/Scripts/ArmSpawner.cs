using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSpawner : MonoBehaviour
{
    [SerializeField] GameObject arm;
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            arm.SetActive(true);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("Despawn");
            StartCoroutine(WaitForAnim());
        }
 
    }
    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(.4f);
        arm.SetActive(false);
    }
}
