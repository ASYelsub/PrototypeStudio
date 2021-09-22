using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxBehavior : MonoBehaviour
{
    [Header("Variables")]
    public float typeSpeed;
   // public List<string> texts = new List<string>();
    string startText;
    public Text txtBox;
    public List<AudioClip> typeBeep = new List<AudioClip>();
    AudioSource audioSource;
    [HideInInspector]
    public bool typing = false;
    [HideInInspector]
    public string currentString;
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        startText = "I am the coin flipper. I am here to flip your luck. Luck is deterministic. Let us begin.";
        SetTextTypeWrite(startText);
    }

    void SetText(string newText)
    {
        txtBox.text = newText;
    }
    public void SetTextTypeWrite(string newText)
    {
        SetText("");
        currentString = newText;
        StartCoroutine(TypeWrite(newText, txtBox));
        
    }

    bool soundFlip = false;
    private IEnumerator TypeWrite(string typeString, Text placeToType)
    {
        typing = true;
        foreach (char c in typeString)
        {
            placeToType.text += c;
            if(!System.Char.IsWhiteSpace(c))
            {
                if (soundFlip)
                    audioSource.PlayOneShot(typeBeep[0]);
                else
                    audioSource.PlayOneShot(typeBeep[1]);
                if(System.Char.IsPunctuation(c))
                    soundFlip = !soundFlip;

            }
            yield return new WaitForSeconds(typeSpeed);
        }
      //  print("DONE");
        typing = false;
        yield return null;
    }
}
