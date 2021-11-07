using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public static MonsterScript me;

    //Set Max Distance
    public float maxDistance;

    public float rotSpeed;
    public float speed;
    public float baseSpeed;
    public float minSpeed;

    bool noL;
    bool noR;

    public bool onlyMove;
    public bool dontMove;
    public bool killing;

    //p stands for player, d stands for detector
    public GameObject pDObj;
    public GameObject pDObjDisplay;
    public GameObject pObj;
    public float pDMaxDistance;
    public float pDMinDistance;
    bool couldSeePlayer;
    bool canSeePlayer;

    public Vector3 pDirection;

    int timer;

    public float angle;

    public Light[] lights;
    public float detectionTimer;
    public Color blue;
    public Color pink;
    public Color red;
    public int stateOfSeeing;

    public bool turningRight;
    public bool turningLeft;

    public AudioSource as1;
    public AudioSource as2;

    void Awake()
    {
        me = this;
        
    }

    void Update()
    {
        SpeedController();
        PlayerDetectionRay();
        HeadTurner();
        CloseRangeRays();        
    }

    void FixedUpdate()
    {
        if (couldSeePlayer)
        {
            DetectionTimer();
        }

        if (!dontMove && !killing)
        {
            Movement();
        }
        if (onlyMove)
        {
            timer++;
            if (timer > 45)
            {
                onlyMove = false;
                timer = 0;
            }
        }
    }

    void SpeedController()
    {
        float disToP = Vector3.Distance(transform.position, pObj.transform.position);
        speed = baseSpeed * (disToP / pDMaxDistance);
        if(speed < minSpeed)
        {
            speed = minSpeed;
        }
    }

    void PlayerDetectionRay()
    {
        pDObj.transform.LookAt(pObj.transform);
        Ray pDRay = new Ray(pDObj.transform.position, pDObj.transform.forward);
        RaycastHit pDHit;

        Debug.DrawRay(pDRay.origin, pDRay.direction * 1000, Color.magenta);

        if (Physics.Raycast(pDRay, out pDHit, 1000))
        {
            if (pDHit.collider.tag == "Player")
            {
                pDirection = new Vector3(transform.eulerAngles.x, pDObj.transform.eulerAngles.y, transform.eulerAngles.z);
                couldSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
                couldSeePlayer = false;
                stateOfSeeing = 1;
                detectionTimer = 0;
            }
        }
    }

    void DetectionTimer()
    {
        detectionTimer++;
        if(detectionTimer > 90)
        {
            stateOfSeeing = 3;
            canSeePlayer = true;
        }
        else
        {
            stateOfSeeing = 2;
        }

    }

    //Turns the Head and activates canSeePlayer if they are in front of the monster
    void HeadTurner()
    {
        if (couldSeePlayer)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].gameObject.transform.LookAt(pObj.transform);
            }
        }
        else
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].gameObject.transform.eulerAngles = transform.eulerAngles; ;
            }
        }

        if(stateOfSeeing == 1)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].color = blue;
                as1.Play();
            }          
        }
        if(stateOfSeeing == 2)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].color = pink;
                as2.Play();
            }
        }
        if(stateOfSeeing == 3)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].color = red;
            }
        }
    }

    void CloseRangeRays()
    {
        // Define Ray

        Ray roombaRayL = new Ray(transform.position - (Vector3.up * 3), transform.forward - (transform.right * 0.5f));
        Ray roombaRayR = new Ray(transform.position - (Vector3.up * 3), transform.forward + (transform.right * 0.5f));
        RaycastHit RHit;
        RaycastHit LHit;


        // Draw Debug Ray
        Debug.DrawRay(roombaRayL.origin, roombaRayL.direction * maxDistance, Color.cyan);
        Debug.DrawRay(roombaRayR.origin, roombaRayR.direction * maxDistance, Color.cyan);

        //Shoot RayCast

        if (Physics.Raycast(roombaRayL, out LHit, maxDistance))
        {
            if (LHit.collider.tag != "Floor")
            {
                noL = false;
                onlyMove = true;
                timer = 0;
            }
        }
        else
        {
            noL = true;
        }

        if (Physics.Raycast(roombaRayR, out RHit, maxDistance))
        {
            if (RHit.collider.tag != "Floor")
            {
                noR = false;
                onlyMove = true;
                timer = 0;
            }
        }
        else
        {
            noR = true;
        }
    }

    public void Kill()
    {
       killing = true;
    }

    void Movement()
    {
        if (!noR && !noL)
        {
            int randomChance = Random.Range(0, 100);
            if (randomChance < 50)
            {
                transform.Rotate(0, 30, 0);
            }
            else
            {
                transform.Rotate(0, -30, 0);
            }
        }

        if (!noL && noR)
        {
            transform.Rotate(0, rotSpeed, 0);
            turningRight = true;
        }
        else
        {
            turningRight = false;
        }

        if(!noR && noL)
        {
            transform.Rotate(0, -rotSpeed, 0);
            turningLeft = true;
        }
        else
        {
            turningLeft = false;
        }

        if (transform.eulerAngles.x != 0)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (transform.eulerAngles.z != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }


        if (noR && noL)
        {
            if (canSeePlayer && !onlyMove)
            {  
                Vector3 pDir = pObj.transform.position - transform.position;
                Vector3 nDir = Vector3.RotateTowards(transform.forward, pDir, 0.02f, 0.0f);
                transform.rotation = Quaternion.LookRotation(nDir);
            }
            transform.Translate(0, 0, Time.deltaTime * speed);
        }    
    }
}
