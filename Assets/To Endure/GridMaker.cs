using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public GameObject gridMarker;
    public GameObject gridHolder;

    public int gridXCount;
    public int gridYCount;

    List<GameObject> gridMarkers = new List<GameObject>();
    public float gridSpacer;
    void Start()
    {
        for (int i = 0; i < gridXCount; i++)
        {
            for (int j = 0; j < gridYCount; j++)
            {
              GameObject newMarker = Instantiate(gridMarker,new Vector3(0,0,0),Quaternion.identity,gridHolder.GetComponent<Transform>());
              newMarker.GetComponent<Transform>().localPosition = new Vector3(i * gridSpacer, 0, j * gridSpacer);
                gridMarkers.Add(newMarker);  
            }
        }
    }

}
