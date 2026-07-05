using UnityEngine;

public class DetectWalls : MonoBehaviour
{
    public Rigidbody rb;    
    private float bulletSpeed = 10f; 

    private Vector3 currentVelocity;
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

        bulletSpeed = speed;
        
        isFlying = true;
        
        //MAKES THE BULLET TRAVEL/MOVE IN A CONSISTENT WAY//
        rb.linearVelocity = shootDirection.normalized * bulletSpeed;
        
        //THIS MAKES THE BULLET'S TRANSFORM VISUALLY FACE IN THE NEW DIRECTION IT'S GOING//
        transform.forward = rb.linearVelocity.normalized;
    }

    void FixedUpdate()
    {

        //IF THE BULLET ISN'T FLYING OR IN MOTION, SKIP HERE AND STOP (RETRURN)
        if (!isFlying) return;

        // Maintain exact target speed overriding any physics drag
        rb.linearVelocity = rb.linearVelocity.normalized * bulletSpeed;
        
        // Match visual rotation smoothly to the current flight trajectory
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            transform.forward = rb.linearVelocity.normalized;
        }
        
        //SAVES THE VVELOITY
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
                 // IF THIS IS THE PERSON WITH THE BULLET, DON'T MAKE IT COUNT TOWARDS DAMAGE
              if(collision.gameObject == Owner)
             {
                return;
             }
       
                 // 🎯 FIXES: Turn off flying, clear rigidbodies, and use Vector3.zero
                 isFlying = false;
                 rb.linearVelocity = Vector3.zero; 
                 currentVelocity = Vector3.zero; 
       
                 HealthBar HB = collision.gameObject.GetComponentInChildren<HealthBar>();

               if (HB != null)
               {       
                    
                 HB.DecreaseHealthVisually(50f);
                   
               }

            // 🎯 PROFESSIONAL TOUCH: Destroy the bullet asset on impact so it cleans up nicely
           //  Destroy(gameObject);
    
        }   

    }


    //FUNCTION TO SET OWNER//

    public void SetOwner(GameObject shooter)
    {

       Owner = shooter;



    }
}
