using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightmare_CameraController : MonoBehaviour
{
    public float mouseSensitivity = 5;

    public float minLookDown = -90;
    public float maxLookUp = 90;

    public float minLookDownWithTablet = -90;
    public float maxLookUpWithTablet = 90;

    float pitch;

    public Transform playerTransform;

    Nightmare_PlayerController owningController;



    public void Initialize(Transform player, Nightmare_PlayerController controller)
    {
        playerTransform = player;
        owningController = controller;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
  

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // yaw input
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // pitch input

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minLookDown, maxLookUp);

            transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);

            playerTransform.Rotate(playerTransform.up * mouseX);

    }

    public Nightmare_PlayerController GetOwningController()
    {
        return owningController;
    }
}
