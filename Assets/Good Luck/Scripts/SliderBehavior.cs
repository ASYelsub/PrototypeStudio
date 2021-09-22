using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderBehavior : MonoBehaviour
{
    public GameObject sliderPanel;
    public GameObject UISlider;

    public Vector2 sliderPanelOriginal;
    List<GameObject> cubes = new List<GameObject>();
    List<float> cubeXs = new List<float>();
    public AudioClip beepOn;
    public AudioClip beepOff;
    private void Start()
    {
        sliderPanelOriginal = sliderPanel.GetComponent<RectTransform>().sizeDelta;
        sliderPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
    }
    public void SetCubes(List<GameObject> inputCubes)
    {
        foreach (var i in inputCubes)
        {
            cubes.Add(i);
            cubeXs.Add(i.GetComponent<Transform>().localPosition.x);
        }
    }


    public void SliderInput(float sliderPos)
    {
      //  sliderPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(sliderPos* sliderPanelOriginal.x, sliderPanelOriginal.y);
        HighlightCubes(sliderPos);
    }

    int turnedOnCubes = 0;

    [HideInInspector]
    public List<GameObject> selectedCubes = new List<GameObject>();
    public void HighlightCubes(float sliderCutOff)
    {
        sliderCutOff = sliderCutOff * (8.55f+2.615f) - 2.6f;
        List<GameObject> inCubes = new List<GameObject>();
//        Debug.Log("slider cut off: " + sliderCutOff);
        for (int i = 0; i < cubeXs.Count; i++)
        {
            
  //          Debug.Log("cube x: " + cubeXs[i]);
            
            if(cubeXs[i] < sliderCutOff)
            {
                  cubes[i].GetComponent<Renderer>().material.color = Color.green;
                inCubes.Add(cubes[i]);
                
            }
            else
            {
                cubes[i].GetComponent<Renderer>().material.color = Color.red;
            }
        }
        selectedCubes.Clear();
        foreach (var item in inCubes)
        {
            //gameObject.GetComponent<AudioSource>().PlayOneShot(beepOn);
            selectedCubes.Add(item);
        }
        
    }
}
