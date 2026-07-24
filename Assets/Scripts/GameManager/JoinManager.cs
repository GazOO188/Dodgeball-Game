using UnityEngine;
using UnityEngine.InputSystem; //INCLUDE THIS NAMESPACE FOR INPUTACTION//
using System.Collections;
using System.Collections.Generic;

public class JoinManager : MonoBehaviour
{
    //THIS SCRIPT IS FOR JOINING IN THE GAME//
    
    //INSTANCE OF JOINMANAGER//
    public static JoinManager JoinInstance { get; private set; }


    [Header("Transform")]
    [SerializeField] private List<Transform> PlayerSpawnPoints = new List<Transform>();
    [SerializeField] private int Index;



    [Header("Script Reference")]
    [SerializeField] public HealthBar HealthScript;


    [Header("Boolean")]
    [SerializeField] public bool IsinRedTeam, IsinBlueTeam;
   

    //ENUM FOR TEAMS//
    public enum Teams
    {

     None,
          
     Red,

     Blue,


    }

     public Teams CurrentTeams = Teams.None;

    
    void Awake()
    {

        ///DECLARING || SAVING INSTANCE OF THIS JOINMANAGER//
        if (JoinInstance == null)
        {
            JoinInstance = this;
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


    //THIS FUNCTION IS CALLED WHEN A PLAYER JOINS THE GAME ON BUTTON INPUT/PRESS
    public void OnPlayerJoined(PlayerInput playerInput)
    {
         
           // DEBUG//
           Debug.Log("PLAYERJOINED!");

           
          //SAFETY CHECK: IF PLAYER DATA/PLAYER DOSENT EXIST OR MISSING STOPS THE CODE HERE//
          if (playerInput == null) return;


              //THIS LOOKS INSIDE THE NEWLY SPAWNED PLAYER AND SEARCHES FOR THE PLAYERMOVEMENT SCRIPT//
              PlayerMovement playerScript = playerInput.GetComponent<PlayerMovement>();

         //IF THE PLAYERSCRIPT IS SUCCESFFULLY FOUND -> != NULL
          if (playerScript != null)
          {
                
                //THIS CHECKS IF THE GAMEMANAGER (SCRIPT/OBJ) EXISTS IN THE SCENE//
                if (GameManager.Instance != null)
                {
                    //SAVES THE CANVAS INTO THIS VARIABLE//
                    GameObject globalCanvas = GameManager.Instance.RequestCanvas();
            
                    // ASSIGNS THE CANVAS OFFCIAILLY//
                    playerScript.AssignButtonCanvas(globalCanvas);
                }
    
    
          } 
          
          //GETS THE CURRENT PLAYER INDEX (WHO CURRENTLY JOINED)//
          int PlayerIndex = playerInput.playerIndex;

          //THE CURRENT TEAM SET TO NONE BY DEFAULT FOR NOW//
          Teams CurrentTeam = Teams.None;


          //SPAWN THE PLAYER AT THEIR SPAWN POINTS//  

          //IF THE PLAYERINDEX IS LESS THAN THE PLAYER SPAWN AMOUNTS THEN DO THIS, ELSE DON'T DO THIS CODE IF THERE A LOT OF PLAYERS/
          if (PlayerIndex < PlayerSpawnPoints.Count && PlayerSpawnPoints[Index] != null)
          {
                //THIS ASSIGNS THE PLAYER'S TO THEIR POSITION ON THE MAP WITH CORRECT POSITION AND ROTATION//
                playerInput.transform.position = PlayerSpawnPoints[PlayerIndex].position;
                
                playerInput.transform.rotation = PlayerSpawnPoints[PlayerIndex].rotation;

                

          }

          //IF THE PLAYER INDEX IS IN BETWEEN THE RANGES OF 0-2, THEY ARE TEAM RED//
          if(PlayerIndex <= 2)
          {
               
              CurrentTeam = Teams.Red;


          }

          //IF NOT IN THE RANGES BETWEEN 0-2, THEY ARE TEAM BLUE//
          else
          {
          
             
             CurrentTeam = Teams.Blue;


          }


          //SETS THE COLORS OF THE TEAM BASED ON HOW THEY JOIBED
          switch (CurrentTeam)
          {
               
               //IF IN BETWEEN THE RANGES 0-2, GIVE THE PLAYER A RED COLOR//
               case Teams.Red:
               {
                    //MAKES THE PLAYER APPEAR RED//
                    Renderer r = playerInput.GetComponent<Renderer>();

                    r.material.color = Color.red;


                    break;

               }

               //IF NOT IN BETWEEN THE RANGES 0-2, GIVE THE PLAYER A BLUE COLOR//
               case Teams.Blue:
               {
                    
                    //MAKES THE PLAYER APPEAR BLUE//
                    Renderer r = playerInput.GetComponent<Renderer>();

                    r.material.color = Color.blue;
                    
                    break;

               }







          }

    }


}



