using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    void FixedUpdate(){
        gameObject.transform.Rotate(0,-.1f,0);
    }
}
