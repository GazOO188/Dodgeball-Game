using UnityEngine;
using System.Collections.Generic; // ---> FOR DICTIONARIES//
using UnityEngine.InputSystem; 


public interface ITeamManager
{
    
    //REFERENCE TO THE PLAYERTEAMTRACKER INT//

    int PlayerCount{get;}

    void HandleTeamAssignment(PlayerInput Pi, TeamChoice.Teams PlayerTeams);

    void CheckTeamSpots(PlayerInput pi, TeamChoice.Teams PlayerTeams);

    void HandleTeamAddition(int playerID, TeamChoice.Teams PlayerTeams);

    void HandleTeamRemoval(int playerID, TeamChoice.Teams PlayerTeams);


    Dictionary<int, TeamChoice.Teams> PlayerTeamTracker{get;}


}


public class TeamChoice : MonoBehaviour, ITeamManager
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


    //DICTIONARY TO STORE THE PLAYER AND THEIR CORRESPONDING TEAM//
    public Dictionary<int, Teams> playerTeamTracker = new Dictionary<int, Teams>();
    
    

    //A PROPERTY (READ-ONLY) FOR DICTIONARY//
    public Dictionary<int, Teams> PlayerTeamTracker
    {

        get
        {
            return playerTeamTracker;
        }



    }


    //A PROPERTY (READ-ONLY) FOR AN INT VAIRIABLE//
    public int PlayerCount
    {
        
        get{return PlayerTeamTracker.Count;}

    }


    //REFERENCE TO UIMANAGER//
    private IUiAnimation UIManagerScript;
   
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
    
  
    //STORE A REFERENCE TO THE UIMANAGER//

    public void StoreUIManagerScriptref(IUiAnimation UIManRef)
    {

        UIManagerScript = UIManRef;  



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


    //-----FUNCTION TO HANDLE REMOVING PLAYER TEAM ENTRIES-----//
    public void HandleTeamRemoval(int PlayerID, Teams PlayerTeams)
    {
        
        
        if(playerTeamTracker.ContainsKey(PlayerID))
        {
            
            playerTeamTracker.Remove(PlayerID);


            if (UGM.UniversalTeamData.ContainsKey(PlayerID))
            {
                
                UGM.UniversalTeamData.Remove(PlayerID);  


            }

            


        }




    }


    //-----FUNCTION TO HANDLE ADDING TEAM ENTIRES-----//
    public void HandleTeamAddition(int PlayerIndex, Teams PlayerTeams)
    {
        
      
        //IF THE PLAYER INDEX ISN'T IN THE DICTIONARY, ADD THE PLAYERINDEX TO THE PLAYERTEAMTRACKER//
        if(!playerTeamTracker.ContainsKey(PlayerIndex))
        {
            
            //ADD THE PLAYERINDEX TO THE TEAM DICTIONARY TRACKER//
            playerTeamTracker.Add(PlayerIndex, PlayerTeams);


            //ALSO ADD THE INDEX TO THE UINIVERSALTEAM DATA DICTIONARY//
            if(UGM != null && !UGM.UniversalTeamData.ContainsKey(PlayerIndex))
            {
                
               UGM.UniversalTeamData.Add(PlayerIndex, PlayerTeams);  



            }
            
           

        }



    }

    
    //-----FUNCTION TO HANDLE WHAT HAPPENS WHEN BOTH PLAYERS CHOOSE THEIR TEAM-----//
    public void CheckTeamSpots(PlayerInput pi, Teams PlayerTeams)
    {
        
        //GET THE PLAYER INDEX//
        int PlayerIndex = pi.playerIndex;

       
        //IF THE PLAYERTEAMTRACKER CAPACITY IS 2: DO THE CODE BELOW//
        if(PlayerTeamTracker.Count == 2)
        {


            if (PlayerTeamTracker.ContainsKey(PlayerIndex))
            {
                
                if(PlayerTeamTracker.TryGetValue(0, out Teams P1Team) && PlayerTeamTracker.TryGetValue(1, out Teams P2Team))
                {
                    
                    //THIS SHOWS THE START BUTTON ONCE THE PLAYERS ARE LOCKED ON THEIR TEAMS//
                    UIManagerScript.ShowReadyButton();
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



    public class EnemyClass
    {
        
       
        private int health;


        public EnemyClass(int StartingHealth)
        {

            health = StartingHealth;   


        }

        //Function to handle health//
        public void HandleHealth(int amount)
        {
            
            health -= amount;


        }


    }


    public class Zombie : EnemyClass
    {
        

 // ✔️ FIXED: You write this by hand to catch the data and forward it up-line!
    public Zombie(int StartingHealth) : base(StartingHealth)
    {
        // You can leave the body completely empty!
    }


    }


    


}







