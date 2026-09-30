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


    //FUNCTION TO ASSIGN TEAMS//
    public void AssignTeams(Teams PlayerChoice)
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
    }


      //-----FUNCTION TO HANDLE TEAM SUBMISSION. LINKED TO THE (ONTEAMSUBMIT FUNCTION)-----//
    public void HandleTeamAssignment(PlayerInput Pi, Teams PlayerTeams)
    {
        

        int Playerindex = Pi.playerIndex;
        AssignTeams(PlayerTeams);




        Debug.Log($"Player: {Playerindex} joined team: {PlayerTeams}");

    }







}
