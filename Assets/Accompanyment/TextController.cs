using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class TextController : MonoBehaviour
{
    public List<TextMeshPro> textMeshObjects = new List<TextMeshPro>();
    public List<string> texts = new List<string>();
    private int textsOnScreen;
    public Vector3 onScreenVec;
    public Vector3 offScreenVec;

    private Transform screenTransform;
    public float screenMoveSpeed;
    public List<AudioClip> textSounds = new List<AudioClip>();
    AudioSource AS;
    private void Start()
    {
        AS = GetComponent<AudioSource>();
        textsOnScreen = 0;
        screenTransform = GetComponent<Transform>();
        foreach (var obj in textMeshObjects)
        {
            obj.text = "";
        }
    }

    private bool screenIsOn = false;
    private bool screenIsMoving = false;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && !screenIsMoving)
        {
            ToggleOnScreen();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            AddMessage();
        }
    }

    void AddMessage()
    {
        for (int i = textMeshObjects.Count-1; i >= 1; i--)
        {
            textMeshObjects[i].text = textMeshObjects[i-1].text;
        }
        textMeshObjects[0].text = texts[textsOnScreen];
        textsOnScreen++;
      TextAddSound();
    }

    void TextAddSound()
    {
        System.Random ran = new System.Random();
        int i = ran.Next(0, 2);
        AS.PlayOneShot(textSounds[i]);
    }

    void ToggleOnScreen()
    {
        screenIsMoving = true;
        if(screenIsOn)
            StartCoroutine(MovePanel(onScreenVec, offScreenVec));
        else
            StartCoroutine(MovePanel(offScreenVec, onScreenVec));
        screenIsOn = !screenIsOn;
        screenIsMoving = false;
    }
    IEnumerator MovePanel(Vector3 startPos, Vector3 endPos)
    {
        float t = 0;
        while (t < 1f)
        {
            screenTransform.localPosition = Vector3.Lerp(startPos, endPos, t);
            t += screenMoveSpeed * Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
    }
}
