using UnityEngine;
using UnityEngine.InputSystem;


public class EventMessenger : MonoBehaviour
{
   
    
    //THIS SCRIPT IS LINKINNG THE TEAM EVENET FUNCTION IN TEAMCHOICE//
    
    //REFERENCE TO OTHER SCRIPT//
    [SerializeField] public TeamChoice TC;
    
    [SerializeField] public MenuManager MM;

    [SerializeField] public UiManager UIM;
      
    //LINK THE MENUMANAGER ONTEAMSUBMIT TO THE TEAMCHOICE HANDLETEAMASSIGNMENT FUNCTION//
    private void OnEnable()
    {
        
        if(MM != null && TC != null)
        {
            //THERE IS NO INTSANCE OF A PLAYER YET SO MAKE THE EVENT IN THE MENU MANAGER//
            MM.OnTeamSubmitted += TC.HandleTeamAssignment;


        }

    }


    private void OnDisable()
    {
        

       
        if(MM != null && TC != null)
        {
            
            MM.OnTeamSubmitted -= TC.HandleTeamAssignment;
              
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


    public void TriggerTeamCheck(PlayerInput Pi)
    {
        
        PlayerInputHandler Pih = Pi.gameObject.GetComponent<PlayerInputHandler>();



        if(Pih != null && TC != null)
        {

            Pih.OnBothPlayersSubmitted += TC.CheckTeamSpots;

            Pih.OnBothPlayersSubmitted += UIM.ShowReadyButton; 


            //WHEN PLAYER CANCELS THEIR TEAM OPTION, LINK THE CANCEL EVENT TO THE HIDEREADYBUTTON TO PLAY THE HIDE ANIMATION//
            Pih.CancelToggleChoice += UIM.HideReadyButton;


            
        }



    }



}
