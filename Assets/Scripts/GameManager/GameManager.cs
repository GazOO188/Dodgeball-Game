using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 1. THE GLOBAL SHORTCUT: Any player prefab can see this instantly
    public static GameManager Instance { get; private set; }

    [Header("Scene Canvas Link")]
    [SerializeField] private GameObject sceneButtonCanvas; // Keep your 'button cnabs' dragged here
    [SerializeField] private HealthBar HB; // Keep your 'button cnabs' dragged here

    void Awake()
    {
        // 2. Set up the singleton shortcut safely
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //IF THERE IS ANOTHER INSTACNE DESTORY THE SECOND INSTANCE OF GAME MANAGER SO THERE'S ONLY ONE
            Destroy(gameObject);
        }
    }

   
    // This allows any newly spawned player to ask for the canvas reference
    //THIS FEEDS ANY GAMEOBJECT THE SCENEBUTTONCAVS LIKE: GAMEOBJECT CANVAS = REQUESTCANVAS();
    public GameObject RequestCanvas()
    {
        return sceneButtonCanvas;
    }

    
    public HealthBar RequestHealthBarScript()
    {
        return HB;
    }
}
