using UnityEngine;
using UnityEngine.InputSystem; //INCLUDE THIS NAMESPACE FOR INPUTACTION//
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
   
    private Vector2 Movement;

    [SerializeField] private float CursorSpeed;

    [SerializeField] private RectTransform CursorRect;

   
    void Awake()
    {

        CursorRect = GetComponent<RectTransform>();  


    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 Direction = (Vector3.right * Movement.x + Vector3.up * Movement.y).normalized;


        CursorRect.anchoredPosition += (Vector2)Direction * CursorSpeed * Time.deltaTime;
    }


    public void OnNavigate(InputAction.CallbackContext context)
    {
       
        
        Movement = context.ReadValue<Vector2>();


    }
}
