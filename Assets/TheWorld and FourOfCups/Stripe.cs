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
    void Start()
    {
        myTransform = GetComponent<Transform>();
        currentPos = myTransform.localPosition;
        initialPos = currentPos;
    }
    float moveSpeed;
    
    public void SendVariables(float ms,Vector3 blp, Vector3 trp){
        moveSpeed = ms;
        bottomLeftPos = blp;
        topRightPos = trp;
    }
    void FixedUpdate()
    {
        if(currentPos.x > bottomLeftPos.x){
            currentPos.x -= moveSpeed * Time.deltaTime;
            currentPos.y -= moveSpeed * Time.deltaTime;
        }else{
            currentPos = topRightPos;
        }
        myTransform.localPosition = currentPos;
    }
}
