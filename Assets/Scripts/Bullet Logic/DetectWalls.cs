using UnityEngine;

public class DetectWalls : MonoBehaviour
{

    //GETS THE RIGIDBODY OF THE BULLET//
    public Rigidbody rb;    

    //VAIRIABLE FOR SPEED OF THE BULLET//
    [SerializeField] private float bulletSpeed = 2f; 

    //THIS VARIIABLE SAVES THE SPEED AND DIRECTION OF THE BULLET//
    private Vector3 currentVelocity;
   
    //BOOL FOR TRACKING IF THE BULLET IS IN FILIGHT//
    public bool isFlying = false;


    //THIS VARIABLE IS TO REMEMBER WHO SHOT THE BULLET FIRST TO PREVENT DAMAGE//
    [SerializeField] public GameObject Owner;

  
    void Start()
    {
        //SAFETY CHECK: IF FORGOT TO ASSIGN THE RIGIDBODY, ASSIGN IT//
        if (rb == null)
        {   
        
        rb = GetComponent<Rigidbody>();
        
        }   
    
    }

    //THIS FUNCTION TAKES BOTH DIRECTION AND SPEED//
    //uUSE THIS FUNCTION IN THE SHOOTING SCRIPT//
    public void Launch(Vector3 shootDirection, float speed)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        //SETS THE SPEED OF THE BULLET OF YOUR CHOOSING//
        bulletSpeed = speed;
        
        isFlying = true;


        Owner.GetComponent<PlayerMovement>().HoldingBullet = false;
        
        //MAKES THE BULLET TRAVEL/MOVE IN A CONSISTENT WAY//

        //THIS TAKES: 

        //1) shootDirection.normalized -> THE DIRECTION THE GUN IS POINTING AND NORMALIZES IT (LENGTH OF 1)

        //2) MULTIPLIES THE DIRECTION BY BULLETSPEED TO MAKE IT SHOOT//
        
        //3) THIS LINE IS FOR THE INITIAL MOMENTUM OF THE BULLET -> MAKES BULLET FLY ON THE FIRST FRAME//
        rb.linearVelocity = shootDirection.normalized * bulletSpeed;
        
        //THIS MAKES THE BULLET'S TRANSFORM VISUALLY FACE IN THE NEW DIRECTION IT'S GOING//
        transform.forward = rb.linearVelocity.normalized;
    }



    //THIS FUNCTION IS FOR TURNING OFF/DISABLING FLIGHT//

    public void StopBulletMotion()
    {
        isFlying = false;
        
        if (rb == null) rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            
            rb.angularVelocity = Vector3.zero;
            
            rb.isKinematic = true; 
        }

        Debug.Log("STOP!");

    }

    void FixedUpdate()
    {

        //IF THE BULLET ISN'T FLYING OR IN MOTION, SKIP HERE AND STOP (RETRURN)
        if (!isFlying) return;

        //THIS KEEPS THE BULLET MOVING IN FLIGHT IN THE DIRECTION ITS MOVING//
        rb.linearVelocity = rb.linearVelocity.normalized * bulletSpeed;
        
        // IS THE BULLET MOVING ENOUGH (CAN BE NOT MOVING AND WITH ZERO VELOCITY)//
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
           // Match visual rotation smoothly to the current flight trajectory
            transform.forward = rb.linearVelocity.normalized;
        }
        
        //SAVES THE VELOITY BEOFRE THE FRAME IT COLLIDES WITH THE WALL//
        currentVelocity = rb.linearVelocity;
    }


    //ONCOLLISIONENTER FUNCTION//
    void OnCollisionEnter(Collision collision)
    {
        if (!isFlying) return;

        //CHECKS IF THE BULLET COLLIDES WITH THE WALL//
        if (collision.gameObject.CompareTag("Walls"))
        {

            //THIS GRABS DATA ABOUT THE FIRST CONTACT POINT ON THE SURFACE//
            
            //COLLISION.CONTAXT -> IS A LIST OF TOUCHPOINTS//
            ContactPoint contact = collision.contacts[0];
           
            //CONTACT.NORMAL IS THE VECTOR/ARROW POINTING OUT OF THE SURFACE//
            Vector3 reflectedDirection = Vector3.Reflect(currentVelocity, contact.normal);

            //BOUNCE OFF AT A CERTAIN SPEED IN THAT DIRECTION//
            rb.linearVelocity = reflectedDirection.normalized * bulletSpeed;

            Owner = null;
        }

        // CHECK IF BULLET COLLIDED WITH PLAYER //
        if (collision.gameObject.CompareTag("Player") && isFlying)
        {

                PlayerMovement Obj = collision.gameObject.GetComponent<PlayerMovement>();
                
                 // IF THIS IS THE PERSON WITH THE BULLET, DON'T MAKE IT COUNT TOWARDS DAMAGE
                 if(Obj == null) return;

                 
              
                 // GET THE SCRIPT OF THE PLAYER WHO SHOT THE BULLET
                 if (Owner != null)
                 {
                    
                    PlayerMovement Shooter = collision.gameObject.GetComponent<PlayerMovement>();

                            //IF THE SHOOTER TEAM AND TEAMAMATE ARE ON THE SAME TEAM DON'T MAKE TEAMMATE TAKE DAMAGE (FRIENDLY FIRE OFF)//
                            if(Shoot != null && Shooter.MyTeam == Obj.MyTeam)
                             {
                    
                                return;


                             }


                 }


           
                 
                 //MAKES SURE DAMAGE ISN'T APPLIED ON DAMAGE//
                 if (Obj.CanCatch && Obj.BulletReference == gameObject && Obj.CatchingPressed)
                 {
                     return; 
                 }

                
                //IF THE 
                if (collision.gameObject == Owner)
                {
                 return; 
                }

                // STOP ALL FLIGHT LOGIC AND FORWARD MOMENTUM IMMEDIATELY
                isFlying = false; 
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero; 

                // ALLOW GRAVITY TO TAKE OVER SO IT DROPS TO THE GROUND
                rb.isKinematic = false; 
                rb.useGravity = true;

    
                //WHEN ISTRIGGER IS TURN OFF -> COLLIDER BECOMES SOLID WALL//

                //WHEN ISTRIGGER IS TURN ON -> SWITCHES FROM ONCOLLISIONENTER TO ONTRIGGERENETER

                // ONTRIGGERENTER WDETECTS OBJECTS THAT ARE SET AS TRIGGERS//
                if (TryGetComponent<Collider>(out Collider myCollider))
                {

                //THIS WORKS WITH THE PICKUP MECHANIC:
               
                // 1) WHEN THE BULLET COLLIDES WITH THE PLAYER, THE PICKUP PROMPT TRIGGERS//
                // 2) PLAYER/ENEMY PICKS IT UP AND SHOOTS AGAIN//
                 myCollider.isTrigger = true; 
               
                }

                HealthBar HB = collision.gameObject.GetComponentInChildren<HealthBar>();


                // DECREASES THE HEALTH OF THE PLAYER HIT BY THE BULLET // 
                if (HB != null)
                {       
                
                HB.DecreaseHealthVisually(50f);
    
                }

   
            // Destroy(gameObject, 3f);
        }

    }


    //FUNCTION TO SET OWNER//
    public void SetOwner(GameObject shooter)
    {

       Owner = shooter;



    }
}
