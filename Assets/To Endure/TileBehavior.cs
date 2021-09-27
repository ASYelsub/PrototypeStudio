using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public GameObject seeThrough;
    private void Start()
    {
        seeThrough.SetActive(false);
    }
    private void OnTriggerExit(Collider other)
    {
        seeThrough.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        seeThrough.SetActive(true);
        Debug.Log("Hello");
    }
}
