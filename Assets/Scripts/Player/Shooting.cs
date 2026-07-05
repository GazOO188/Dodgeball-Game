using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public PlayerMovement PM;

    [Header("Transforms")]
    [SerializeField] private Transform BulletPos;

    [Header("Variables")]
    [SerializeField] private float Bulletspeed = 20f;

    void Awake()
    {
        PM = GetComponent<PlayerMovement>();
    }

    public void Shoot()
    {
        // SAFE CHECK FIRST
        if (PM.BulletReference != null)
        {
            GameObject bulletObj = PM.BulletReference;

            // UNPARENT THE BULLET FROM PLAYER
            bulletObj.transform.SetParent(null);

            // GET THE RIGIDBODY AND COLLIDER
            Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
            Collider Col = bulletObj.GetComponentInChildren<Collider>(); 
            
            //IF THE COLLIDER EXISTS//
            if (Col != null)
            {
                Col.isTrigger = false; 
                Col.enabled = true;
            }

            //IF THE RIGIDBODY EXISTS//
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            //THIS GETS THE SHOOTING FINCTION ON THE BULLET TO LAUNCH IT//
            if (bulletObj.TryGetComponent<DetectWalls>(out DetectWalls bouncingBullet))
            {
                //SETS THE OWNER TO BE THE PLAYER HOLDING THE BULLET//
                bouncingBullet.SetOwner(gameObject);
                bouncingBullet.Launch(BulletPos.forward, Bulletspeed);
            }
            else
            {
                // Fallback physics launch if the script is missing
                if (rb != null) rb.linearVelocity = BulletPos.forward * Bulletspeed;
            }

            // CLEARS THE BULLET REFERENCE
            PM.BulletReference = null;
        }
        else
        {
            Debug.LogWarning("No bullet reference held to shoot!");
        }
    }
}
