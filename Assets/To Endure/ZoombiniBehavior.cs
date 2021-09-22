using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoombiniBehavior : MonoBehaviour
{
    public GameObject propRotate;
    public GameObject EWRot;
    public GameObject NSRot;

    Vector3 rotateVec;
    GameObject body;

    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 bodyPos;
    float compoundNum = 0f;
    private void Start()
    {
        body = gameObject;
        bodyPos = new Vector3(gameObject.GetComponent<Transform>().localPosition.x,
            gameObject.GetComponent<Transform>().localPosition.y,
            gameObject.GetComponent<Transform>().localPosition.z);
        rotateVec = new Vector3(0,1,0);
    }
    void FixedUpdate()
    {
        compoundNum += Time.deltaTime;
        body.GetComponent<Transform>().position = new Vector3(bodyPos.x, Mathf.Sin((compoundNum) * Mathf.PI * frequency) * amplitude,bodyPos.z);
        EWRot.GetComponent<Transform>().Rotate(rotateVec);
        NSRot.GetComponent<Transform>().Rotate(rotateVec);
    }
}
