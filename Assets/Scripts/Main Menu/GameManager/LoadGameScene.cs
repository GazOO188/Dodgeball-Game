using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public interface ISceneLoader
{
    
    bool CanCountdownBegin{get; set;}
   
    void Execute();

}

public class LoadGameScene : MonoBehaviour, ISceneLoader

{
    
    [SerializeField] private TextMeshProUGUI CountdownText;

   


    private WaitForSeconds TimerDelay;

   
    private Coroutine ActiveCoro;


    public bool CanCountdownBegin{get; set;}
   



    //A REFERENCE TO THE UIMANAGER SCRIPT WITH THE IUIANIMATION INTERFACE -> (THE UIMananger SCRIPT)//
    private IUiAnimation Uiscript;

    


    private void Awake()
    {

        TimerDelay = new WaitForSeconds(1.0f); 

        
        CountdownText.enabled = false;  


    }

    
    //STORE THE UIMANAGER SCRIPT FOR REFERENCING//
    public void StoreUIAnimScript(IUiAnimation UiManagerscript)
    {
        

        Uiscript = UiManagerscript;


    }
    
  
    //THIS FUNCTION WILL PLAY WHEN IS IT CALLED IN THE MENU MANAGER SCRIPT B/C OF THE REFERENCE//
    public void Execute()
    {
       

       CanCountdownBegin = true;

      
       if(ActiveCoro == null && CanCountdownBegin)
       {
            
        ActiveCoro = StartCoroutine(Countdown());    

       }


     

    }


    //THIS COROUTINE IS FOR COUNTDING DOWN//
    private IEnumerator Countdown()
    {

        IUiAnimation UIManagerScript = Uiscript;

      
        
        CountdownText.enabled = true;  
       
       
        CountdownText.text = "3";
       
      
        if(UIManagerScript != null)
        {
           Uiscript.HideReadyButton();
        }
       
       
        yield return TimerDelay;


        CountdownText.text = "2";
        yield return TimerDelay;



        CountdownText.text = "1";
        
        
        yield return TimerDelay;
        CountdownText.text = "0";

       
        if(UIManagerScript != null)
        {
          Uiscript.PlayDirectorTransition();  
        }
       
        
        yield return TimerDelay;

        
        CountdownText.enabled = false;
        
       
        yield return TimerDelay;  

        TransitionToGameScene("Game Scene");


    }



    //LOAD THE GAME SCENE//
    private void TransitionToGameScene(string SceneName)
    {
        

        SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);



    }



    //----FUNCTION TO STOP THE ACTIVE COUROUTINE----//
    public void StopCurrentCoroutine()
    {
        
        if(ActiveCoro != null)
        {
            
            //STOP THE COUNTDOWN FROM PLAYING//
            StopCoroutine(ActiveCoro);


            //EMPTY OUT THE CURRENT COROUTINE//
            ActiveCoro = null;


            //HIDE THE COUNTDOWN TEXT//
            CountdownText.enabled = false;


            //STOP THE PLAYABLE DIRECTOR//
            Uiscript.StopFadetoBlackAnim();


        }



    }




}
