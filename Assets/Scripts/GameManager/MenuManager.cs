using UnityEngine;
using UnityEngine.InputSystem; //INCLUDE THIS NAMESPACE FOR INPUTACTION//
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MenuManager : MonoBehaviour
{
   
    public static MenuManager MenuJoinInstance{get; private set;}

    [Header("UI Elements")]
    [SerializeField] private GameObject TogglePrefab;
    [SerializeField] private Transform UICanvas;



    [Header("Locations For UI")]
    [SerializeField] private List<Transform> ToggleLocations = new List<Transform>();
  

    public Dictionary<int, DetectButtons> PlayerWithDBScript = new Dictionary<int, DetectButtons>();
   
    
    private Dictionary<int, TeamChoice.Teams> PlayerTeamTracker = new Dictionary<int, TeamChoice.Teams>();


    //DICTIONARY THAT TRACKS  PLAYER TO THEIR TOGGLE//
    private Dictionary<int, GameObject> PlayerToggle = new Dictionary<int, GameObject>();

   

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
         
         PIH.OnTeamSubmitted += TeamChoice.TeamManagerInstance.HandleTeamAssignment;

         PIH.CancelToggleChoice += RemoveToggleOnScreen;


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


            if (playerWithDbScript.IsonRed)
            {
                
                TargetLocations = ToggleLocations[0];



            }

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
            

            //GET THE BACKGROUND COMPONENT ON THE CHILD OF THE TOGGLE
            Transform Background = ToggletoSpawn.transform.Find("Background");

            
            if (Background != null)
            {
                // ADJUST THE SCALE OF THE TOGGLE BACKGROUND
                Background.transform.localScale = new Vector3(0.7453667f, 0.7453667f, 0.7453667f);

                
                //GET THE RECT TRANSFORM AND MAKE IT CENETERED AROUND THE POSITION OF THE TOGGLE//
                RectTransform bgRect = Background.GetComponent<RectTransform>();
                if (bgRect != null)
                {
                    bgRect.anchoredPosition = Vector2.zero;
                    bgRect.localPosition = Vector3.zero;
                }
            }


  
            //ADD THE PLAYER'S TOGGLE TO THE DICTIONARY
            PlayerToggle.Add(PlayerIndex, ToggletoSpawn);
        }
    }
}

   

    //-----FUNCTION TO HANDLE REMOVING TOGGLE BUTTON WHEN PRESSING ("B") ->> LINKS TO CANCEL EVENT-----//
     private void RemoveToggleOnScreen(PlayerInput PI)
    {
      
        int playerIndex = PI.playerIndex;


        if (PlayerToggle.TryGetValue(playerIndex, out GameObject Toggle))
        {


            Destroy(Toggle);
            
            PlayerToggle.Remove(playerIndex);  



        }



    }



  

  

}
