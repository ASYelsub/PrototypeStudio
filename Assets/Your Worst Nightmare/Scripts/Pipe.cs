using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject pipeObj;
    [HideInInspector]public Body bodyParent;
    [HideInInspector]public Body bodyTouch;

    public enum PipeStatus
    {
        latent,
        disconnected,
        connected
    }
    public enum PipeAnimStatus{
        inside,
        extending,
        receding,
        outside
    }
    public PipeStatus myStatus;
    public PipeAnimStatus myAnimStatus;

    Vector3 outPos;
    Vector3 inPos;
    Vector3 outscale;
    Vector3 inScale;
    public void Init(Body bp){

        outPos = new Vector3(2.21f,-4.45e-13f,0);
        inPos = new Vector3(0,-4.45e-13f,0);
        outscale = new Vector3(4.796188f,0.79285f,0.79285f);
        inScale = new Vector3(0,0.79285f,0.79285f);

        bodyParent = bp;
        myStatus = PipeStatus.disconnected;
        myAnimStatus = PipeAnimStatus.inside;
        
        //outscale = 
        DetermineStartVisual();
    }

    void DetermineStartVisual(){
        switch(myAnimStatus){
            case (PipeAnimStatus.inside): 
                pipeObj.transform.localScale = inScale;
                pipeObj.transform.localPosition = inPos;
                break;
            case (PipeAnimStatus.outside):
                pipeObj.transform.localScale = outscale;
                pipeObj.transform.localPosition = outPos;
                break;
        }
    }

    float extendSpeed = 1f;
    [HideInInspector] public bool pipeAnimating = false;
    public void ExtendPipe(){
        pipeAnimating = true;
        myAnimStatus = PipeAnimStatus.extending;
        StartCoroutine(Scale(pipeObj.transform,inScale,outscale,extendSpeed));
        StartCoroutine(Move(pipeObj.transform,inPos,outPos,extendSpeed));
    }
    public void RecedePipe(){
        pipeAnimating = true;
        myAnimStatus = PipeAnimStatus.receding;
        StartCoroutine(Scale(pipeObj.transform, outscale, inScale, extendSpeed));
        StartCoroutine(Move(pipeObj.transform, outPos, inPos, extendSpeed));
    }

    
    void TogglePipeAnimStatus(){
        if(myAnimStatus.Equals(PipeAnimStatus.extending))
            myAnimStatus = PipeAnimStatus.outside;
        else if (myAnimStatus.Equals(PipeAnimStatus.receding))
            myAnimStatus = PipeAnimStatus.inside;

        BodyManager.anyPipesAnimating = false;
    }


    /*Coroutines*/
    public IEnumerator Scale(Transform tr, Vector3 oldS, Vector3 newS, float speed)
    {
        float t = 0;
        while (t <= 1f)
        {
            tr.localScale = Vector3.Lerp(oldS, newS, t);
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        TogglePipeAnimStatus();
        yield return null;
    }
    public IEnumerator Move(Transform tr, Vector3 oldP, Vector3 newP, float speed){
        float t = 0;
        while (t <= 1f)
        {
            tr.localPosition = Vector3.Lerp(oldP, newP, t);
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
    }

}
