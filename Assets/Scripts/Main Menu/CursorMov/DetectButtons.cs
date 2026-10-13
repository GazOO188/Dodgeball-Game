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
     //POINTEREVENTDATA IS A CUSTOM DATA TYPE FOR A THING THAT BEHAVES AS A POINTING DEVICE//
     
     //THIS DATA TYPE IS A PACKAGE OF DATA THAT MIMICS A MOUSE POINTER, THAT TELLS WHERE THE CURSOR IS HOVERING OVER//
     private PointerEventData pointerData;

     //THIS STORED ALL THE UI ELEMENTS THAT THE UI CURSOR TOUCHES//
     private List<RaycastResult> raycastResults = new List<RaycastResult>();


     [Header("Bools")]

     [SerializeField] public bool IsonRed = false; 
     [SerializeField] public bool IsonBlue = false;

   
   

     
   
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
       //FUNCTION TO DETECT UI//
    private void DetectUIFromCursor()
    {
        //EXITS OR STOP IF ONE OF THESE IS MISSING OR DOSEN'T EXIST//
        if(pointerData == null || cursorRect == null || EventSystem.current == null)
        {
            return;
        }

        //CONVERTS THE CURSOR'S 3D LOCATION TO A 2D SCREEN COORDINATE//
        pointerData.position = RectTransformUtility.WorldToScreenPoint(null, cursorRect.position);
        
        raycastResults.Clear();

        //THIS CASTS A RAY FROM THE UI CURSOR
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        //EVERY FRAME EMPTIES OUT THE BUTTON THIS FRAME BUTTON -> SET TO NULL//
        Button ButtonThisFrame = null;

        //EVERY FRAME THIS SETS ISONRED && ISONBLUE TO FALSE
        IsonRed = false;
        IsonBlue = false;
       
        //CHECKS EVERY RAYCAST HIT//
        foreach(RaycastResult hit in raycastResults)
        {
            //GET BUTTON COMPONENT//
            Button foundButton = hit.gameObject.GetComponentInParent<Button>();

            if(foundButton != null)
            {
                ButtonThisFrame = foundButton;
              
                if(foundButton.gameObject.name == "Red Team Button")
                {
                    IsonRed = true;
                    Debug.Log("Red");
                }
                else if(foundButton.gameObject.name == "Blue Team Button")
                {
                    IsonBlue = true;
                    Debug.Log("BLUE");
                }
                else if(foundButton.gameObject.name == "Ready Button")
                {
                    Debug.Log("Start detected");
                }

               
                break;
            }
        } 


        //THIS IS SAYING IF THE BUTTON THIS FRAME IS A DIFFERENT BUTTON THEN CURRENTBUTTON//
        if(ButtonThisFrame != CurrentButton)
        {
          

            //THIS IF STATEMENT EXITS THE HIGHLIGHTING ANIMATION//
            if (CurrentButton != null)
            {
                CurrentButton.OnPointerExit(pointerData); 
            }

          

            //THIS IF STATEMENT TRIGGERS THE HIGHLIGHTING ANIMATION ON THE READY BUTTON//

            // THIS TRIGGERS BECAUSE WE EMPTY THE CURRENT BUTTON THE NEXT FRAME//
            if (ButtonThisFrame != null)
            {
                ButtonThisFrame.OnPointerEnter(pointerData);
            }    
        }

        //STORE THE BUTTON FOR NEXT FRAME CHECK//
        CurrentButton = ButtonThisFrame;
    }


 

}
