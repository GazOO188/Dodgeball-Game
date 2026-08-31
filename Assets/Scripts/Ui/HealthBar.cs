using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //THIS SCRIPT IS FOR HEALTHBAR LOGIC//

    [Header("Variables")]
    [SerializeField] private float MaxHealth = 100;
    [SerializeField] private float CurrentHealth;


    //SLIDER INFO//
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
        
        //DEDUCTS DAMAGE//
        CurrentHealth -= DamageAmount;


        //CLAMPS HEALTH SO THAT IT DOSENT GO BELOW 0//
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
       

        //SETS THE HEALTH BAR VALUE TO BE THE CURRENT HEALTH (VISUALLY)//
        HealthSlider.value = CurrentHealth;


    }

}
