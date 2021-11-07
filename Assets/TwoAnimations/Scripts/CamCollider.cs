using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamCollider : MonoBehaviour
{
    public TreeCameraController myCameraController;
    void OnTriggerStay(Collider other){
        myCameraController.RecieveCollision();
    }
    void OnTriggerExit(Collider other){
        myCameraController.LeaveCollision();
    }
}