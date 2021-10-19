using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stripe : MonoBehaviour
{
    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public Vector3 initialPos;
    [HideInInspector]
    public Vector3 currentPos;

    Vector3 bottomLeftPos;
    Vector3 topRightPos;
    float moveSpeed;
    
    public void Init(float ms,Vector3 blp, Vector3 trp){
        myTransform = GetComponent<Transform>();
        currentPos = myTransform.localPosition;
        initialPos = currentPos;
        moveSpeed = ms;
        bottomLeftPos = blp;
        topRightPos = trp;
        
        if(currentPos.x < bottomLeftPos.x)
        {
            currentPos.x = topRightPos.x;
            currentPos.y = topRightPos.y;
        }
        myTransform.localPosition = currentPos;
    }
    public void UpdateMoveSpeed(float ms){
        moveSpeed = ms;
    }
    void FixedUpdate()
    {
        /*if(currentPos.x > bottomLeftPos.x){
            currentPos.x -= moveSpeed * Time.deltaTime;
            currentPos.y -= moveSpeed * Time.deltaTime;
        }else{
            currentPos = topRightPos;
        }*/

        if(currentPos.x >= topRightPos.x)
            currentPos = bottomLeftPos;
        else if(currentPos.x <= bottomLeftPos.x)
            currentPos = topRightPos;
        else{
            currentPos.x += moveSpeed * Time.deltaTime;
            currentPos.y += moveSpeed * Time.deltaTime;
        }

        myTransform.localPosition = currentPos;
    }
}
