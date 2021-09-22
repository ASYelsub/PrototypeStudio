// Floater v0.0.2
// by Donovan Keith
//
// [MIT License](https://opensource.org/licenses/MIT)

using UnityEngine;
using System.Collections;

// Makes objects float up & down while gently spinning.
public class Floater : MonoBehaviour
{
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    CoinToss coinToss;
    // Use this for initialization
    void Start()
    {
        coinToss = FindObjectOfType<CoinToss>();
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }

    float timePassed = 0f;
    // Update is called once per frame
    void Update()
    {
        //        Spin();
        if (coinToss.coinSpinning)
            Float();
        
    }

    void Spin() //in update
    {
        //Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
    }
    public void SetNewPosOffSet()
    {
        posOffset = transform.position;
    }
    float compoundNum = 0f;
    void Float() //in update
    {
        compoundNum += Time.fixedDeltaTime;
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y -= Mathf.Sin((compoundNum) * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}