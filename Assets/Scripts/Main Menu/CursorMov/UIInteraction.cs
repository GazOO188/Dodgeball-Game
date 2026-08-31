using UnityEngine;
using UnityEngine.InputSystem; 

public class UIInteraction : MonoBehaviour
{
    
    //THIS SCRIPT IS FOR THE INTERACTIONS WITH BUTTONS ON BUTTON PRESS//

    [Header("Script References")]
    [SerializeField] public DetectButtons Db;


    [Header("Input Stuff")]
    [SerializeField] private PlayerInput PI;


   

    
    void OnEnable()
    {
        
        if(PI == null)
        { 
         PI = GetComponent<PlayerInput>();

        }



        if(PI != null)
        {
        
        PI.actions["Submit"].performed += OnSubmit;
        
        }

    }


    void OnDisable()
    {
        

        if(PI != null)
        {
        
        PI.actions["Submit"].performed -= OnSubmit;
        
        }


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //THIS FUNCTION IS FOR PRESSING A ON THE UI WHEN SELECTING TEAM COLOR//
    private void OnSubmit(InputAction.CallbackContext context)
    {
        
        if(Db == null)
        {
            return;  


        }

        if(context.performed && Db.CurrentButton != null)
        {


            Debug.Log("Julius Pizarro");
            
            

        }




    }
}
