using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{//this class draws lines connecting a bunch of objects in a linear manner

    public Material lineMat;
    public float radius = 0.05f;
    [HideInInspector]
    public List<GameObject> points = new List<GameObject>();
    public Mesh cylinderMesh;

    List<GameObject> ringGameObjects = new List<GameObject>();

    bool lineDrawStarted = false;
    public void StartLineDraw()
    {

        for (int i = 0; i < points.Count; i++)
        {
            this.ringGameObjects.Add(new GameObject());
        }
       
        for (int i = 0; i < points.Count; i++)
        {
            // Make a gameobject that we will put the ring on
            // And then put it as a child on the gameobject that has this Command and Control script
            this.ringGameObjects[i] = new GameObject();
            this.ringGameObjects[i].name = "Connecting ring #" + i;
            this.ringGameObjects[i].transform.parent = this.gameObject.transform;

            // We make an offset gameobject to counteract the default cylindermesh pivot/origin being in the middle
            GameObject ringOffsetCylinderMeshObject = new GameObject();
            ringOffsetCylinderMeshObject.transform.parent = this.ringGameObjects[i].transform;

            // Offset the cylinder so that the pivot/origin is at the bottom in relation to the outer ring gameobject.
            ringOffsetCylinderMeshObject.transform.localPosition = new Vector3(0f, 1f, 0f);
            // Set the radius
            ringOffsetCylinderMeshObject.transform.localScale = new Vector3(radius, 1f, radius);

            // Create the the Mesh and renderer to show the connecting ring
            MeshFilter ringMesh = ringOffsetCylinderMeshObject.AddComponent<MeshFilter>();
            ringMesh.mesh = this.cylinderMesh;

            MeshRenderer ringRenderer = ringOffsetCylinderMeshObject.AddComponent<MeshRenderer>();
            ringRenderer.material = lineMat;

        }
        lineDrawStarted = true;
        gameObject.GetComponent<SliderBehavior>().SetCubes(points);
    }


    void Update()
    {
        if (lineDrawStarted)
        {
            for (int i = 0; i < points.Count; i++)
            {
                // Move the ring to the point
                this.ringGameObjects[i].transform.position = this.points[i].transform.position;

                // Match the scale to the distance
                float cylinderDistance;
                if (i != 0)
                    cylinderDistance = 0.5f * Vector3.Distance(this.points[i].transform.position, this.points[i - 1].transform.position);
                else
                    cylinderDistance = 0f;

                this.ringGameObjects[i].transform.localScale = new Vector3(this.ringGameObjects[i].transform.localScale.x, cylinderDistance, this.ringGameObjects[i].transform.localScale.z);

                // Make the cylinder look at the main point.
                // Since the cylinder is pointing up(y) and the forward is z, we need to offset by 90 degrees.
                if (i != 0)
                    this.ringGameObjects[i].transform.LookAt(this.points[i - 1].transform, Vector3.up);
                this.ringGameObjects[i].transform.rotation *= Quaternion.Euler(90, 0, 0);
            }

        }


    }
}