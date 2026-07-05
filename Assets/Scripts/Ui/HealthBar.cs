using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //THIS SCRIPT IS FOR HEALTHBAR LOGIC//

    [Header("Variables")]
    [SerializeField] private float MaxHealth = 100;
    [SerializeField] private float CurrentHealth;


    [Header("Ui Info")]
    public Slider HealthSlider;

   

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SETS CURRENTHEALTH TO BE MAXHEALTH AT START OF THE GAME//
        CurrentHealth = MaxHealth;

        //SETS THE MAX VALUE OF THE SLIFER TO BE THE MAXHEALTH OF 100//
        HealthSlider.maxValue = MaxHealth;

        //SETS THE VALUE TO BE CURRENTHEALTH//
        HealthSlider.value = CurrentHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //FUNCTION TO DECREASE HEALTHBAR HEALTH//
    public void DecreaseHealthVisually(float DamageAmount)
    {
        
        CurrentHealth -= DamageAmount;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
       
        HealthSlider.value = CurrentHealth;


    }

}
