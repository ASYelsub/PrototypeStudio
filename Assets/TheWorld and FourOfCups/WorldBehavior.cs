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
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && mouseBuffer == 0){
            mouseHeld = true;
        }else if (mouseHeld){
            if (Input.GetMouseButtonUp(0)){
                mouseHeld = false;
                mouseMove = Input.GetAxis("Mouse X");
                mouseBuffer = Mathf.Abs(mouseMove);
            }
        }
        else if(mouseBuffer > 0){
            moveSpeed = mouseMove;
            mouseBuffer -= Time.deltaTime;
        }

        UpdateMS(moveSpeed);
    }

    void UpdateMS(float ms){
        foreach (var s in stripeScripts)
        {
            s.UpdateMoveSpeed(ms);
        }
    }
}
