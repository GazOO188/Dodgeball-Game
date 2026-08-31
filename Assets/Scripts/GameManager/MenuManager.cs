using UnityEngine;
using UnityEngine.InputSystem; //INCLUDE THIS NAMESPACE FOR INPUTACTION//
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
   
    public static MenuManager MenuJoinInstance{ get; private set;}

    [Header("UI Elements")]
    [SerializeField] private GameObject TogglePrefab;
    [SerializeField] private Transform UICanvas;



    [Header("Locations For UI")]
    [SerializeField] private List<Transform> ToggleLocations = new List<Transform>();
    
    
   
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
        
        
        if(playerInput != null && UICanvas != null)
        {
            
            playerInput.transform.SetParent(UICanvas, false);


        }


        if(PlayerIndex < ToggleLocations.Count)
        {

           GameObject ToggleForPlayer = Instantiate(TogglePrefab, ToggleLocations[PlayerIndex]);


           ToggleForPlayer.transform.SetParent(UICanvas, false);
       


        }

       






    }
}
