using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{

  

    [HideInInspector]
    public GameObject activeZoombini;
    RaycastHit hitInfo = new RaycastHit();
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                if (hitInfo.transform.gameObject.CompareTag("Zoombini"))
                {
                    if(activeZoombini != null)
                        activeZoombini.GetComponent<Zoombini>().PutDown();
                    activeZoombini = hitInfo.transform.gameObject;
                    activeZoombini.GetComponent<Zoombini>().PickUp();
                }


            }

        }
        else if(activeZoombini != null && !activeZoombini.GetComponent<Zoombini>().isUp)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                if (hitInfo.transform.gameObject.CompareTag("Zoombini"))
                {
                    if (activeZoombini != null)
                        activeZoombini.GetComponent<Zoombini>().Deselect();
                    activeZoombini = hitInfo.transform.gameObject;
                    activeZoombini.GetComponent<Zoombini>().Select();

                }
                else
                {
                    if(activeZoombini != null)
                    {
                        activeZoombini.GetComponent<Zoombini>().Deselect();
                        activeZoombini = null;
                    }
                }


            }
        }
    }
}
