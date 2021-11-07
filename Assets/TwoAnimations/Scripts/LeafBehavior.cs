using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class LeafBehavior : MonoBehaviour
{

    public float amplitude = 0.5f;
    public float frequency = 1f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start(){
        posOffset = transform.localPosition;
    }
    void FixedUpdate(){
    FloatInWind();
    }
    public float changeInterval = 10f;
    public float leafMoveXSpeed = 0f;
    public float leafMoveYSpeed = 0f;
    public float leafMoveZSpeed = 0f;
    float yDirection = 1f;
    float xDirection = 1f;
    float zDirection = 1f;

    float compoundNum = 0f;
    float changeTimer = 0f;
    float sinTrack = 0;
    float cosTrack = 0;
    Vector3 addPos;
        float sinSign = 1;
        float cosSign = 1;
        Vector3 rotVec;
    void FloatInWind(){
        compoundNum += Time.fixedDeltaTime;
        sinTrack = Mathf.Sin((compoundNum) * Mathf.PI/2);
        cosTrack = Mathf.Sin((compoundNum) * Mathf.PI /2);


        if(sinTrack%1==0){
            int r = Random.Range(0,2);
            if(r==0){
                if (sinSign == -1)
                    sinSign = 1;
                else if (sinSign == 1)
                    sinSign = -1;
            }
        }
        
        if(cosTrack%1==0){
            int r = Random.Range(0, 2);
            if (r == 0)
            {
            if(cosSign==-1)
            cosSign=1;
            else if(cosSign==1)
            cosSign=1;
            }
        } 

        tempPos.y = -1*(posOffset.y + compoundNum);
        tempPos.x = Mathf.Cos((posOffset.x + compoundNum) * Mathf.PI) * amplitude;
        tempPos.z = Mathf.Sin((posOffset.z+compoundNum) * Mathf.PI) * amplitude;
        
       // tempPos.x = (posOffset.x + compoundNum);
       // tempPos.z = (posOffset.z + compoundNum);
        transform.localPosition = tempPos + addPos;

        rotVec.x = Mathf.Cos(compoundNum * Mathf.PI) * amplitude;
        rotVec.z = Mathf.Sin(compoundNum * Mathf.PI) * amplitude;
        transform.Rotate(rotVec);
        changeTimer += compoundNum * Mathf.PI;

       
    }
   
}
