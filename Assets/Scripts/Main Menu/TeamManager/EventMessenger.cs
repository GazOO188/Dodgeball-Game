using UnityEngine;
using UnityEngine.InputSystem;

//CURRENTLY WOEKING ON A WAY TO ACCESS THE EVENTMESSGENR IN THE MENUMANAGER IN A CLEAN LESS TIGHTLY COUPLED WAY FOR CANCELING OPTION//
public interface IEventMessenger
{
    
    void Cancel();

}

public class EventMessenger : MonoBehaviour, IEventMessenger
{
   
    
    //THIS SCRIPT IS LINKINNG THE TEAM EVENET FUNCTION IN TEAMCHOICE//
    
    //REFERENCE TO OTHER SCRIPT//
    [SerializeField] public TeamChoice TC;
    
    [SerializeField] public MenuManager MM;

    [SerializeField] public UiManager UIM;


    //A REFERENCE TO THE LOADGAMESCENE//
    [SerializeField] private LoadGameScene targetScript; 

    [SerializeField] private EventMessenger EM;
      
    
    
    
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


    
    void Start()
    {
        //DRAG THE LOADGAMESCENE AND SAVES A REFERENCE TO IT, SO YOU CAN USE THE REFERENCE IN MENUMANAGER//
        if(targetScript != null)
        {
            
            //THIS FULLY STORES THE REFERENCE//
            MM.StoreScriptReference(targetScript);


        }


        //DRAG THE EVENTMESSENGER AND SAVES A REFERENCE TO IT, SO YOU CAN USE THE REFERENCE IN MENUMANAGER//
        if(EM != null)
        {
            

            MM.StoreEventMessengerScript(EM);



        }




        //DRAG THE UIMANAGER AND SAVES A REFERENCE TO IT, SO YOU CAN USE THE REFERENCE IN LOADGAMESCENE//
        if(UIM != null)
        {
            

            targetScript.StoreUIAnimScript(UIM);


        }

    
    
    }
   
   
   
    // Update is called once per frame
    void Update()
    {
        
    }







    //THIS FUCNTION IS BEING USED: CONNECTED TO THE JOINMANAGER//

    public void TriggerTeamCheck(PlayerInput Pi)
    {
        
        PlayerInputHandler Pih = Pi.gameObject.GetComponent<PlayerInputHandler>();



        if(Pih != null && TC != null)
        {

            Pih.OnBothPlayersSubmitted += TC.CheckTeamSpots;

            Pih.OnBothPlayersSubmitted += UIM.ShowReadyButton; 

            
        }



    }

    



    //FUNCTION A PART OF THE CONTRACT//
    public void Cancel()
    {
        
        if(targetScript != null)
        {
            
            //A REFERENCE TO THE LOADSCENE SCRIPT//
            targetScript.StopCurrentCoroutine();



        }


    }



    //-----FUNCTION TO CLEAN UP EVENTS-----//
    public void CleanUpEvents(PlayerInput pi)
    {
        
        PlayerInputHandler PIH = pi.gameObject.GetComponent<PlayerInputHandler>();


        if(PIH != null)
        {
            


            PIH.OnBothPlayersSubmitted -= TC.CheckTeamSpots;

            PIH.OnBothPlayersSubmitted -= UIM.ShowReadyButton; 


        }




    }






}
