using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 2.0f;
    public GameObject playerCamera;
   
    public float moveSpeed = 10.0f;

    CharacterController characterController;
    float yVelocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera.GetComponent<CameraController>().Initialize(transform, this);
    }


    void Update()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDirection.Normalize();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Move(inputDirection);
    }

    
    void Move(Vector2 input)
    {
        Vector3 moveVelocity = transform.right * input.x + transform.forward * input.y;
        yVelocity -= gravity * Time.deltaTime;
        moveVelocity += Vector3.up * yVelocity;
        characterController.Move(moveVelocity * moveSpeed * Time.deltaTime);
        
    }
    

    void Jump()
    {
        if(!characterController.isGrounded)
        {
            return;
        }

        float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        yVelocity = jumpVelocity;
    }

   
   

   
}
