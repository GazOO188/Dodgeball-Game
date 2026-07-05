using UnityEngine;

public class FaceTowardCamera : MonoBehaviour
{
    [Header("Target Tracking")]
    [SerializeField] private Transform playerPos;
    
    [Header("Height Adjustment")]
    [SerializeField] private Vector3 Offset; 

    // Cache the camera reference to save performance processing power
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (playerPos == null) return;

        // 1. POSITION: Keeps the health bar attached above the player's head
        transform.position = playerPos.position + Offset;

        // 2. ROTATION: Forces the canvas to mirror the camera view so it never flips or spins
        if (mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
