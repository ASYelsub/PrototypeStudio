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
    public GameObject directionalLight;
    public GameObject ring1;
    public GameObject ring2;

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

    void FixedUpdate()
    {
        directionalLight.transform.Rotate(new Vector3(0,.1f,0));
        ring1.transform.Rotate(new Vector3(0,.5f,0));
        ring2.transform.Rotate(new Vector3(0, .5f, 0));
    }
}
