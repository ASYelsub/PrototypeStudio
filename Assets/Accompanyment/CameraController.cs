using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 5;

    public float minLookDown = -90;
    public float maxLookUp = 90;
    public float minLookSide = -90;
    public float maxLookSide = 90;
    float pitch;
    float yaw;

    public Transform playerTransform;

    PlayerController owningController;
    
    public void Initialize(Transform player, PlayerController controller)
    {
        playerTransform = player;
        owningController = controller;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // yaw input
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // pitch input

        if (!owningController.canMove)
        {
            yaw +=mouseX;
            yaw = Mathf.Clamp(yaw, minLookSide, maxLookSide);
        }
        else
            yaw = 0;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minLookDown, maxLookUp);
        

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0.0f);

        if(owningController.canMove)
            playerTransform.Rotate(playerTransform.up * mouseX); 
    }

    public PlayerController GetOwningController()
    {
        return owningController;
    }
}