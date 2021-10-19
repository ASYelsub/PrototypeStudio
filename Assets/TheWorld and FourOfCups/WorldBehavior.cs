using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehavior : MonoBehaviour
{

    [Header("Variables")]
    public float moveSpeed = .3f;
    public GameObject topRight;
    public GameObject bottomLeft;
    public List<GameObject> stripes = new List<GameObject>();
    
    Vector3 bottomLeftPos;
    Vector3 topRightPos;

    List<Stripe> stripeScripts = new List<Stripe>();
    void Awake()
    {
        topRightPos = topRight.GetComponent<Transform>().localPosition;
        bottomLeftPos = bottomLeft.GetComponent<Transform>().localPosition;

        foreach (var s in stripes)
        {
            stripeScripts.Add(s.GetComponent<Stripe>());
        }
        foreach (var s in stripeScripts)
        {
            s.Init(moveSpeed,bottomLeftPos,topRightPos); 
        }
    }
    bool mouseHeld = false;
    float mouseBuffer = 0;
    float mouseMove;

    public float decreaseFactor = 3;
    public float moveSpeedAddition = 2;
    bool moving = false;
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && moving == false){
            moveSpeed = 0;
            mouseHeld = true;
        }else if (mouseHeld){
            if (Input.GetMouseButtonUp(0)){
                mouseHeld = false;
                mouseMove = Input.GetAxis("Mouse X");
                mouseBuffer = Mathf.Abs(mouseMove);
                moving = true;
            }
        }
        UpdateMS(moveSpeed);
        if(mouseBuffer > 0){
            Debug.Log("hello");
            moveSpeed = mouseMove;
            if(mouseMove < 0){
                mouseMove += Time.deltaTime * decreaseFactor;
            }else{
                mouseMove -= Time.deltaTime * decreaseFactor;  
            }

            
            mouseBuffer -= Time.deltaTime*decreaseFactor;
        }else{
            moveSpeed = 0;
            moving = false;
        }
    }
    void UpdateMS(float ms){
        foreach (var s in stripeScripts)
        {
            s.UpdateMoveSpeed(ms);
        }
    }
}
