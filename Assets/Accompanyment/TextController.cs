using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class TextController : MonoBehaviour
{
    public TextMeshPro startText;
    public List<TextMeshPro> textMeshObjects = new List<TextMeshPro>();
    public List<TextMeshPro> nearTextMeshes = new List<TextMeshPro>();
    public List<string> texts = new List<string>();
    private int textsOnScreen;
    public Vector3 onScreenVec;
    public Vector3 offScreenVec;

    private List<Vector3> finalPos = new List<Vector3>();
    private List<Vector3> startPos = new List<Vector3>();
    private Transform screenTransform;
    public float screenMoveSpeed;
    public List<AudioClip> textSounds = new List<AudioClip>();
    AudioSource AS;
    public AudioSource musicSource;
    private List<bool> textFloated = new List<bool>();
    bool textEmptied = false;
    private void Start()
    {
        
        AS = GetComponent<AudioSource>();
        textsOnScreen = 0;
        screenTransform = GetComponent<Transform>();

        for (int i = 0; i < textMeshObjects.Count; i++)
        {
            textMeshObjects[i].text = "";
            nearTextMeshes[i].text = "";
            startPos.Add(textMeshObjects[i].gameObject.GetComponent<Transform>().localPosition);
            finalPos.Add(nearTextMeshes[i].gameObject.GetComponent<Transform>().localPosition);
            textFloated.Add(false);
            finalPosFront.Add(nearTextMeshes[i].gameObject.GetComponent<Transform>().localPosition);
            finalPosFront[i] += new Vector3(0, 0, 3);
            finalPosBack.Add(nearTextMeshes[i].gameObject.GetComponent<Transform>().localPosition);
            finalPosBack[i] += new Vector3(0, 0, -3);
            compoundNums.Add(0);
        }
    }

    private bool screenIsOn = false;
    private bool screenIsMoving = false;
    private List<Vector3> finalPosFront = new List<Vector3>();
    private List<Vector3> finalPosBack = new List<Vector3>();
    private void Update()
    {
       /* if (Input.GetKeyUp(KeyCode.Tab) && !screenIsMoving)
        {
           // ToggleOnScreen();
        }*/

       if (Input.GetKeyDown(KeyCode.R))
       {
           Application.LoadLevel(0);
       }
        if (Input.GetKeyUp(KeyCode.C) && !AS.isPlaying && !textEmptied)
        {
            AddMessage();
        }

        for (int i = 0; i < textFloated.Count; i++)
        {
            if (textFloated[i] == true)
            {
                Floater(nearTextMeshes[i].gameObject.GetComponent<Transform>(), i);
            }
        }
    }

    private bool startTextOn = true;
    private bool musicStarted = false;

    List<float> compoundNums = new List<float>();
    public float frequency;
    public float amplitude;
    void Floater(Transform obj, int i)
    {
        compoundNums[i] += Time.fixedDeltaTime;
        float num2 = compoundNums[i] + 3;
        float num3 = compoundNums[i] - 1;
        Vector3 tempPos = obj.localPosition;
        tempPos.z -= Mathf.Sin((compoundNums[i]) * Mathf.PI * frequency) * amplitude;
        tempPos.y -= Mathf.Sin(num3 * Mathf.PI * frequency) * amplitude;
        tempPos.x -= Mathf.Sin(num2 * Mathf.PI * frequency) * amplitude;
        obj.localPosition = tempPos;
    }
    void AddMessage()
    {
        if (startTextOn)
        {
            startText.enabled = false;
            startTextOn = false;
        }
        for (int i = textMeshObjects.Count-1; i >= 1; i--)
        {
            textMeshObjects[i].text = textMeshObjects[i-1].text;
        }
        textMeshObjects[0].text = texts[textsOnScreen];
        
        TextAddSound();
        if (textsOnScreen == 3)
        {
            musicSource.Play();
            musicStarted = true;
        }
        if (textsOnScreen > 3)
        {
            System.Random ran = new System.Random();
            int i = ran.Next(0, 4);
            print(i);
            nearTextMeshes[i].gameObject.transform.localPosition = startPos[i];
            nearTextMeshes[i].text = textMeshObjects[i].text;
            int f = ran.Next(0, 4);
            StartCoroutine(MoveThing(startPos[i], finalPos[i], nearTextMeshes[i].gameObject.GetComponent<Transform>(), screenMoveSpeed,f));
            textFloated[i] = true;
        }
        textsOnScreen++;
        if (textsOnScreen == textMeshObjects.Count)
        {
            textEmptied = true;
        }
      
    }

    void TextAddSound()
    {
        System.Random ran = new System.Random();
        int i = ran.Next(0, 3);
        AS.PlayOneShot(textSounds[i]);
    }

    void ToggleOnScreen()
    {
        screenIsMoving = true;
        System.Random ran = new System.Random();
        int i = ran.Next(0, 3);
        if(screenIsOn)
            StartCoroutine(MoveThing(onScreenVec, offScreenVec,screenTransform,screenMoveSpeed,i));
        else
            StartCoroutine(MoveThing(offScreenVec, onScreenVec,screenTransform,screenMoveSpeed,i));
        screenIsOn = !screenIsOn;
        screenIsMoving = false;
    }
    public static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }
    public static float EaseInOutCubic(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value + 2) + start;
    }
    public static float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }
    public static float EaseInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }
    IEnumerator MoveThing(Vector3 startPos, Vector3 endPos, Transform objectToMove, float speed, int easeID)
    {
        float t = 0;
        while (t < 1f)
        {
            switch (easeID)
            {case 0:
                    objectToMove.gameObject.GetComponent<TextMeshPro>().color = Color.Lerp(Color.red, Color.white, EaseInOutCubic(0,1,t));
                    objectToMove.localPosition = Vector3.Lerp(startPos, endPos, EaseInOutCubic(0,1,t));
                break;
            case 1:
                objectToMove.gameObject.GetComponent<TextMeshPro>().color = Color.Lerp(Color.red, Color.white, EaseOutQuad(0,1,t));
                objectToMove.localPosition = Vector3.Lerp(startPos, endPos, EaseOutQuad(0,1,t));
                break;
            case 2:
                objectToMove.gameObject.GetComponent<TextMeshPro>().color = Color.Lerp(Color.red, Color.white, EaseOutCubic(0,1,t));
                objectToMove.localPosition = Vector3.Lerp(startPos, endPos, EaseOutCubic(0,1,t));
                break;
            case 3:
                objectToMove.gameObject.GetComponent<TextMeshPro>().color = Color.Lerp(Color.red, Color.white, EaseInQuad(0,1,t));
                objectToMove.localPosition = Vector3.Lerp(startPos, endPos, EaseInQuad(0,1,t));
                break;
            }
            
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
    }
}
