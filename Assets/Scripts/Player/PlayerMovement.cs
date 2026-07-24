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
    [SerializeField] private float JumpForce;
    [SerializeField] private float gravityForce;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rb;

    [Header("GameObjects")]
    [SerializeField] public GameObject BulletReference = null;

    [Header("Bools")]
    [SerializeField] private bool CanShowPickUpPrompt = false;
    [SerializeField] private bool CanPickup = false;
    [SerializeField] public bool HoldingBullet = false;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool CanJump = false;
    [SerializeField] public bool CanCatch = false;
    [SerializeField] public bool CatchingPressed = false;


    [Header("Transforms")]
    [SerializeField] private Transform BulletPos;
    [SerializeField] private Transform GroundPos;

   
    [Header("Layers")]
    [SerializeField] private LayerMask GroundLayer;


    [Header("SphereCast Settings")]
    [SerializeField] private float catchRadius = 2.0f;
    [SerializeField] private float SphereOffset;
    [SerializeField] private Vector3 VectorOffset;  //OFFSET FOR SPHERECAST//



    [Header("Variables for Timing Catch")]
    [SerializeField] private float CatchTimer = 0.5f;


    public JoinManager.Teams MyTeam = JoinManager.Teams.None;
    
    
    //VECTORS TO USE WITH NEW INPUT//
    private Vector2 MoveVector;
   
    private Vector2 LookVector;
   
    private Vector2 DashDir;
   
    private Vector3 Direction;

    // HOLDS A REFERENCE TO THE BUTTONCANVAS//
    private GameObject ButtonCanvas;


    private GameObject GroundPositionCheck;




    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        PI = GetComponent<PlayerInput>();
        
        // Find the canvas even if it starts hidden/deactivated inside the prefab
        FindButtonCanvas("Button Canvas");

        FindGroundPosition();
    }

    void Start()
    {
        
        
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
        PI.actions["Jump"].performed += OnJumpTriggered;
        PI.actions["Catch"].performed += OnCatchTriggered;
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
        PI.actions["Jump"].performed -= OnJumpTriggered;
        PI.actions["Catch"].performed -= OnCatchTriggered;

    }


    //THIS FUNCTION IS FOR FINDING THE BUTTON CANVAS//
    private void FindButtonCanvas(string ChildName)
    {

        //THIS MAKES AN ARRAY THAT HOLDS MULTIPLE TRANSFORM COMPONENTS/

        //A FOREACH LOOP WHICH LOOKS AT ALL THE TRANSFORM CHILDREN ON THE OBJECT//
        foreach (Transform child in GetComponentsInChildren<Transform>(true))

        {
            // CHECKS IF THE CHILD'S NAME MATCHES BUTTON CANVAS//
            if (child.name == ChildName)
            {
                ButtonCanvas = child.gameObject;

                //MAKES THE BUTTON CANAVS HIDDEN//
                ButtonCanvas.SetActive(false); 

                Debug.Log("Located the Button Canvas GameObject");

                //STOPS SEARCHING AND EXITS THE FUNCTION//
                return; 
            }
        }
         
        Debug.LogError("Couldn't find Button Canvas");
    }


    //THIS FUNCTION IS FOR FINDING THE GROUNDPOSITION GAMEOBJECT//
    private void FindGroundPosition()
    {
      

        foreach(Transform children in GetComponentsInChildren<Transform>(true))
        {
            
            if(children.name == "GroundPosition")
            {
                
                 GroundPos = children.gameObject.transform;
                 
                 
                 return;


            }


        }


    }



    void Update()
    {


        //STORES VECTOR2 VALUES FOR MOVE AND LOOK VECTOR2//

        //READS THE LEFT JOYSTICK AND SAVES INPUTS INTO MOVEVECTOR//
        MoveVector = PI.actions["Move"].ReadValue<Vector2>();

        //READS THE RIGHT JOYSTICK FOR LOOKING AROUND//
        LookVector = PI.actions["Look"].ReadValue<Vector2>();


       
  
        //BOOLEAN FOR JUMPING//
        CanJump = Physics.CheckSphere(GroundPos.position, 1f, GroundLayer);



        //CALCULATES HOW MUCH THE PLAYER SHOULD TURN THIS 
        float yRotation = LookVector.x * RotationSpeed * Time.deltaTime;

        //THIS ACTUALLY TURNS THE PLAYER SPINS THE CHARACTER LEFT AND RIGHT AROUNF THE Y-AXIS//
        transform.Rotate(Vector3.up * yRotation);

        //CAM FORWARD IS ASKING WHICH WAY YOU ARE LOOKING RIGHT NOW/FACING//
        Vector3 camForward = Camera.main.transform.forward;

        //CAM RIGHT IS ASKING WHICH WAY IS THE RIGHT SIDE LOOKING AT//
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        //THIS LINE CREATES A NEW DIRECTION VECTOR (CAMERA RELATIVE MOVEMENT):

        // 1) WHEN PLAYER PUSHES FORWARD ON JOYSTICK -> MOVE FORWARD RELATIVE TO CAMERA OR IS LOOKING//

        // 2) IF YOU PUSH RIGHT ON THE JOYSTICK, MOVES RIGHT RELATIVE TO THE CAMERA//
        Direction = (camRight * MoveVector.x + camForward * MoveVector.y).normalized;

        ShowPickupButton(ButtonCanvas);

        CastSphere();

        TimerCatch();

    }



       // DRAWS THE CATCHING ZONE VISUALLY INSIDE THE UNITY SCENE VIEW //
    private void OnDrawGizmos()
    {
        // 1. Choose a clear color (Green)
        Gizmos.color = Color.green;

        // 2. Define the exact center position matching your CastSphere function (5f offset)
        Vector3 sphereCenter = transform.position + (transform.forward * SphereOffset) + VectorOffset;

        // 3. Draw the single catching circle sphere
        Gizmos.DrawWireSphere(sphereCenter, catchRadius);
    }

    void FixedUpdate()
    {
        // THIS ACTS A SAEFTY SO THAT MOVEMENT DOES NOT CANCEL THE DASH VELOCITY//
        if (isDashing) return;

        rb.linearVelocity = new Vector3(Direction.x * speed, rb.linearVelocity.y, Direction.z * speed);



        //THIS IS TO APPLY GRAVITY WHEN THE PLAYER IS FALLING TO THE GROUND//
        if(rb.linearVelocity.y < 0)
        {
        
        //APPLYS GRAVITY//
        rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityForce - 1) * Time.fixedDeltaTime;

        }


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


    // NEW INPUT FUNCTION: FOR JUMPING ON BUTTON PRESS//
    private void OnJumpTriggered(InputAction.CallbackContext context)
    {

        if (CanJump)
        {
            
            //THIS MAKES THE PLAYER JUMP//
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z);

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


  
  
    // CATCHING LOGIC W/ NEW INPUT SYSTEM //
    private void OnCatchTriggered(InputAction.CallbackContext context)
    {

        if(!CanCatch) return;
        
        if (CanCatch && BulletReference != null && !HoldingBullet)
        {

             CatchingPressed = true;
            
            // GETS THE DETECT WALLS SCRIPT AND STOPS THE BULLET FROM TRAVELING//
            if (BulletReference.TryGetComponent<DetectWalls>(out DetectWalls bulletScript))
            {

                //CALLS THE STOP BULLET FUNCTION
                bulletScript.StopBulletMotion();
            }

            //THIS IS TO CATCH THE BULLET AND PUT IT IN THE PLAYERS HANDS//
            BulletReference.transform.position = BulletPos.position;
            BulletReference.transform.SetParent(BulletPos);
            BulletReference.transform.localRotation = Quaternion.Euler(84.815f, 0f, 0f);
            BulletReference.transform.localScale = new Vector3(0.1563195f, 0.152701f, 0.06677458f);

            // DISABLING THE COLLIDER SO IT DOSENT CAUSE PROBLEMS
            if (BulletReference.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }

            //GETS RIGIDBODY AND PREVENTS SPIINING AND MOVMEMENT TO MAKE SURE//
            if (BulletReference.TryGetComponent<Rigidbody>(out Rigidbody bulletRb))
            {
                bulletRb.linearVelocity = Vector3.zero;
                bulletRb.angularVelocity = Vector3.zero;
                bulletRb.isKinematic = true;
            }

            //RESETTING VARIABLES + MARK THE BULLET AS BEING HELD CURRENTLYS//
            CanCatch = false;
            CanPickup = false;
            CanShowPickUpPrompt = false;
            HoldingBullet = true;
            CatchTimer = 0f;

            CatchingPressed = false;
        }
    }


    // THIS FUNCTION IS FOR SCANNING SPACE CONTINUOUSLY TO LOCATE THE BULLET //
    private void CastSphere()
    {
       
        if (BulletReference != null) return;

        Vector3 sphereCenter = transform.position + (transform.forward * SphereOffset);
       
        // QueryTriggerInteraction.Collide forces the scan to see the bullet even if it turned into a ghost trigger
        Collider[] hits = Physics.OverlapSphere(sphereCenter, catchRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);

        bool foundBullet = false;

        foreach(Collider hit in hits)
        {
            if(hit.gameObject == gameObject)
            {
               continue;
            }

            if(hit.TryGetComponent<DetectWalls>(out DetectWalls bullet))
            {
                
                
           
                if(!bullet.isFlying)
                {
                   
                    continue;  

                }

                
                 if(bullet.Owner == gameObject) continue;



                Debug.Log("Bullet target indexed into inventory tracking buffer: " + hit.gameObject.name);


                // Save references cleanly ahead of time for OnCatchTriggered to consume instantly
                BulletReference = bullet.gameObject;

                CanCatch = true;

                foundBullet = true;


                // SET THE TIMER: Reset your real script variable to its max window time (e.g., 0.5f)
                CatchTimer = 0.5f; 
                break; 
            }
        }

    }


    //FUNNCTION TO TIME THE CATCH//
    private void TimerCatch()
    {
        

        if(CanCatch && CatchTimer > 0f)
        {
            
            CatchTimer -= Time.deltaTime;



            if(CatchTimer <= 0f)
            {
                
             
             Debug.Log("Window over");

             CanCatch = false; 
             BulletReference = null;
             CatchTimer = 0f;

            }


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

            //CUSTOM SETTINGS FOR THE ROTATION AND SCALE//
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

        
            HoldingBullet = true;
            CanCatch = false;
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
               
                FindButtonCanvas("Button Canvas");
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
            Debug.LogWarning("JoinManager tried to assign an external canvas");
            return; 
        }

        ButtonCanvas = Canvas;
       
        if (ButtonCanvas != null)
        {
            ButtonCanvas.SetActive(false);
        }
    }
}
