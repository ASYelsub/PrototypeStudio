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
    public AudioClip mySound;
    public AudioSource AS;
    Material onMat;
    Material offMat;

public void SendMat(Material on, Material off){
    onMat = on;
    offMat = off;
}

bool isOn = false;
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
    void OnMouseDown(){
        AS.PlayOneShot(mySound);
        if(!isOn){
            gameObject.GetComponent<MeshRenderer>().material = onMat;
        }else{
            gameObject.GetComponent<MeshRenderer>().material = offMat;
        }
        isOn = !isOn;

    }
}
