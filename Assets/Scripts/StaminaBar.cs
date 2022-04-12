using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private int maxStamina = 100;
    private float currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public float smoothing = 5;
    public float staminaIncreasingSpeed = 1;

    public static StaminaBar instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }


    public void useStamina(int amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            //  staminaBar.value = currentStamina;

            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("Not enough stamina");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentStamina != staminaBar.value)
        {
            staminaBar.value = Mathf.Lerp(staminaBar.value, currentStamina, smoothing * Time.deltaTime);
        }
        //stamina kullanma fonksiyonu
        //baþka scriptte çaðýrmak için 
        //StaminaBar.instance.useStamina(12);
        if (Input.GetKeyDown(KeyCode.F))
        {
            useStamina(15);
        }

    }


    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1);

        while (currentStamina < maxStamina)
        {
            currentStamina += staminaIncreasingSpeed * (maxStamina / 100);
            // staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }
}
