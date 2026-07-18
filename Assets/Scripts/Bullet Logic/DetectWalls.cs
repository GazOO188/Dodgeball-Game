using UnityEngine;

public class DetectWalls : MonoBehaviour
{

    //GETS THE RIGIDBODY OF THE BULLET//
    public Rigidbody rb;    

    //VAIRIABLE FOR SPEED OF THE BULLET//
    private float bulletSpeed = 10f; 

    //THIS VARIIABLE SAVES THE SPEED AND DIRECTION OF THE BULLET//
    private Vector3 currentVelocity;
   
    //BOOL FOR TRACKING IF THE BULLET IS IN FILIGHT//
    private bool isFlying = false;


    //THIS VARIABLE IS TO REMEMBER WHO SHOT THE BULLET FIRST TO PREVENT DAMAGE//
    [SerializeField] private GameObject Owner;


  
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
        
        //MAKES THE BULLET TRAVEL/MOVE IN A CONSISTENT WAY//

        //THIS TAKES: 

        //1) shootDirection.normalized -> THE DIRECTION THE GUN IS POINTING AND NORMALIZES IT (LENGTH OF 1)

        //2) MULTIPLIES THE DIRECTION BY BULLETSPEED TO MAKE IT SHOOT//
        
        //3) THIS LINE IS FOR THE INITIAL MOMENTUM OF THE BULLET -> MAKES BULLET FLY ON THE FIRST FRAME//
        rb.linearVelocity = shootDirection.normalized * bulletSpeed;
        
        //THIS MAKES THE BULLET'S TRANSFORM VISUALLY FACE IN THE NEW DIRECTION IT'S GOING//
        transform.forward = rb.linearVelocity.normalized;
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
        
        //SAVES THE VVELOITY BEOFRE THE FRAME IT COLLIDES WITH THE WALL//
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
        }

            // CHECK IF BULLET COLLIDED WITH PLAYER //
        if (collision.gameObject.CompareTag("Player") && isFlying)
        {
              //IF THIS IS THE PERSON WITH THE BULLET, DON'T MAKE IT COUNT TOWARDS DAMAGE
              if(collision.gameObject == Owner)
             {
                //EXITS/ DOSEN'T SPPLY DAMAGE TO THE OWNER OF THE BULLET//
                return;
             }
       
                
                 isFlying = false;
                 rb.linearVelocity = Vector3.zero; 
                 currentVelocity = Vector3.zero; 
       
                 HealthBar HB = collision.gameObject.GetComponentInChildren<HealthBar>();


               //DECREASES THE HEALTH OF THE PLAYER HIT BY THE BULLET//
               if (HB != null)
               {       
                    
                 HB.DecreaseHealthVisually(50f);
                   
               }

           //  Destroy(gameObject);
    
        }   

    }


    //FUNCTION TO SET OWNER//
    public void SetOwner(GameObject shooter)
    {

       Owner = shooter;



    }
}
