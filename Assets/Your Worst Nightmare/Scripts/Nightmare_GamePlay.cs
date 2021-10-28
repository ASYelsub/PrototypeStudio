using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Nightmare_GamePlay : MonoBehaviour
{
    public Text txt;

[HideInInspector]
    public bool isTyping = false;
    public void TypeStart(string input)
    {   
        if(!isTyping){
            txt.text = "";
            StartCoroutine(PlayText(input));
        }
    }

    IEnumerator PlayText(string input)
    {   isTyping = true;
        foreach (char c in input)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.125f);
        }
        isTyping = false;
        yield return null;
    }
}
