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

    enum RotState {down, goingUp, up,goingDown};

    RotState myRotState = RotState.down;


    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(myRotState.Equals(RotState.down)){
                myRotState = RotState.goingUp;
                StartCoroutine(RotateLegJoint(rs.transform, rf.transform, rb.transform, new Vector3(0, 1, 0), 1f));
                StartCoroutine(MoveLeg(transform,transform.localPosition, (transform.localPosition + new Vector3(-3,3,0))));
            }
        }
    }
    void FixedUpdate(){
        if(myRotState.Equals(RotState.goingUp)){
            if(!rotating)
                myRotState = RotState.up;
        }
        if(myRotState.Equals(RotState.up)){
            myRotState = RotState.goingDown;
            StartCoroutine(RotateLegJoint(rs.transform, rf.transform, rb.transform, new Vector3(0, -1, 0), 1f));
            StartCoroutine(MoveLeg(transform, transform.localPosition, (transform.localPosition - new Vector3(-3, 3, 0))));
        }
        if(myRotState.Equals(RotState.goingDown)){
            if(!rotating)
                myRotState = RotState.down;
        }
    }
    bool rotating = false;
    IEnumerator MoveLeg(Transform legspinner, Vector3 start, Vector3 end){
        float t = 0f;
        while (t < 1){
            legspinner.transform.localPosition = Vector3.Lerp(start,end,t);
            t+= Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
    }
    IEnumerator RotateLegJoint(Transform legspinner, Transform frontLeg, Transform backLeg, Vector3 addVec, float length){
        rotating = true;
        float t = 0f;
        while(t<length){
            legspinner.Rotate(addVec);
            frontLeg.Rotate(-addVec);
            backLeg.Rotate(-addVec);
            t += Time.fixedDeltaTime;
            yield return null;
        }
        rotating = false;
        yield return null;
    }

}
