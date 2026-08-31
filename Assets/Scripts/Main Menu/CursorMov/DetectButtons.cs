using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DetectButtons : MonoBehaviour
{
    //THIS SCRIPT IS FOR DETECTING WHAT THE UI CURSOR INTERACTS WITH SUCH AS BUTTONS//

    //THINGS NEEDED:

        // 1) EVENTSYSTEM: MANAGES ALL INPUT AND RAYCASTING IN THE UI

        // 2) POINTER EVENT DATA: A DATA CONTAINER THAT HOLDS PHYSICAL INFOMRATION ABOUT A POINTER DEVICE (COMPUTER MOUSE, VR CONTROLLER, TOUCH SCREEN, ETC)

            //2A) POINTER: A POINTER IS WHATEVER TOOL THE PLAYER USES TO POINT AT ANY UI ELEMENTS (UI CURSOR, MOUSE, FINGER FOR IPAD)

        // 3) RAYCAST RESULT: A DATA TYPE LIKE (INT/FLOAT) THAT BEHAVES LIKE A DATA PACKET WHICH HAS ACCESS TO THINGS LIKE:

            // 3A) hit.gameobject -> THE THING THE RAYCAST HITS//

     [Header("Ui Elements")]
     [SerializeField] private RectTransform cursorRect;
     //STORES THE BUTTON THAT THE CURSOR IS ON//
     [SerializeField] public Button CurrentButton;
     
     //THIS IS INFORMATION ABOUT WHAT THE CURSOR IS DOING/
     // POINTEREVENTDATA IS A CUSTOM DATA TYPE FOR A THING THAT BEHAVES AS A POINTING DEVICE//
     private PointerEventData pointerData;

     //THIS STORED ALL THE UI ELEMENTS THAT THE UI CURSOR TOUCHES//
     private List<RaycastResult> raycastResults = new List<RaycastResult>();

   
   

     
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        //SAFETY CHECK: MAKE SURE THE EVENTSYSTEM GAMEOBJECT EXISTS//
        if(EventSystem.current != null)
        {
            
            //THIS LINE CREATES A NEW INFORMAATION PACKAGE FOR THE CUSTOME CURSOR//
            pointerData = new PointerEventData(EventSystem.current);


        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectUIFromCursor();
        
    }


    //FUNCTION TO DETECT UI//
    private void DetectUIFromCursor()
    {
        
        //EXITS OR STOP IF ONE OF THESE IS MISSING OR DOSEN'T EXIST//
        if(pointerData == null || cursorRect == null || EventSystem.current == null)
        {
            
            return;
        }

        //WorldToScreenPoint -> TAKES A POINT IN THE GAME WORLD AND TURN IT INTO A POINT ON THE SCREEN//
        //THIS LINE READS THE POSITION OF THE UI CURSOR ON THE SCREEN AND TURNS IT INTO A SCREEN POINT//
        pointerData.position = RectTransformUtility.WorldToScreenPoint(null, cursorRect.position);

        
        raycastResults.Clear();

        
        //THIS CASTS A RAY FROM THE UI CURSOR AND HITS ALL ITEMS OR THINGS THAT ARE IN THE WAY OF THE RAYCAST
        EventSystem.current.RaycastAll(pointerData, raycastResults);


        //EVERY FRAME EMPTIES OUT THE BUTTON THIS FRAME BUTTON -> SET TO NULL//
        Button ButtonThisFrame = null;
       

        //CHECKS EVVERY RAYCAST HIT//
        foreach(RaycastResult hit in raycastResults)
        {
            
            //IF A GAMEOBJECT NAMED "RED TEAM BUTTON" IS DETECTED?/
            if(hit.gameObject.name == "Red Team Button")
            {
                
                Debug.Log("Found Item");

                //GET THE BUTTON ON THE GAMEOBJECT//
                ButtonThisFrame = hit.gameObject.GetComponent<Button>();

                break;

            }



        }

        //STORE THE BUTTON//
        CurrentButton = ButtonThisFrame;

    }


 

}
