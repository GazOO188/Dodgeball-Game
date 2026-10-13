using UnityEngine;
using UnityEngine.InputSystem; //INCLUDE THIS NAMESPACE FOR INPUTACTION//
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
   
    public static MenuManager MenuJoinInstance{get; private set;}

    [Header("UI Elements")]
    [SerializeField] private GameObject TogglePrefab;
    [SerializeField] private Transform UICanvas;



    [Header("Locations For UI")]
    [SerializeField] private List<Transform> ToggleLocations = new List<Transform>();
  

    public Dictionary<int, DetectButtons> PlayerWithDBScript = new Dictionary<int, DetectButtons>();
   



    //DICTIONARY THAT TRACKS  PLAYER TO THEIR TOGGLE//
    private Dictionary<int, GameObject> PlayerToggle = new Dictionary<int, GameObject>();



    //---TEAM EVENTS---//
    public event Action<PlayerInput, TeamChoice.Teams> OnTeamSubmitted;

   

    void OnEnable()
    {
    
    }


    void OnDisable()
    {

    }
    
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    //JOIN FUNCTION//
    public void OnPlayerJoined(PlayerInput playerInput)
    {

        int PlayerIndex = playerInput.playerIndex;
        Debug.Log("Player: " + PlayerIndex);
        

        //----GET THE PLAYERINPUTHANDLER SCRIPT ON THE PLAYER THAT JOINED------//
        PlayerInputHandler PIH = playerInput.gameObject.GetComponent<PlayerInputHandler>();

       
        //----GET THE DETECT BUTTONS SCRIPT ON THE PLAYER THAT JOINED------//
        DetectButtons DB = playerInput.gameObject.GetComponent<DetectButtons>();


        //ADD THE RESPECTIVE PLAYER AND THEIR DETECT BUTTONS SCRIPT TO THE DICTIONARY//
        PlayerWithDBScript.Add(PlayerIndex, DB);

        

       
       
        
        if(PIH != null)
        {
            
         ///THIS LINKS THE EVENTS TO THEIR RESPECTIVE FUNCTIONS
         
         PIH.OnSubmitPressed += SpawnToggle;
         PIH.CancelToggleChoice += RemoveToggleOnScreen;

         //THIS LINKS THE ONTEAMSUBMIITED EVENT IN PLAYERINPUTHANDLER TO TRIGGERTEAMSUBMISSIONFULLY//
         PIH.OnTeamSubmitted += TriggerTeamSubmissionFully;

        

        }

       



        // THIS KEEPS THE PLAYER'S CURSOR IN THE UI CANVAS SO IT'S SEEN//
        if (UICanvas != null)
        {
            
            playerInput.transform.SetParent(UICanvas, false);
        
        }




    }


    //-----FUNCTION TO HANDLE TOGGLE SPAWN-------//
    private void SpawnToggle(PlayerInput Pi)
    {
        
        int PlayerIndex = Pi.playerIndex;

        GameObject ToggletoSpawn = null;

        Transform TargetLocations = null;


        //GET THE PLAYERINPUTHANDLER SCRIPT//
        PlayerInputHandler PiH = Pi.gameObject.GetComponent<PlayerInputHandler>();

        


        //RETURNS(EXITS) IF THE SCRIPT ATTACHED TO THE PLAYER CAN'T BE FOUND//
        if (!PlayerWithDBScript.TryGetValue(PlayerIndex, out DetectButtons playerWithDbScript))
        {
            return; 
        }
       
        //SAFETY CHECK//
        if(PlayerToggle.ContainsKey(PlayerIndex))
        {
            
            //Debug.Log("Hey it does");
            
            return;

        }



        if(PlayerIndex <= ToggleLocations.Count)
        {


            //IS ISONRED IS TRUE -> SPAWN THE TOGGLE AT THE RED SIDE BUTTON//
            if (playerWithDbScript.IsonRed)
            {
                
                TargetLocations = ToggleLocations[0];



            }


            //IS ISONBLUE IS TRUE -> SPAWN THE TOGGLE AT THE BLUE SIDE BUTTON//
            else if(playerWithDbScript.IsonBlue)
            {
                
                TargetLocations = ToggleLocations[1];

                Debug.Log("Spawn on Blue");

            }


            
            if(TargetLocations != null)
           {
           
            //SPAWN THE TOGGLE -> THIS PUTS THE TOGGLE IN THE UICANVAS SO IT APPEARS ON THE CANVAS
            ToggletoSpawn = Instantiate(TogglePrefab, UICanvas); 

            //THIS MAPS THE TOGGLE TO THEIR CORRECT LOCATIONS
            ToggletoSpawn.transform.position = TargetLocations.position;

            //GET THE TOGGLESETTINGS SCRIPT//
            ToggleSettings TS = ToggletoSpawn.GetComponent<ToggleSettings>();


            //GETTING PLAYER INPUT REFERENCE//
            TS.RetrievePlayerInput(Pi);
            
            

    
            //ADD THE PLAYER'S TOGGLE TO THE DICTIONARY
            PlayerToggle.Add(PlayerIndex, ToggletoSpawn);
           
           }
    
    
        }
}

   

    //-----FUNCTION TO HANDLE REMOVING TOGGLE BUTTON WHEN PRESSING ("B") ->> LINKS TO CANCEL EVENT-----//
     private void RemoveToggleOnScreen(PlayerInput PI)
    {
      
        //GET PLAYERINDEX VALUE//
        int playerIndex = PI.playerIndex;

        //IF THE TOGGLE EXISTS, DESTROY IT//
        if (PlayerToggle.TryGetValue(playerIndex, out GameObject Toggle))
        {


            Destroy(Toggle);
            
            PlayerToggle.Remove(playerIndex);  



        }


    }


    //-----FUNCTION TO FULLY TRIGGER THE EVENT-----//
    private void TriggerTeamSubmissionFully(PlayerInput Pi, TeamChoice.Teams teams)
    {
        

        //MAKE THIS VERSION OF ONTEAMSUBMITTED EVENT THAT IS LINKED TO THE TEAMCHOICE FUNCTION, TRIGGER//
        OnTeamSubmitted?.Invoke(Pi, teams);



    }

    
  



  

}
