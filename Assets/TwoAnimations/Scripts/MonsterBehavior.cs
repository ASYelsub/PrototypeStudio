using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    [Header("Monster Body Parts")]
    public GameObject rf;
    public GameObject lf;
    public GameObject rb;
    public GameObject lb;
    public GameObject rs;
    public GameObject ls;
    public GameObject body;
    public GameObject entire;
    public GameObject target;
    public GameObject lLengthf;
    public GameObject rLengthf;
    public GameObject lLengthb;
    public GameObject rLengthb;

    enum RotState {down, goingUp, up, goingForward, atForward, goingDown};

    RotState myRotState = RotState.down;

    float lsf = 0;
    float rsf = Mathf.PI;

    [Header("Adjustment")]
    public float legHeight;
    public float legForward;
    public float rotAngle;
    public float moveSpeed;
    public float moveInterval;
    public float restTime;
    
    Vector3 rinit;
    Vector3 linit;
    void Start(){
        rinit = rs.transform.localPosition;
        linit = ls.transform.localPosition;
        startLength = lLengthb.transform.localScale;
        backLengthStartPos = lLengthb.transform.localPosition;
        frontLengthStartPos = lLengthf.transform.localPosition; 

    }
    void FixedUpdate(){
       // TargetRot();
        TargetRotBasic();
        LegSpinnerMovement();
        LegJointRot();
      //  LegLengthMovement();
    }
    float moveTimer = 0f;
    void TargetRotBasic(){
        target.transform.Rotate(0, -.01f, 0);
    }
    void TargetRot(){
        if(moveTimer > moveInterval && moveTimer <moveInterval+restTime){
            moveTimer += Time.deltaTime*moveSpeed;
        }else if (moveTimer > moveInterval+restTime){
            moveTimer = 0;
        }else{
            moveTimer += Time.deltaTime;
            target.transform.Rotate(0, -.1f, 0);
        }

    }
    void LegSpinnerMovement(){
        lsf += Time.fixedDeltaTime;
        rsf += Time.fixedDeltaTime;
        ls.transform.localPosition = linit + new Vector3(Mathf.Cos(lsf) * legForward,
                                                 Mathf.Sin(lsf) * legHeight,
                                                 0);
        rs.transform.localPosition = rinit + new Vector3(Mathf.Cos(rsf) * legForward,
                                                 Mathf.Sin(rsf) * legHeight,
                                                 0);
    }
    void LegJointRot(){
        lb.transform.Rotate(new Vector3(0,Mathf.Cos(lsf)*rotAngle,0));
        lf.transform.Rotate(new Vector3(0, Mathf.Cos(lsf) * rotAngle, 0));
        rb.transform.Rotate(new Vector3(0, Mathf.Cos(rsf) * rotAngle, 0));
        rf.transform.Rotate(new Vector3(0, Mathf.Cos(rsf) * rotAngle, 0));
    }
    Vector3 startLength;
    Vector3 backLengthStartPos;
    Vector3 frontLengthStartPos;

    void LegLengthMovement(){
        lLengthb.transform.localScale = startLength + new Vector3(0,0,Mathf.Cos(lsf));
        rLengthb.transform.localScale = startLength + new Vector3(0, 0,Mathf.Cos(rsf));
        rLengthf.transform.localScale = startLength + new Vector3(0, 0, Mathf.Cos(rsf));
        lLengthf.transform.localScale = startLength + new Vector3(0, 0, Mathf.Cos(lsf));
        lLengthb.transform.localPosition = backLengthStartPos + new Vector3(0, 0, Mathf.Cos(rsf));
        rLengthb.transform.localPosition = backLengthStartPos + new Vector3(0, 0, Mathf.Cos(lsf));
        lLengthf.transform.localPosition = frontLengthStartPos + new Vector3(0, 0, Mathf.Cos(lsf));
        rLengthf.transform.localPosition = frontLengthStartPos + new Vector3(0, 0, Mathf.Cos(rsf));
    }
}
