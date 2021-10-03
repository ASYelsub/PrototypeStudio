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

    public void ChangeLookSpan(float newAngle)
    {
        maxLookUp = newAngle;
        minLookDown = -newAngle;
        maxLookSide = newAngle;
        minLookDown = -newAngle;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private bool gameplayCursor = true;
    public void SwitchCursorLockState()
    {
        gameplayCursor = !gameplayCursor;
        if (gameplayCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SwitchCursorLockState();
        }

        if (gameplayCursor)
        {
            GameplayCameraUpdate();
        }
    }

    public void GameplayCameraUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // yaw input
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // pitch input

        if (!owningController.canMove)
        {
            yaw +=mouseX;
            yaw = Mathf.Clamp(yaw, minLookSide, maxLookSide);
        }
        else
        {
            yaw = 0;
        }
            

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