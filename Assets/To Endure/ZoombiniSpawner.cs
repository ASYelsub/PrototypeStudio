using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ZoombiniSpawner : MonoBehaviour
{
   public enum HairTypes { spikey, ponytail, tuft, flat, cap, length}
   public enum EyeTypes { dots, droopy, sunglass, glasses, mono, length}
   public enum NoseTypes { yellow, pink, red, green, blue, length }
   public enum FeetTypes { wheels, propeller, spring, sneakers, skates, length}

   int zoombiniCount = 16;

   public List<Zoombini> zoombinis;


   [Header("Zoombini Prefab Components")]
   public GameObject bodyPrefab;
    public GameObject[] hairPrefabs = new GameObject[(int)HairTypes.length];
    public GameObject[] eyePrefabs = new GameObject[(int)EyeTypes.length];
    public GameObject[] nosePrefabs = new GameObject[(int)NoseTypes.length];
    public GameObject[] feetPrefabs = new GameObject[(int)FeetTypes.length];


    public void Start()
    {
       
        zoombinis = CreateZoombinis();
    }

    public List<Zoombini> CreateZoombinis()
    {
        for (int i = 0; i < zoombiniCount; i++)
        {
           // Zoombini newZoombini = 
        }
        return null;
    }
}
