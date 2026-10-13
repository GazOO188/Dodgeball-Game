using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
    
    //THIS SCRIPT IS TO HANDLE ALL OF THE UI ELEMENTS (SHUTTING THEM OFF/ETC)
    [SerializeField] public List<GameObject> UiElements = new List<GameObject>();


    [SerializeField] public GameObject ReadyButton;


    [SerializeField] private Animator ReadyAnim;


    void Awake()
    {
        
        ReadyAnim.enabled = false;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //TOGGLE OFF ALL UI ELEMENTS//

    public void ToggleOffUiElements()
    {
        

        foreach(GameObject UI in UiElements)
        {
            

            if(UI != null)
            {
                
            
                 UI.SetActive(false);
            
            
            
            }
            



        }




    }


    //-----FUNCTION TO SHOW THE READY BUTTON-----//
    public void ShowReadyButton(PlayerInput Pi, TeamChoice.Teams teams)
    {
        
        
        if(ReadyButton != null)
        {
            
            Button SrtButton = ReadyButton.GetComponent<Button>();


            //THIS MAKES THE BUTTON NOT INTERACTABLE WHEN THE HIDE ANIMATION PLAYS//
            if(SrtButton != null)
            {
                
                SrtButton.interactable = true;

            }

            ReadyButton.SetActive(true);

            ReadyAnim.enabled = true;

            HandleCorrectAnimationClip("SlideIn");

        }


    }


    //-----FUNCTION TO HIDE THE READY BUTTON-----//
    public void HideReadyButton(PlayerInput Pi)
    {
        
        
        if(ReadyButton != null)
        {

            Button SrtButton = ReadyButton.GetComponent<Button>();


            //THIS MAKES THE BUTTON NOT INTERACTABLE WHEN THE HIDE ANIMATION PLAYS//
            if(SrtButton != null)
            {
                
                SrtButton.interactable = false;

            }

            ReadyButton.SetActive(true);

            
            HandleCorrectAnimationClip("SlideOut");

            

        }


    }



    //-----FUNCTION TO HANDLE PLAYING THE CORRECT ANIMATION-----//
    public void HandleCorrectAnimationClip(string AnimationName)
    {

        //IF THERE IS NO ANIMATOR STOP RIGHT HERE!//
        if(ReadyAnim == null)
        {
            return;
        }
        
        //CONDITIONS FOR PLAYING THE RIGHT ANIMATION//
        switch (AnimationName)
        {
            
            case "SlideIn":
            {
                    
                ReadyAnim.Play("SlideIn");

                break;


            }


            case "SlideOut":
            {
                    

                ReadyAnim.Play("SlideOut");

                break;


            }



        }





    }



}
