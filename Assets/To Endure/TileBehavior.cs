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
        if (other.gameObject.CompareTag("Zoombini"))
        {
            other.gameObject.GetComponent<Zoombini>().overTile = false;
            other.gameObject.GetComponent<Zoombini>().currentTileOver = null;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        seeThrough.SetActive(true);
        Debug.Log("Hello");
        if (other.gameObject.CompareTag("Zoombini"))
        {
            other.gameObject.GetComponent<Zoombini>().overTile = true;
            other.gameObject.GetComponent<Zoombini>().currentTileOver = gameObject.transform;
        }
    }
}
