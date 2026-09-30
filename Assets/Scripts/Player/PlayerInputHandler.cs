using UnityEngine;
using UnityEngine.InputSystem; 
using System;

public class PlayerInputHandler : MonoBehaviour
{


    //THIS SCRIPT HANDLES PLAYER INPUT//
    [Header("Script References")]
    [SerializeField] public DetectButtons Db;


    [Header("Input Stuff")]
    [SerializeField] private PlayerInput PI;


  
    //------EVENTS-------//
   
    //WHEN THIS EVENT HAPPENS IT WILL SEND PLAYERINPUT INFORMATION TO THE FUNCTION IT IS SUBSCRIBED TO//
    public event Action<PlayerInput> OnSubmitPressed;
    
    //---TEAM EVENTS---//
    public event Action<PlayerInput, TeamChoice.Teams> OnTeamSubmitted;


    //---CANCEL EVENT---//
    public event Action<PlayerInput> CancelToggleChoice;

   

    [SerializeField] private bool Submitted = false;

    //---------SUBSCRIBE TO ACTIONS-------------//
    void OnEnable()
    {
    
        if(PI == null)
        { 
         PI = GetComponent<PlayerInput>();

        }


        //LINKS/SUBSCRIBES TO THE FUNCTION TO THE INPUT ACTION EVENT//
        if(PI != null)
        {
            
            PI.actions["Submit"].performed += OnSubmit;
            PI.actions["Cancel"].performed += OnCancel;



        }




    }

    //---------UNSUBSCRIBE TO ACTIONS---------------//
    void OnDisable()
    {
        
        if(PI != null)
        {
            
            PI.actions["Submit"].performed -= OnSubmit;
            PI.actions["Cancel"].performed -= OnCancel;

        }


    }

    private void Awake()
    {
        
        PI = GetComponent<PlayerInput>();

    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // ------------------UI INPUT ACTIONS------------------------------

    //THIS FUNCTION IS FOR PRESSING A ON THE UI WHEN SELECTING TEAM COLOR//
    private void OnSubmit(InputAction.CallbackContext context)
    {
        
        if(Db == null)
        {
            return;  


        }

        if(context.performed)
        {


            Debug.Log("Julius Pizarro");


            OnSubmitPressed?.Invoke(PI);

            
            //CONDITIONAL STATEMENT IF ISONRED IS TRUE -> ASSIGN TEAM TO BE RED, ELSE TEAM IS BLUE//
            TeamChoice.Teams ChosenTeam = Db.IsonRed ? TeamChoice.Teams.Red : TeamChoice.Teams.Blue;


            //THIS GIVES THE PLAYERINPUT AND THE CHOSENTEAM VARIABLE AS INFO FOR THE EVENT FUNCTION//
            OnTeamSubmitted?.Invoke(PI, ChosenTeam);
            
            

        }



    }


    //THIS FUNCTION IS FOR CANCELING//
    private void OnCancel(InputAction.CallbackContext context)
    {


        if(context.performed)
        {
            
            Debug.Log("Cacnel Event Triggered");


            CancelToggleChoice?.Invoke(PI);


        }




    }


     // ------------------END OF UI INPUT ACTIONS------------------------------

}
