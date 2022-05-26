using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour

{
    public int maxHealth = 100;
    public float smoothing = 5;
    
    public HealthBar healthBar;
    public Slider slider;
    
    public static HealthBar instance;

   


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {  
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (PlayerManager.instance.CurrentHealth != slider.value)
        {
            slider.value = Mathf.Lerp(slider.value, PlayerManager.instance.CurrentHealth, smoothing * Time.deltaTime);
        }       
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

}