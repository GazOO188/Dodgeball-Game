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
    [SerializeField] private Transform SelectTeamUICanvas;


    [Header("UI State")]
    [SerializeField] private bool CanHideStartButton = false;
    


    [Header("Locations For UI")]
    [SerializeField] private List<Transform> ToggleLocations = new List<Transform>();
  

    public Dictionary<int, DetectButtons> PlayerWithDBScript = new Dictionary<int, DetectButtons>();
   



    //DICTIONARY THAT TRACKS PLAYER TO THEIR TOGGLE//
    private Dictionary<int, GameObject> PlayerToggle = new Dictionary<int, GameObject>();



    //---TEAM EVENTS---//
    public event Action<PlayerInput, TeamChoice.Teams> OnTeamSubmitted;
    

   
    //---REF TO THE SCRIPT WITH THE INTERFACE(LOADGAMESCENE)---//
    private ISceneLoader sceneLoader;

    
    //---REF TO THE SCRIPT WITH THE INTERFACE(EVENTMESSENGER)---//
    private IEventMessenger eventMessenger;

    
    //---REF TO THE SCRIPT WITH THE INTERFACE(UIManager)---//
    private IUiAnimation UIManager;

    
    //---REF TO THE SCRIPT WITH THE INTERFACE(TEAMCHOICE)---//
    private ITeamManager TeamChoice;


    private bool CanCancelOption = false;
   

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

    
    //THIS FUNCTION STORES A REFERENCE TO THE LOADGAME SCENE SCRIPT//
    public void StoreScriptReference(ISceneLoader incomingReference)
    {
       
        sceneLoader = incomingReference;
        
    }


    //-----THIS FUNCTION IS TO STORE A REFERENCE TO THE EVENTMESSENGER SCRIPT-----//
    public void StoreEventMessengerScript(IEventMessenger incomingReference)
    {
        

        eventMessenger = incomingReference;


    }

    
    //------THIS FUNCTION IT TO STORE A REFERECNE TO THE UIMANAGER SCRIPT WITH THE INTERFACE ATTACHED-----//
    public void StoreUIManagerScript(IUiAnimation incomingReference)
    {

        UIManager = incomingReference;


    }



    //------THIS FUNCTION IS TO STORE A REFERENCE TO THE TEAMCHOICE SCRIPT-----//
    public void StoreTeamChoice(ITeamManager incomingReference)
    {
        
        TeamChoice = incomingReference;


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
         PIH.CancelToggleChoice += HandleTimeCancel;
         

        
         //THIS LINKS THE ONTEAMSUBMIITED EVENT IN PLAYERINPUTHANDLER TO TRIGGERTEAMSUBMISSIONFULLY//
         PIH.OnTeamSubmitted += TriggerTeamSubmissionFully;


         PIH.OnStartButtonPressed += TriggerStartButton;

        

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



            // THIS KEEPS THE PLAYER'S CURSOR IN THE UI CANVAS SO IT'S SEEN//
            if (UICanvas != null)
            {
            
             //THIS SETS THE TOGGLE THAT SPAWNED TO BE A CHILD OF THE TEAMUI CANVAS//
             ToggletoSpawn.transform.SetParent(SelectTeamUICanvas, false);

             //MAKE THE TOGGLE THAT SPAWNED APPEAR 2 IN THE CHILD LIST SO THAT IT DOSEN'T BE OVERLAY OR BE ON TOP OF THE TRANSTION ANIMATION//
             ToggletoSpawn.transform.SetSiblingIndex(2);
        
            }
    
    
        }



        //LOGIC FOR ADDING TEAM INTO PLAYERTEAMTRACKER//
        if(TeamChoice != null)
        {

            //THIS IS SAYING: GET THE VALUE OF THE PLAYERINDEX IN THE DICTIONARY AND ITS ASSOCIATED TEAM//
            if (TeamChoice.PlayerTeamTracker.TryGetValue(PlayerIndex, out TeamChoice.Teams ChosenTeam))
                
                
                TeamChoice.HandleTeamAddition(PlayerIndex, ChosenTeam);



            }

        
        }
        
        

   

    //-----FUNCTION TO HANDLE REMOVING TOGGLE BUTTON WHEN PRESSING ("B") ->> LINKS TO CANCEL EVENT-----//
     private void RemoveToggleOnScreen(PlayerInput PI)
    {
      
        //GET PLAYERINDEX VALUE//
        int playerIndex = PI.playerIndex;


       
        PlayerInputHandler PIH = PI.gameObject.GetComponent<PlayerInputHandler>();



        //IF THE PLAYERTEAMTRACKER IS 2 CANHIDESTARTBUTTON IS TRUE || ELSE ITS FALSE
        CanHideStartButton = (TeamChoice.PlayerTeamTracker.Count == 2) ? true : false;

       
        //IF THE TOGGLE EXISTS, DESTROY IT//
        if (PlayerToggle.TryGetValue(playerIndex, out GameObject Toggle))
        {


            Destroy(Toggle);
            
           
            PlayerToggle.Remove(playerIndex);


            //HIDE THE READY BUTTON WHEN THE PLAYER CANCELS THEIR OPTION//
            
            //THIS PREVENTS THE HIDEBUTTON FROM BEING CANCELED TWICE//
            if(!sceneLoader.CanCountdownBegin || CanHideStartButton)
            {
                
           
                UIManager.HideReadyButton();
           
           
            }



            //REMOVE THE PLAYER FROM THE DICTIONARY TEMPORARILY, UNTIL DECIDING//
            if(TeamChoice.PlayerTeamTracker.TryGetValue(playerIndex, out TeamChoice.Teams ExclusiveTeam))
            {
                
                

                TeamChoice.HandleTeamRemoval(playerIndex, ExclusiveTeam);


            }
            
        
        
        }


    }


    //-----FUNCTION TO HANDLE STOPPING THE COUNTDOWN COROUTINE-----//
    private void HandleTimeCancel(PlayerInput PI)
    {
        
        IEventMessenger Ievent = eventMessenger;


        if(Ievent != null)
        {
            


            Ievent.Cancel();
        }




    }


    //-----FUNCTION TO FULLY TRIGGER THE ONTEAMSUBMMITED EVENT-----//
    private void TriggerTeamSubmissionFully(PlayerInput Pi, TeamChoice.Teams teams)
    {
        

        //MAKE THIS VERSION OF ONTEAMSUBMITTED EVENT THAT IS LINKED TO THE TEAMCHOICE FUNCTION, TRIGGER//
        OnTeamSubmitted?.Invoke(Pi, teams);



    }


    //-----FUNCTION TO FULLY TRIGGER THE ONSTARTBUTTONCLICKED EVENT-----//
    private void TriggerStartButton(PlayerInput PI, DetectButtons DB)
    {
        

        //GET THE LOADGAMESCENE SCRIPT//
        ISceneLoader LGS = sceneLoader;


        //GET THE PLAYERINPUTHANDLER SCRIPT//
        PlayerInputHandler PiH = PI.gameObject.GetComponent<PlayerInputHandler>();


        if(LGS != null && PiH != null)
        {
            
            LGS.Execute();

          


        }



      }



    //----FUNCTION TO CARRY OUT WHAT HAPPENS WHEN 
  



  

}
