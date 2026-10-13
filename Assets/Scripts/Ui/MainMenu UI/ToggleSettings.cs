using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class ToggleSettings : MonoBehaviour
{
    
     //---THIS SCRIPT HANDLES TOGGLE SETTINGS----//
    
    [SerializeField] private RectTransform backgroundRect;

    [SerializeField] private TextMeshProUGUI PlayerText;


    private PlayerInput PiReference;
     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HandleToggleSettings(PiReference);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //---FUNCTION TO HANDLE TOGGLE SETTINGS---//

    public void HandleToggleSettings(PlayerInput Pi)
    {
        
        //SCALES | ADJUST POSITION | AND KEEP IT ANCHORED//
        if (backgroundRect != null)
        {
            backgroundRect.localScale = new Vector3(0.7453667f, 0.7453667f, 0.7453667f);
            backgroundRect.anchoredPosition = Vector2.zero;
            backgroundRect.localPosition = Vector3.zero;
        }



        //CHANGE TEXT//
        if(PlayerText != null)
        {
            
            PlayerText.text = "Player " + PiReference.playerIndex;



        }



    }



    //GET A REFERENCE TO PLAYERINPUT//
    public void RetrievePlayerInput(PlayerInput Pi)
    {
        

       PiReference = Pi;



    }




}
