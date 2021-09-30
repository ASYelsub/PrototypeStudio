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

        if (Input.GetKeyUp(KeyCode.C) && !AS.isPlaying)
        {
            AddMessage();
        }

        for (int i = 0; i < textFloated.Count-1; i++)
        {
            if (textFloated[i] == true)
            {
                Float(nearTextMeshes[i].gameObject.GetComponent<Transform>(), i);
            }
        }
    }

    private bool startTextOn = true;
    private bool musicStarted = false;

    List<float> compoundNums = new List<float>();
    public float frequency;
    public float amplitude;
    void Float(Transform obj, int i)
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
            int i = ran.Next(0, 3);
            nearTextMeshes[i].gameObject.transform.localPosition = startPos[i];
            nearTextMeshes[i].text = textMeshObjects[i].text;
            StartCoroutine(MoveThing(startPos[i], finalPos[i], nearTextMeshes[i].gameObject.GetComponent<Transform>(), screenMoveSpeed));
            textFloated[i] = true;
        }
        textsOnScreen++;
      
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
        if(screenIsOn)
            StartCoroutine(MoveThing(onScreenVec, offScreenVec,screenTransform,screenMoveSpeed));
        else
            StartCoroutine(MoveThing(offScreenVec, onScreenVec,screenTransform,screenMoveSpeed));
        screenIsOn = !screenIsOn;
        screenIsMoving = false;
    }
    public static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }
    IEnumerator MoveThing(Vector3 startPos, Vector3 endPos, Transform objectToMove, float speed)
    {
        float t = 0;
        while (t < 1f)
        {
            objectToMove.gameObject.GetComponent<TextMeshPro>().color = Color.Lerp(Color.red, Color.white, EaseOutCubic(0,1,t));
            objectToMove.localPosition = Vector3.Lerp(startPos, endPos, EaseOutCubic(0,1,t));
            t += speed * Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
    }
}
