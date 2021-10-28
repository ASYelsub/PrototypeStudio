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
        GameObject newBody = Instantiate(bodyPrefab,Vector3.zero,Quaternion.identity,bodyHolder.transform);
        newBody.transform.Rotate(new Vector3(-90, 0, 0));
        newBody.transform.position += bodyStartPos.transform.position;
        Body newBodyScript = newBody.GetComponent<Body>();
        newBodyScript.Init(3,null,null);
        bodyScripts.Add(newBodyScript);
        bodyObjects.Add(newBody);

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
