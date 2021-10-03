using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AngelSpawner : MonoBehaviour
{
    public GameObject angelPrefab;
    public List<GameObject> angelHolders;
    public GameObject cubePrefab;
    public int angelCount = 40;
    private void Start()
    {
        int h = 0;
        for (int i = 0; i < angelCount; i++)
        {
            Vector3 pos = angelHolders[h].GetComponent<Transform>().position + new Vector3(0, -i,0);
            GameObject newAngel;
            if (i > 80)
                newAngel = Instantiate(cubePrefab,pos,Quaternion.identity,angelHolders[h].GetComponent<Transform>());
            else
                newAngel = Instantiate(angelPrefab,pos,Quaternion.identity,angelHolders[h].GetComponent<Transform>());
            newAngel.GetComponent<Transform>().localRotation = new Quaternion(0,0,0,0);
            newAngel.GetComponent<Transform>().Rotate(0,3,0);
            if (h != 7)
            {
                h++;
            }
            else
            {
                h = 0;
            }
        }
    }

    void FixedUpdate()
    {
        gameObject.GetComponent<Transform>().Rotate(0,-1,0);
    }
}
