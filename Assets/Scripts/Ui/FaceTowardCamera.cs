using UnityEngine;

public class FaceTowardCamera : MonoBehaviour
{
    [Header("Target Tracking")]
    [SerializeField] private Transform playerPos;
    
    [Header("Height Adjustment")]
    [SerializeField] private Vector3 Offset; 


    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (playerPos == null) return;

        // OFFSET//
        transform.position = playerPos.position + Offset;

        // ROTATION
        if (mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
