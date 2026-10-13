using UnityEngine;
using System.Collections.Generic; // ---> FOR DICTIONARIES//
using UnityEngine.InputSystem; 

public class TeamChoice : MonoBehaviour
{

    public static TeamChoice TeamManagerInstance{get; private set;}


    public enum Teams
    {
        None, 
        
        Red,

        Blue, 

    }

    public Teams currentTeams = Teams.None;


    [Header("Script References")]
    
    [SerializeField] public DetectButtons Db;
    [SerializeField] private UniversalGM UGM;



    private Dictionary<int, Teams> PlayerTeamTracker = new Dictionary<int, TeamChoice.Teams>();
   
   void Awake()
    {

        ///DECLARING || SAVING INSTANCE OF THIS JOINMANAGER//
        if (TeamManagerInstance == null)
        {
            TeamManagerInstance = this;


        }
        
        else
        {
            Destroy(gameObject);
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


    //FUNCTION TO ASSIGN TEAM -> THIS IS A RETURN FUNCTION//
    public Teams AssignTeams(Teams PlayerChoice)
    {

        switch (PlayerChoice)
        {
            
            case Teams.Red:
            {


                  
             
             break;


            }


            case Teams.Blue:
            {
                    

             break;

            }
            

        }

            //RETURN THE PLAYERCHOICE//
            return PlayerChoice;

        

        }
    


    //-----FUNCTION TO HANDLE TEAM SUBMISSION. LINKED TO THE (ONTEAMSUBMIT FUNCTION)-----//
    public void HandleTeamAssignment(PlayerInput Pi, Teams PlayerTeams)
    {
        

        int Playerindex = Pi.playerIndex;



        Teams AssociatedTeamOnPlayer = AssignTeams(PlayerTeams);



        if(!PlayerTeamTracker.ContainsKey(Playerindex))
        {
            
            PlayerTeamTracker.Add(Playerindex, AssociatedTeamOnPlayer);


        }


        //IF THE SAME PLAYER ALREADY HAS A TEAM LOGGED IN, UPDATE IT WITH THE NEW TEAM
        else
        {
             PlayerTeamTracker[Playerindex] = AssociatedTeamOnPlayer;

        }


        //THIS SAVES THE TEAMS TO PERSIST ACROSS SCENES SO THE GAME SCENE CAN USE THIS DATA//
        if(UGM != null)
        {
        
            UGM.SaveTeams(Playerindex, AssociatedTeamOnPlayer); 

            Debug.Log("Team Saved");
        
        }



        //Debug.Log($"Player: {Playerindex} joined team: {PlayerTeams}");


        DebugDictionaryEntries();

    }

    
    //-----FUNCTION TO HANDLE WHAT HAPPENS WHEN BOTH PLAYERS CHOOSE THEIR TEAM-----//
    public void CheckTeamSpots(PlayerInput pi, Teams PlayerTeams)
    {
        
        int PlayerIndex = pi.playerIndex;

        if(PlayerTeamTracker.Count <= 2)
        {


            if (PlayerTeamTracker.ContainsKey(PlayerIndex))
            {
                
                if(PlayerTeamTracker.TryGetValue(0, out Teams P1Team))
                {
                    

                    Debug.Log("Team Spot Filled");


                }


            }



        }




    } 



 //-----FUNCTION TO LOOP THROUGH AND PRINT ALL DICTIONARY ENTRIES-----//
    private void DebugDictionaryEntries()
    {
        Debug.Log("--- CURRENT LOBBY DICTIONARY ENTRIES ---");
        
        // KeyValuePair loops through every single combination inside your database tracking
        foreach (KeyValuePair<int, Teams> entry in PlayerTeamTracker)
        {
            // entry.Key is the Playerindex, entry.Value is their chosen Team
            Debug.Log($"[Database Check] -> Player ID: {entry.Key} is actively locked on: {entry.Value}");
        }

        Debug.Log("----------------------------------------");
    }
}







