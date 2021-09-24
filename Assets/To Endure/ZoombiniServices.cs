using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ZoombiniServices 
{
    public static ZoombiniSpawner zoombiniSpawner;
    public static MouseBehavior mouseBehavior;
   public static void Init()
    {
        zoombiniSpawner = GameObject.FindObjectOfType<ZoombiniSpawner>();
        zoombiniSpawner.Init();
        mouseBehavior = GameObject.FindObjectOfType<MouseBehavior>();

    }
}
