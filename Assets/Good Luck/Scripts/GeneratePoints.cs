using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoints : MonoBehaviour
{
    public GameObject cubeHolder;
    float xMax = 8.76f;
    float xMin = -2.79f;
    float yMax = 4.31f;
    float yMin = -3.9f;
    public GameObject cubePrefab;
    [Header("Attributes")]
    [HideInInspector]
    public int cubeCount = 30;
    [HideInInspector]
    public LineDraw lineDraw;
    // Start is called before the first frame update
    void Start()
    {
        float xSize = (xMax - xMin)/cubeCount;
        lineDraw = gameObject.GetComponent<LineDraw>();
        float x;
        float y;
        float z = -1;
        List<GameObject> tempPoints = new List<GameObject>();
        for (int i = 0; i < cubeCount; i++)
        {
            x = xSize + i * xSize - 3f;
            y = Random.Range(yMin,yMax);
            GameObject tempCube = Instantiate(cubePrefab,new Vector3(x,y,z),Quaternion.identity,cubeHolder.GetComponent<Transform>());
            tempPoints.Add(tempCube);
        }
        lineDraw.points = tempPoints;
        lineDraw.StartLineDraw();
    }

    
}
