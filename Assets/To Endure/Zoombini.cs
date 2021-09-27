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

    public GameObject hairObj;
    public GameObject eyesObj;
    public GameObject noseObj;
    public GameObject feetObj;

    public GameObject propRotate;
    public GameObject EWRot;
    public GameObject NSRot;



    Vector3 rotateVec;
    GameObject bodyObj;

    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 bodyPos;
   
    int numberID;
    Zoombini thisZoom;
    Vector3 initialPos;
    public void SetZoombini(GameObject bodyPrefab, ZoombiniSpawner.HairTypes newHair, ZoombiniSpawner.EyeTypes newEyes, ZoombiniSpawner.NoseTypes newNose, ZoombiniSpawner.FeetTypes newFeet, int count)
    {
        this.hair = newHair;
        this.eyes = newEyes;
        this.nose = newNose;
        this.feet = newFeet;
        numberID = count;
        bodyObj = bodyPrefab;
        hairObj = Instantiate(ZoombiniServices.zoombiniSpawner.hairPrefabs[(int)hair], bodyObj.GetComponent<Transform>());
        eyesObj = Instantiate(ZoombiniServices.zoombiniSpawner.eyePrefabs[(int)eyes], bodyObj.GetComponent<Transform>());
        noseObj = Instantiate(ZoombiniServices.zoombiniSpawner.nosePrefabs[(int)nose], bodyObj.GetComponent<Transform>());
        feetObj = Instantiate(ZoombiniServices.zoombiniSpawner.feetPrefabs[(int)feet], bodyObj.GetComponent<Transform>());

        rotateVec = new Vector3(0, 1, 0);

        
        bodyObj.GetComponent<Transform>().localPosition = new Vector3(count%4, 0,count/4);
        initialPos = gameObject.transform.position;
    }

    public bool isUp = false;
    public bool isSelected = false;
    public void Select()
    {    
        gameObject.GetComponent<MeshRenderer>().material = ZoombiniServices.zoombiniSpawner.zoomBodSelectMat;
    }
    public void Deselect()
    {
        gameObject.GetComponent<MeshRenderer>().material = ZoombiniServices.zoombiniSpawner.zoomBodDeselectMat;
    }    
    public void PickUp()
    {
        isUp = true;
    }
    public void PutDown()
    {
        isUp = false;
        gameObject.GetComponent<Transform>().position = initialPos;
    }
    
    void FixedUpdate()
    {
        switch (feet)
        {
            case (ZoombiniSpawner.FeetTypes.propeller) :
               // PropellerAnim();
                    break;
        }
        if(isUp)
         transform.localPosition = GetMouseAsWorldPoint() + mOffset;
    }

    float propellerAnimTimer = 0f;
    void PropellerAnim()
    {
        propellerAnimTimer += Time.deltaTime;
        bodyObj.GetComponent<Transform>().position = new Vector3(bodyPos.x, bodyPos.y + Mathf.Sin((propellerAnimTimer) * Mathf.PI * frequency) * amplitude, bodyPos.z);
        EWRot.GetComponent<Transform>().Rotate(rotateVec);
        NSRot.GetComponent<Transform>().Rotate(rotateVec);
    }



    private Vector3 mOffset;
    private float mZCoord;  
    void OnMouseDown(){
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.localPosition).z;
        mOffset = gameObject.transform.localPosition - GetMouseAsWorldPoint();
    }
    private Vector3 GetMouseAsWorldPoint(){
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        mousePoint = Camera.main.ScreenToWorldPoint(mousePoint);
        mousePoint.y = ZoombiniServices.gridMaker.GetComponent<Transform>().position.y;
        return mousePoint;
    }

    
}
