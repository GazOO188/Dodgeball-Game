using UnityEngine;

public class BulletCount : MonoBehaviour
{
    //THIS SCRIPT IS FOR SPAWNING THE BULLETS (DODGEBALLS)


    [Header("Variables")]
    [SerializeField] public GameObject BulletDodgeBalls;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private bool CanSpawn = true;


    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBullets(BulletDodgeBalls);

        SpawnBulletstoTheLeft(BulletDodgeBalls);
        
    }

    // Update is called once per frame
    void Update()
    {

       
    }



    private void SpawnBullets(GameObject Dodgeballs)
    {
        

        //DO A FOR LOOP TO SPAWN THE DODGEBALLS//
        for(int i = 0; i< 4; i++)
        {
            

            Vector3 SpawnPos = SpawnPoint.position + (Offset * i);
           
            Instantiate(BulletDodgeBalls, SpawnPos, Quaternion.identity);


            
        }



    }

    //THIS FUNCTION IS FOR SPAWNING BULLETS//
    private void SpawnBulletstoTheLeft(GameObject Dodgeballs)
    {
        

        //DO A FOR LOOP TO SPAWN THE DODGEBALLS//
        for(int i = 0; i < 4; i++)
        {
            //SKIP THE FIRST ITERATION//
            if(i == 0)
            {
                //CONTINUE MEANS SKIP//
                continue;

            }

           
            //SPAWN THE OTHER BULLETS TO THE LEFT//
            else
            {
                
            

            Vector3 SpawnPos = SpawnPoint.position + (-Offset * i);
           
            Instantiate(BulletDodgeBalls, SpawnPos, Quaternion.identity);

            
            }
            
        }



    }
}
