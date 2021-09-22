using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Zoombini : MonoBehaviour
{
    //ZoombiniSpawner zmSpawner;
    ZoombiniSpawner.HairTypes hair;
    ZoombiniSpawner.EyeTypes eyes;
    ZoombiniSpawner.NoseTypes nose;
    ZoombiniSpawner.FeetTypes feet;


    public GameObject propRotate;
    public GameObject EWRot;
    public GameObject NSRot;

    Vector3 rotateVec;
    GameObject body;

    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 bodyPos;
   

    public Zoombini(ZoombiniSpawner.HairTypes newHair, ZoombiniSpawner.EyeTypes newEyes, ZoombiniSpawner.NoseTypes newNose, ZoombiniSpawner.FeetTypes newFeet)
    {
        this.hair = newHair;
        this.eyes = newEyes;
        this.nose = newNose;
        this.feet = newFeet;
    }
    private void Start()
    {
        
        bodyPos = new Vector3(gameObject.GetComponent<Transform>().localPosition.x,
            gameObject.GetComponent<Transform>().localPosition.y,
            gameObject.GetComponent<Transform>().localPosition.z);
        rotateVec = new Vector3(0, 1, 0);
    }
    void FixedUpdate()
    {
        switch (feet)
        {
            case (ZoombiniSpawner.FeetTypes.propeller) :
                PropellerAnim();
                    break;
        }
    }

    float propellerAnimTimer = 0f;
    void PropellerAnim()
    {
        propellerAnimTimer += Time.deltaTime;
        body.GetComponent<Transform>().position = new Vector3(bodyPos.x, bodyPos.y + Mathf.Sin((propellerAnimTimer) * Mathf.PI * frequency) * amplitude, bodyPos.z);
        EWRot.GetComponent<Transform>().Rotate(rotateVec);
        NSRot.GetComponent<Transform>().Rotate(rotateVec);
    }
}
