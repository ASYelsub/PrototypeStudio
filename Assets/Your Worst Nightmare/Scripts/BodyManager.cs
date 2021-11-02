using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    public GameObject bodyPrefab;
    public GameObject bodyHolder;
    List<GameObject> bodyObjects = new List<GameObject>();
    List<Body> bodyScripts = new List<Body>();
    public static bool anyPipesAnimating = false;
    public GameObject bodyStartPos;
    void Start(){
        bodyStartPos.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < 20; i++)
        {
            float r = Random.Range(-4,4);
            float r2 = Random.Range(0,5);
            GameObject newBody = Instantiate(bodyPrefab, Vector3.zero, Quaternion.identity, bodyHolder.transform);
            newBody.transform.Rotate(new Vector3(20*i*r2*r, -90, 90));
            newBody.transform.position += bodyStartPos.transform.position + new Vector3(2f*r,2f*r2,0);
            Body newBodyScript = newBody.GetComponent<Body>();
            int r3 = Random.Range(1,4);
            newBodyScript.Init(r3, null, null);
            bodyScripts.Add(newBodyScript);
            bodyObjects.Add(newBody);
        }
        

    }

    void Update(){
        if(Input.GetKeyUp(KeyCode.F)&&!anyPipesAnimating){
            ToggleAllPipes();
        }
    }

    public void ToggleAllPipes(){
        anyPipesAnimating = true;
        foreach (var b in bodyScripts)
        {
            b.TogglePipes();
        }
    }
    public void RecedeAllPipes(){
        anyPipesAnimating = true;
        foreach (var b in bodyScripts)
        {
            b.RecedePipes();
        }

    }
    public void ExtendAllPipes(){
        anyPipesAnimating = true;
        foreach (var b in bodyScripts)
        {
            b.ExtendPipes();
        }
    }
}
