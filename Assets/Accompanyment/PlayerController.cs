using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 2.0f;
    public GameObject playerCamera;

    private Vector3 movingCameraPos = new Vector3(0, 1.1f, 0.36f);

    private Vector3 stillCameraPos;
    public float moveSpeed = 10.0f;
    public float cameraMoveSpeed;
    public Animator animator;
public bool canMove = false;
    CharacterController characterController;
    float yVelocity;
    public Transform hingeTransform;
    void Start()
    {
        stillCameraPos = playerCamera.GetComponent<Transform>().localPosition;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerCamera.GetComponent<CameraController>().Initialize(transform, this);
    }


    void Update()
    {

        if (Input.GetKeyUp(KeyCode.F))
        {
            SwitchMoveState();
        }

        if (canMove)
        { 
            Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            inputDirection.Normalize();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            Move(inputDirection);
        }

    }
    public void SwitchMoveState(){
        canMove = !canMove;
        if (canMove)
        {
            StartCoroutine(HingeMove(rotateSpeed, hingeTransform));
            animator.SetTrigger("ToSwim");
            StartCoroutine(MoveObject(playerCamera.GetComponent<Transform>(),stillCameraPos,movingCameraPos,cameraMoveSpeed));
        }
        else if (!canMove)
        {
            animator.SetTrigger("ToSit");
        }
    }

    public IEnumerator MoveObject(Transform objectToMove, Vector3 initialPos, Vector3 finalPos, float speed)
    {
        float t = 0;
        while (t < .9f)
        {
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        while (t < 1f && t>=.9f)
        {
            objectToMove.GetComponent<Transform>().localPosition = Vector3.Lerp(initialPos, finalPos, t);
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
    }
    void Move(Vector2 input)
    {
        Vector3 moveVelocity = transform.right * input.x + transform.forward * input.y;
        yVelocity -= gravity * Time.deltaTime;
        moveVelocity += Vector3.up * yVelocity;
        if (input.y >=0)
        {
            characterController.Move(moveVelocity * moveSpeed * Time.deltaTime);
   
        }

       
    }

    public float rotateSpeed;
    public IEnumerator HingeMove(float speed, Transform objectToMove)
    {
        float t = 0;
        while (t < 1f)
        {
            Vector3 axis = new Vector3(0, -2f, 0);
            objectToMove.Rotate(eulers:axis,Space.Self);
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
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
