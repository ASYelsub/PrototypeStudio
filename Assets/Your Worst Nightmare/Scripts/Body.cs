using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    int pipeCount;
    List<Body> sprouter;
    List<Body> receiver;
    List<GameObject> myPipeObjects = new List<GameObject>();
    List<Pipe> myPipeScripts = new List<Pipe>();

    public enum GridStatus{
        connected,
        nonConnected
    }
    public enum PipeConfig{
        noPipes,
        onePipeNC,
        twoPipesNC,
        threePipesNC,
        fourPipesNC,
        onePipeC,
        twoPipesC,
        threePipesC,
        fourPipesC
    }
    [HideInInspector]public GridStatus myStatus;
    [HideInInspector]public PipeConfig myConfig;

    public GameObject pipePrefab;
    [HideInInspector]public bool pipesAnimating = false;
    public void Init(int pc, List<Body> s, List<Body> r){
        pipeCount = pc;
        sprouter = s;
        receiver = r;
        myStatus = GridStatus.nonConnected;
        DetermineConnections();
      //  DetermineConfig();
        MakePipes();
    }
    void MakePipes(){
        Debug.Log("Made it here.");
        float pcf = (float)pipeCount;
        float space = 360/pcf;
        for (int i = 0; i < pipeCount; i++)
        {
            GameObject newPipeObject = Instantiate(pipePrefab,this.gameObject.transform);
            Pipe newPipeScript = newPipeObject.GetComponent<Pipe>();
            Vector3 addRot = new Vector3(0,space,0);
            newPipeObject.transform.Rotate(addRot*i);
            newPipeScript.Init(this);
            myPipeObjects.Add(newPipeObject);
            myPipeScripts.Add(newPipeScript);
        }
    }

    public void TogglePipes(){
        pipesAnimating = true;
        foreach (var p in myPipeScripts)
        {
            if(p.myAnimStatus.Equals(Pipe.PipeAnimStatus.inside))
                p.ExtendPipe();
            else if(p.myAnimStatus.Equals(Pipe.PipeAnimStatus.outside))
                p.RecedePipe();
        }
        pipesAnimating = false;
    }
    public void ExtendPipes(){
        pipesAnimating = true;
        foreach (var p in myPipeScripts)
        {
            p.ExtendPipe();
        }
        pipesAnimating = false;
    }
    public void RecedePipes()
    {
        pipesAnimating = true;
        foreach (var p in myPipeScripts)
        {
            p.RecedePipe();
        }
        pipesAnimating = false;
    }
    void DetermineConnections(){
        if(pipeCount>0 && pipeCount < 5)
            myConfig = (PipeConfig)pipeCount;
        else
            Debug.Log("Invalid pipe count.");
    }

   
    public void UpdateConnections(List<Body> s, List<Body> r){
        sprouter = s;
        receiver = r;
        DetermineConfig();
    }




    void DetermineConfig()
    {
        if (sprouter.Equals(null) && receiver.Equals(null))
        {
            myStatus = GridStatus.nonConnected;
        }
        else if (!sprouter.Equals(null) && receiver.Equals(null))
        {
            myStatus = GridStatus.connected;
        }
        else if (sprouter.Equals(null) && !receiver.Equals(null))
        {
            myStatus = GridStatus.connected;
        }
        else if (!sprouter.Equals(null) && !receiver.Equals(null))
        {
            myStatus = GridStatus.connected;
        }
    }
}
