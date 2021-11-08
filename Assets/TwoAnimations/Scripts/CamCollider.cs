using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamCollider : MonoBehaviour
{
    public TreeCameraController myCameraController;
    void OnTriggerEnter(Collider other){
        if(!other.CompareTag("Tree"))
            myCameraController.RecieveCollision();
    }
    void OnTriggerExit(Collider other){
        if (!other.CompareTag("Tree"))
            myCameraController.LeaveCollision();
 

    }
}
