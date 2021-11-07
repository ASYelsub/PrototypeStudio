using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    public static MonsterAnimation me;

    public GameObject LeftSpinner;
    public GameObject FLLeg;
    public GameObject BLLeg;

    public GameObject RightSpinner;
    public GameObject FRLeg;
    public GameObject BRLeg;

    public bool killing;

    public int timer;
    public Vector3 offset;

    void Awake()
    {
        me = this;
    }

    void FixedUpdate()
    {
        if (!MonsterScript.me.dontMove && !killing)
        {
            if (!MonsterScript.me.turningLeft)
            {
                LeftSpinner.transform.Rotate(0, -2, 0);

            }
            else
            {
                LeftSpinner.transform.Rotate(0, 2, 0);
            }

            if (!MonsterScript.me.turningRight)
            {
                RightSpinner.transform.Rotate(0, -2, 0);
            }
            else
            {
                RightSpinner.transform.Rotate(0, 2, 0);
            }
            FLLeg.transform.eulerAngles = Vector3.zero;
            BLLeg.transform.eulerAngles = Vector3.zero;
            FRLeg.transform.eulerAngles = Vector3.zero;
            BRLeg.transform.eulerAngles = Vector3.zero;
        }

        if (killing)
        {
            if (timer > 0)
            {
                timer--;
                transform.position -= offset;
                FLLeg.transform.position += offset;
                BLLeg.transform.position += offset;
                FRLeg.transform.position += offset;
                BRLeg.transform.position += offset;
            }
        }
    }

    public void Kill()
    {
        killing = true;
    }
}
