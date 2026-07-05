using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    // THIS SCRIPT IS FOR PLAYER MOVEMENT //

    [SerializeField] private PlayerInput PI;
    [SerializeField] public Shooting ShootBehavior;

    [Header("Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float RotationSpeed;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rb;

    [Header("GameObjects")]
    [SerializeField] public GameObject BulletReference = null;

    [Header("Bools")]
    [SerializeField] private bool CanShowPickUpPrompt = false;
    [SerializeField] private bool CanPickup = false;

    [Header("Transforms")]
    [SerializeField] private Transform BulletPos;

    private Vector2 MoveVector;
    private Vector2 LookVector;
    private Vector3 Direction;

    // This will securely hold the child canvas that spawned with this player
    private GameObject ButtonCanvas;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        PI = GetComponent<PlayerInput>();
        
        // Find the canvas even if it starts hidden/deactivated inside the prefab
        FindButtonCanvas();
    }

    void Start()
    {
        // Double-check check in Start() in case Awake() fired too early during instantiation
        if (ButtonCanvas == null)
        {
            FindButtonCanvas();
        }
    }

    void OnEnable()
    {
        if (PI == null)
        {
            PI = GetComponent<PlayerInput>();
        }


        PI.actions["PickUp"].performed += OnPickupTriggered;
        PI.actions["Shoot"].performed += OnShootTriggered;
    }

    void OnDisable()
    {
       if(PI == null)
        {
            
            return;

        }

        PI.actions["PickUp"].performed -= OnPickupTriggered;
        PI.actions["Shoot"].performed -= OnShootTriggered;

    }


    private void FindButtonCanvas()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            // Check if the child's name matches your button canvas exactly
            if (child.name == "Button Canvas")
            {
                ButtonCanvas = child.gameObject;
                ButtonCanvas.SetActive(false); // Force it to start hidden
                Debug.Log("🎯 SUCCESS: Located the exact 'Button Canvas' GameObject!");
                return; // Stop searching and exit the method immediately
            }
        }
        
        //  FIXED: Moved OUTSIDE the loop. This only runs if the loop finished completely and found nothing!
        Debug.LogError("❌ ERROR: Checked all children but could not find a GameObject named exactly 'Button Canvas'!");
    }



    void Update()
    {
        MoveVector = PI.actions["Move"].ReadValue<Vector2>();
        LookVector = PI.actions["Look"].ReadValue<Vector2>();

       
        float yRotation = LookVector.x * RotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * yRotation);

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Direction = (camRight * MoveVector.x + camForward * MoveVector.y).normalized;

        ShowPickupButton(ButtonCanvas);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(Direction.x * speed, rb.linearVelocity.y, Direction.z * speed);
    }

    private void OnPickupTriggered(InputAction.CallbackContext context)
    {
        PickupBullet(BulletReference);
    }

    private void OnShootTriggered(InputAction.CallbackContext context)
    {
        if (ShootBehavior != null)
        {
            ShootBehavior.Shoot();
        }
    }

    private void ShowPickupButton(GameObject Canvas)
    {
        if (Canvas == null) return; 

        if (Canvas.activeSelf != CanShowPickUpPrompt)
        {
            Canvas.SetActive(CanShowPickUpPrompt);
        }
    }

    private void PickupBullet(GameObject Reference)
    {
        if (CanPickup && Reference != null)
        {
            Reference.transform.position = BulletPos.position;
            Reference.transform.SetParent(BulletPos);
            Reference.transform.localRotation = Quaternion.Euler(84.815f, 0f, 0f);
            Reference.transform.localScale = new Vector3(0.1563195f, 0.152701f, 0.06677458f);

            if (Reference.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }

            if (Reference.TryGetComponent<Rigidbody>(out Rigidbody bulletRb))
            {
                bulletRb.isKinematic = true;
            }

            CanPickup = false;
            CanShowPickUpPrompt = false;
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Bullet"))
        {
            CanShowPickUpPrompt = true;
            CanPickup = true;
            BulletReference = Other.gameObject;
            
            if (ButtonCanvas != null)
            {
                ButtonCanvas.SetActive(true);
            }
            else
            {
               
                FindButtonCanvas();
                if (ButtonCanvas != null) ButtonCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Bullet"))
        {
            CanShowPickUpPrompt = false;
            CanPickup = false;
            BulletReference = null;

            if (ButtonCanvas != null)
            {
                ButtonCanvas.SetActive(false);
            }
        }
    }

    // SAFEGUARD: This blocks external scripts from accidentally wiping out your internal child canvas
    public void AssignButtonCanvas(GameObject Canvas)
    {
        if (ButtonCanvas != null) 
        {
            Debug.LogWarning("⚠️ JoinManager tried to assign an external canvas, but we are safely using our own child canvas instead!");
            return; 
        }

        ButtonCanvas = Canvas;
        if (ButtonCanvas != null)
        {
            ButtonCanvas.SetActive(false);
        }
    }
}
