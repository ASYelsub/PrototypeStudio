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

    enum RotState {down, goingUp, up, goingForward, atForward, goingDown};

    RotState myRotState = RotState.down;

    float lsf = 0;
    float rsf = Mathf.PI;

    [Header("Adjustment")]
    public float legHeight = 1f;
    public float legForward = 1f;
    
    Vector3 rinit;
    Vector3 linit;
    void Start(){
        rinit = rs.transform.localPosition;
        linit = ls.transform.localPosition;
    }
    void FixedUpdate(){
        lsf += Time.fixedDeltaTime;
        rsf += Time.fixedDeltaTime;
       ls.transform.localPosition = linit + new Vector3(Mathf.Cos(lsf)*legForward,
                                                Mathf.Sin(lsf)*legHeight,
                                                0);
        rs.transform.localPosition = rinit +new Vector3(Mathf.Cos(rsf)*legForward,
                                                 Mathf.Sin(rsf)*legHeight, 
                                                 0);
        target.transform.Rotate(0, -.1f, 0);
    }
    bool rotating = false;
    bool moving = false;
    IEnumerator MoveLeg(Transform legspinner, Vector3 start, Vector3 end){
        moving = true;
        float t = 0f;
        while (t < 1){
            legspinner.transform.localPosition = Vector3.Lerp(start,end,t);
            t+= Time.fixedDeltaTime;
            yield return null;
        }
        moving = false;
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
