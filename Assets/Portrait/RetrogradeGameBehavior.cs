using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RetrogradeGameBehavior : MonoBehaviour
{
    public List<Frame> storyFrames = new List<Frame>();

    [System.Serializable]
    public struct Frame
    {
        public Sprite photo;
        public string text;

        public Frame(Sprite photo, string text){
            this.photo = photo;
            this.text = text;
        }
    }

    

    
    public Text textBox;
    public Image spriteImage;
    public int currentFrame = 0;
    public bool typing = false;
    string currentString = "";
    [Range(5,13)]
    public float spriteScale = 10f;
    public float typeSpeed = 0.1f;

    void Start(){
        NextFrame();
    }
    
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            NextFrame();
        }
    }
    public void NextFrame(){
        if(!typing){
            SetVisuals(storyFrames[currentFrame].photo);
            SetTextTypeWrite(storyFrames[currentFrame].text);
            if(currentFrame<storyFrames.Count-1){
                currentFrame++;
            }
            return;
        } 
    }

    //Visual//
    void SetVisuals(Sprite sprite)
    {
        SetImage(sprite);
        SetImageSize(GetSpriteSize(sprite));
    }
    void SetImage(Sprite sprite)
    {
        spriteImage.sprite = sprite;
    }

    void SetImageSize(Vector2 spriteSize)
    {
        spriteImage.rectTransform.sizeDelta = spriteSize*spriteScale;
    }
    public Vector2 GetSpriteSize(Sprite sprite)
    {
        float width = (float)sprite.bounds.size.x;
        float height = (float)sprite.bounds.size.y;
        Debug.Log(width + height);
        return new Vector2(width, height);
    }


    //Text//
    void SetTextInstant(string newText)
    {
        textBox.text = newText;
    }
    void SetTextTypeWrite(string newText)
    {
        SetTextInstant("");
        currentString = newText;
        StartCoroutine(TypeWrite(newText, textBox));
    }
    IEnumerator TypeWrite(string typeString, Text placeToType)
    {
        typing = true;
        foreach (char c in typeString)
        {
            placeToType.text += c;
            // if (!System.Char.IsWhiteSpace(c))
            // {
            //     if (soundFlip)
            //         audioSource.PlayOneShot(typeBeep[0]);
            //     else
            //         audioSource.PlayOneShot(typeBeep[1]);
            //     if (System.Char.IsPunctuation(c))
            //         soundFlip = !soundFlip;

            // }
            yield return new WaitForSeconds(typeSpeed);
        }
        typing = false;
        yield return null;
    }






    
}


