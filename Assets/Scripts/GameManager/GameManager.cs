using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GLOBAL SHORTCUT/INSTANCE: THIS IS ASKING FOR THE GAMEMANAGER IN THE SCENE:

    //1) GAMEMANAGER.INSTANCE.HB -> EQUIVALENT OF FINDING GAMEMANAGER IN THE SCENE//
    public static GameManager Instance { get; private set; }

    [Header("Scene Canvas Link")]
    [SerializeField] private GameObject sceneButtonCanvas; 
    [SerializeField] private HealthBar HB;

    void Awake()
    {
        // Set up the singleton shortcut safely
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
