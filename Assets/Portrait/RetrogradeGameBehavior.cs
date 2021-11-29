using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RetrogradeGameBehavior : MonoBehaviour
{
    public List<Frame> storyFrames = new List<Frame>();
    public Material imageMat;
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

    

    public GameObject startPanel;
    public GameObject gamePanel;
    public Text textBox;
    public Image spriteImage;
    public int currentFrame = 0;
    public bool typing = false;
    public AudioSource clickAS;
    public AudioSource typeAS;
    public AudioClip clickSound;
    string currentString = "";
    [Range(5,13)]
    public float spriteScale = 10f;
    public float typeSpeed = 0.1f;
    static int GameState = 0;
    void Start(){
       startPanel.SetActive(true);
       gamePanel.SetActive(false);
    }
    
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return)){
            if(GameState==0){
                startPanel.SetActive(false);
                gamePanel.SetActive(true);
                GameState = 1;
            }
            NextFrame();
        }
         if(Input.GetKeyDown(KeyCode.Mouse1)){
             SpeedTextUp();
         }
        if(Input.GetKeyDown(KeyCode.R)){
            Restart();
        }
    }
    void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextFrame(){
        if(!typing){
            clickAS.PlayOneShot(clickSound);
            SetVisuals(storyFrames[currentFrame].photo);
            SetTextTypeWrite(storyFrames[currentFrame].text);
            if(currentFrame<storyFrames.Count-1){
                currentFrame++;
            }
            return;
        } 
    }
    float originalSpeed;
    bool textSped = false;
    void SpeedTextUp(){
        if(!typing)
            return;
        if(textSped)
            return;
        
        originalSpeed = typeSpeed;
        typeSpeed *= .2f;
        textSped = true;
        return;

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
       // imageMat.SetTexture("_MainTex",textureFromSprite(sprite));
    }
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    void SetImageSize(Vector2 spriteSize)
    {
        spriteImage.rectTransform.sizeDelta = spriteSize*spriteScale;
    }
    public Vector2 GetSpriteSize(Sprite sprite)
    {
        float width = (float)sprite.bounds.size.x;
        float height = (float)sprite.bounds.size.y;
      //  Debug.Log(width + height);
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
    public List<AudioClip> typeSounds = new List<AudioClip>();
    IEnumerator TypeWrite(string typeString, Text placeToType)
    {
        typing = true;
        foreach (char c in typeString)
        {
            placeToType.text += c;
             if (!System.Char.IsWhiteSpace(c))
             {  if(System.Char.IsPunctuation(c)){
                 typeAS.PlayOneShot(clickSound);
                }else{
                    int f = Random.Range(0, typeSounds.Count);
                    typeAS.PlayOneShot(typeSounds[f]);
                }
            }
            yield return new WaitForSeconds(typeSpeed);
        }
        typing = false;
        typeSpeed = originalSpeed;
        textSped = false;
        yield return null;
    }






    
}


