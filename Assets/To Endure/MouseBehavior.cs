using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{
    [HideInInspector]
    public GameObject selectedZoombini;
    public GameObject pickedUpZoombini;
    RaycastHit hitInfo = new RaycastHit();
    public Camera mainCam;
    

    
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {            
           LeftMouse();
        }else{
            NonMouseCickUpdate();
        }
    }

    void LeftMouse(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
             if(hitInfo.transform.gameObject.CompareTag("Zoombini")){
                if(pickedUpZoombini != null)
                {
                    DropOldZoombini();
                }
                else
                {
                    DropOldZoombini();
                    PickUpNewZoombini(hitInfo);
                }
              }
            else if(pickedUpZoombini != null)
            {
                DropOldZoombini();
            }
        }
    }
   
    void NonMouseCickUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
            if (hitInfo.transform.gameObject.CompareTag("Zoombini"))
            {
                DeselctOldZoombini();
                SelectNewZoombini(hitInfo);
            }
            else { DeselctOldZoombini();}
        }
    }
    


    void DropOldZoombini()
    {
        if(pickedUpZoombini != null)
            pickedUpZoombini.GetComponent<Zoombini>().PutDown();
        pickedUpZoombini = null;
        DeselctOldZoombini();
    }
    void PickUpNewZoombini(RaycastHit hit)
    {
        pickedUpZoombini = hit.transform.gameObject;
        pickedUpZoombini.GetComponent<Zoombini>().PickUp();
    }

    void SelectNewZoombini(RaycastHit hit)
    {
        selectedZoombini = hit.transform.gameObject;
        selectedZoombini.GetComponent<Zoombini>().Select();
    }

    void DeselctOldZoombini()
    {
        if(selectedZoombini!=null)
            selectedZoombini.GetComponent<Zoombini>().Deselect();
        selectedZoombini = null;
    }

}
