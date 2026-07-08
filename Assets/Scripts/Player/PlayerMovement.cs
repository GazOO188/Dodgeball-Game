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
    [SerializeField] private float DashForce;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rb;

    [Header("GameObjects")]
    [SerializeField] public GameObject BulletReference = null;

    [Header("Bools")]
    [SerializeField] private bool CanShowPickUpPrompt = false;
    [SerializeField] private bool CanPickup = false;
    [SerializeField] private bool isDashing = false;

    [Header("Transforms")]
    [SerializeField] private Transform BulletPos;


    //VECTORS TO USE WITH NEW INPUT//
    private Vector2 MoveVector;
   
    private Vector2 LookVector;
   
    private Vector2 DashDir;
   
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
        PI.actions["Dash"].performed += OnDashTriggered;
    }

    void OnDisable()
    {
       if(PI == null)
        {
            
            return;

        }

        PI.actions["PickUp"].performed -= OnPickupTriggered;
        PI.actions["Shoot"].performed -= OnShootTriggered;
        PI.actions["Dash"].performed -= OnDashTriggered;

    }


    private void FindButtonCanvas()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            // CHECKS IF THE CHILD'S NAME MATCHES BUTTON CANVAS//
            if (child.name == "Button Canvas")
            {
                ButtonCanvas = child.gameObject;

                //MAKES THE BUTTON CANAVS HIDDEN//
                ButtonCanvas.SetActive(false); 

                Debug.Log("Located the exact 'Button Canvas' GameObject");

                //STOPS SEARCHING AND EXITS THE FUNCTION//
                return; 
            }
        }
        
        
        Debug.LogError("Couldn't find a GameObject named exactly 'Button Canvas'!");
    }



    void Update()
    {
        //STORES VECTOR2 VALUES FOR MOVE AND LOOK VECTOR2//
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

        //THIS LINE CREATES A NEW DIRECTION VECTOR (CAMERA RELATIVE MOVEMENT):
        
        // 1) WHEN PLAYER PUSHES FORWARD ON JOYSTICK -> MOVE FORWARD RELATIVE TO CAMERA OR IS LOOKING//
        Direction = (camRight * MoveVector.x + camForward * MoveVector.y).normalized;

        ShowPickupButton(ButtonCanvas);
    }

    void FixedUpdate()
    {
        // THIS ACTS A SAEFTY SO THAT MOVEMENT DOES NOT CANCEL THE DASH VELOCITY//
        if (isDashing) return;

        rb.linearVelocity = new Vector3(Direction.x * speed, rb.linearVelocity.y, Direction.z * speed);
    }


    //NEW INPUT FUNCTION: FOR PICKING UP BULLET ON THE BUTTON X//
    private void OnPickupTriggered(InputAction.CallbackContext context)
    {
        PickupBullet(BulletReference);
    }

    //NEW INPUT FUNCTION: FOR PICKING UP BULLET ON THE BUTTON RIGHT TRIGGER//
    private void OnShootTriggered(InputAction.CallbackContext context)
    {
        if (ShootBehavior != null)
        {
            ShootBehavior.Shoot();
        }
    }

    //DASHING LOGIC W/ INPUT SYSTEM//
    private void OnDashTriggered(InputAction.CallbackContext context)
    {
        //ACTIVATES DASH//
        if (!isDashing)
        {
            ActivateDash();
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


    //FUNCTION FOR WHEN THE PLAYER PICKUP THE BULLET DOES A FEW THINGS:

    //1) MAKES THE BULLET GO TO THE BARREL OF THE GUN//

    //2) MAKES THE BULLET A LITTLE BIGGER//

    //3) MAKES THE BULLET FACE FORWARD//

    //4) DISABLES THE COLLIDER AND SETS ISKNEMATIC ON THE RIGIDBODY TO BE TRUE//
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


    //THIS FUNCTION IS FOR DASHING//

    private void ActivateDash()
    {

        //MARKS IS DAHSING BOOLEAN AS TRUE//
        isDashing = true;
       


        Vector3 dashVelocityDir = Direction;

        
        // If the player is standing completely still, default to dashing straight forward
        if (dashVelocityDir.sqrMagnitude == 0)
        {
            dashVelocityDir = transform.forward;
        }


        //CREATES A NEW VECTOR3 VELOCITY FOR THE DASHING//
        rb.linearVelocity = new Vector3(dashVelocityDir.x * DashForce, rb.linearVelocity.y, dashVelocityDir.z * DashForce);


        //DO INVOKE FOR A MINI COUOTUINE//
        Invoke(nameof(ResetDashState), 0.2f);
    }

    // THIS IS FOR RESETITING DASHING//
    private void ResetDashState()
    {
        isDashing = false;
    }

    
    
    //THIS IS FOR WHENN THE PLAYER IS IN THE TRIGGER ZONE OF THE BULLET AT THE START OF THE GAME//
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Bullet"))
        {
            CanShowPickUpPrompt = true;
            CanPickup = true;

            //SAVES THE BULLET THE PLAYER IS CURRENTLY TOUCHING//
            BulletReference = Other.gameObject;
            

            //SHOWS THE BUTTON PROMPT "E" TO PICKUP//
            if (ButtonCanvas != null)
            {
                ButtonCanvas.SetActive(true);
            }

            //SAFETY CHECKS//
            else
            {
               
                FindButtonCanvas();
                if (ButtonCanvas != null) ButtonCanvas.SetActive(true);
            }
        }
    }

    //THIS IS FOR WHEN THE PLAYER IS NO LONGER IN THE TRIGGER ZONE OF THE BULLET//
    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Bullet"))
        {
            CanShowPickUpPrompt = false;
            CanPickup = false;
            BulletReference = null;

            //MAKES THE BUTTON PROMPT NO LONGER VISIBLE//
            if (ButtonCanvas != null)
            {
                ButtonCanvas.SetActive(false);
            }
        }
    }

    // SAFETY CHECK AND ASSIGNS THE BUTTON CANVAS//
    public void AssignButtonCanvas(GameObject Canvas)
    {
        if (ButtonCanvas != null) 
        {
            Debug.LogWarning("JoinManager tried to assign an external canvas, but we are safely using our own child canvas instead!");
            return; 
        }

        ButtonCanvas = Canvas;
       
        if (ButtonCanvas != null)
        {
            ButtonCanvas.SetActive(false);
        }
    }
}
