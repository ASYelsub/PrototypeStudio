
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class AnimationConverter : MonoBehaviour {
 
    //For example: Assets/_@MYSTUFF/StaticAnimations/
    public string SaveLocation;
    //create a component on your gameobject and add the
    public Animation animation;
    public SkinnedMeshRenderer SkinMeshRender;
    private Mesh[] NewStaticMesh;
    //length of animation clip
    public float ClipLenth;
    //how many frames do you want to make?
    public int numberOfFrames;
    //time between frame captures
    private float WaitAmount;
    //How many frames done BEFORE saving
    private int AmountSoFar;
    //how many frames SAVED
    private int AmountSavedSoFar;
    private Mesh ThisMesh;
    public List<Mesh> MeshQ;
 
    private void Start()
    {
        Debug.Log("Exporting static meshes from skinned mesh...");
        ExportMeshes();
    }
 
    void ExportMeshes()
    {
        ClipLenth = animation.clip.length;
        WaitAmount = animation.clip.length / numberOfFrames;
        //now let's start waiting for the animation to play
        StartCoroutine(WaitForNextMesh());
    }
 
    void AddMeshToQ(Mesh NewMesh)
    {
        MeshQ.Add(NewMesh);
    }
 
    //IMPORTANT: I'm using a coroutine because if I don't, I create
    //the same static meshes because the animation won't change in 1 frame.
    //The purpose is to wait as the animation is playing, then make a static mesh at the correct time.
 
    IEnumerator WaitForNextMesh()
    {
        yield return new WaitForSeconds(WaitAmount);
        AmountSoFar++;
        //wait done! Let's make the static mesh!
        ThisMesh = new Mesh();
        SkinMeshRender.BakeMesh(ThisMesh);
        //now that's it's made, add it to the que
        AddMeshToQ(ThisMesh);
        if (AmountSoFar < numberOfFrames)
        {
            //do it again, we have more meshes to make!
            StartCoroutine(WaitForNextMesh());
        }
 
        else
        {
            //created all meshes, we're done!
            foreach (Mesh staticmesh in MeshQ)
            {
                AmountSavedSoFar++;
                //try to save to specified location
                try
                {
                    AssetDatabase.CreateAsset(staticmesh, SaveLocation + AmountSavedSoFar.ToString() + animation.clip.name + "_StaticFromSkinned" + ".asset");
                }
                //if the location is invalid, throw an error
                catch
                {
                    Debug.Log("<color=red><b>Invalid save location! Make sure you've spelled the path to a folder correctly. For example: Assets/_@MYSTUFF/StaticAnimations/ </b></color>");
                    yield break;
                }
            }
            //spam the console in fancy ways
            Debug.Log("<color=green><b>All meshes created! You'll find them here: </b></color>" + SaveLocation);
            Debug.Log("<color=red><i>For some reason I can't explain, delete the first frame and the last 2 frames so they loop properly.</i></color>");
            Debug.Log("<color=red><i>Don't forget to disable/change this script, or you'll do what you just did again!</i></color>");
        }
    }
}