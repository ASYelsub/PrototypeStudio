using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class WorldBehavior : MonoBehaviour
{

    [Header("Variables")]
    public float moveSpeed = .3f;
    public GameObject topRight;
    public GameObject bottomLeft;
    public List<GameObject> stripes = new List<GameObject>();
    public GameObject directionalLight;
    public GameObject ring1;
    public GameObject ring2;
    public GameObject ring3;
    public Material hub1off;
    public Material hub1on;
    public Material hub6off;
    public Material hub6on;

    public GameObject inside;
    public GameObject outside;
    Vector3 bottomLeftPos;
    Vector3 topRightPos;

    public PostProcessVolume ppv;
    public PostProcessVolume ppv2;

    
    List<Stripe> stripeScripts = new List<Stripe>();
    void Awake()
    {
        topRightPos = topRight.GetComponent<Transform>().localPosition;
        bottomLeftPos = bottomLeft.GetComponent<Transform>().localPosition;
        bool odd = true;
        foreach (var s in stripes)
        {
            stripeScripts.Add(s.GetComponent<Stripe>());
        }
        foreach (var s in stripeScripts)
        {
            if(!odd){
                s.SendMat(hub1on, hub1off);
            }else{
                s.SendMat(hub6on,hub6off);
            }
            odd = !odd;
            s.Init(moveSpeed,bottomLeftPos,topRightPos,this); 
        }
    }
    float directionalLightTrack = 0f;
    bool directionalLightSwitched = false;
    bool inSwitch = false;
    float swTimer = 0f;
    
    [HideInInspector]
    public int touchInt = 0;
    bool touchIntTriggered = false;
    void FixedUpdate()
    {
        directionalLight.transform.Rotate(new Vector3(0,.1f,0));
        directionalLightTrack += .1f;
        
        if(!inSwitch){
            if (directionalLightTrack >= 90f && directionalLightTrack <= 91f)
            {
                directionalLightSwitched = !directionalLightSwitched;
                inSwitch = true;
            }
        }else{
            if(swTimer < 3){
                swTimer += 1*Time.deltaTime;
            }
            else{
                swTimer = 0;
                inSwitch = false;
            }
        }

        if(directionalLightSwitched){
            ring1.transform.Rotate(new Vector3(0, 1.2f, 0));
            ring2.transform.Rotate(new Vector3(0, -1.2f, 0));
            ring3.transform.Rotate(new Vector3(0, .2f, 0));
        }else{
            ring1.transform.Rotate(new Vector3(0, .5f, 0));
            ring2.transform.Rotate(new Vector3(0, .5f, 0));
            ring3.transform.Rotate(new Vector3(0, .5f, 0));
        }
        
        if(!touchIntTriggered){
            if(touchInt>=20){
                touchIntTriggered=true;
                TypeStart();
            }
        }
        inf = Mathf.Lerp(0,1,(Mathf.Sin(Time.deltaTime*Mathf.PI)));
        ouf = Mathf.Lerp(0,-1,(Mathf.Sin(Time.deltaTime*Mathf.PI)));
        Vector3 invec = new Vector3(0,ouf,inf);
        Vector3 outvec = new Vector3(0,inf,ouf);
        inside.GetComponent<Transform>().Rotate(invec);
        outside.GetComponent<Transform>().Rotate(outvec);

        if(textThere){
            if(!ppvHappened)
            {
                ppvHappened = true;
            }
        }
        if(ppvHappened){
            if(!ppvFinished){
                if(ppvTimer<1f){
                    Debug.Log(ppv.weight);
                    ppv.weight = Mathf.Lerp(0, 1, ppvTimer);
                    ppvTimer+=.1f * Time.deltaTime;
                }else{
                    ppvTimer = 0;
                    ppvFinished = true;
                }
            }
        }
        if(ppvFinished){
            Vector3 mousePos = Input.mousePosition;
            
            mousePos.x = Mathf.Abs(scaleBetween(mousePos.x, -1, 1, 0, 900));
            mousePos.y = Mathf.Abs(scaleBetween(mousePos.y, -1, 1, 0, 900));

            //x and y together so it goes from the center
            float l = mousePos.y + mousePos.x; 
            l = Mathf.Abs(scaleBetween(l, -1, 1, -2, 2));

            if (ppvHappened)
            {
                ppv.weight = Mathf.Lerp(0, 1, mousePos.x);
                ppv2.weight = Mathf.Lerp(0,1,mousePos.y);
            }
        }
    }

    float scaleBetween(float unscaledNum, float minAllowed, float maxAllowed, float min, float max)
    {
        return (maxAllowed - minAllowed) * (unscaledNum - min) / (max - min) + minAllowed;
    }


    bool ppvHappened = false;
    bool ppvFinished = false;
    float ppvTimer = 0f;
    public TextMeshPro txt;
    string story = "does it hurt to touch them?";
float inf = 0;
    float ouf = 0;

    float easeInOutCubic(float x){
        return x< 0.5 ? 4 * x* x* x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
    public void TypeStart()
    {
        
        txt.text = "";

        // TODO: add optional delay when to start
        StartCoroutine(PlayText());
    }

    bool textThere = false;
    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.125f);
        }
        textThere = true;
        yield return null;
    }

}
