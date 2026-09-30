using UnityEngine;
using TMPro;

public class ToggleSettings : MonoBehaviour
{
    
     //---THIS SCRIPT HANDLES TOGGLE SETTINGS----//
    
     [SerializeField] private RectTransform backgroundRect;
     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //---FUNCTION TO HANDLE TOGGLE SETTINGS---//

    public void HandleToggleSettings()
    {
        
        //SCALES | ADJUST POSITION | AND KEEP IT ANCHORED//
        if (backgroundRect != null)
        {
            backgroundRect.localScale = new Vector3(0.7453667f, 0.7453667f, 0.7453667f);
            backgroundRect.anchoredPosition = Vector2.zero;
            backgroundRect.localPosition = Vector3.zero;
        }





    }




}
