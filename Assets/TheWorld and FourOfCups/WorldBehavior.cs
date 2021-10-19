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

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        if (mx == 0 && my == 0)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = mx;
        }
    }

    void FixedUpdate(){

        

        foreach (var s in stripeScripts)
        {
            s.UpdateMoveSpeed(moveSpeed);
        }
    }
}
