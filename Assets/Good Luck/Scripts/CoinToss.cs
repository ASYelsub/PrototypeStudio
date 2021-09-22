using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinToss : MonoBehaviour
{
    public GameObject coin;

    [HideInInspector]
    public bool coinSpinning = false;
    float coinAnimTimer;

    [Header("Max Sprites")]
    public GameObject maxObject;
    public Sprite[] preSpin;
    public Sprite[] spinning;
    public Sprite[] windUp;
    public Sprite[] preSlap;

    public Vector3 groundRot;
    int coinState = 0;
    TextBoxBehavior textBoxBehavior;
    private void Start()
    {
        textBoxBehavior = gameObject.GetComponent<TextBoxBehavior>();
        int i = Random.Range(0,preSpin.Length);
        SetMaxSprite(preSpin[i]);
        sliderBehavior = gameObject.GetComponent<SliderBehavior>();
        pointerTransform = coinPointer.GetComponent<Transform>();
    }
    bool hasStarted = false;
    public void StartCoin()
    {
        if (!textBoxBehavior.typing)
        {
            if (!coinSpinning)
            {
                if(hasStarted)
                    textBoxBehavior.SetTextTypeWrite("You may change the time slider again. If you dare.");
                else
                    textBoxBehavior.SetTextTypeWrite("You must change the slider of time fidelity. Over there. The green thing, at the bottom of my box.");

                hasStarted = true;

                //    print("WE ARE HERE");
                coinState = 1;
                int i = Random.Range(0, spinning.Length);
                SetMaxSprite(spinning[i]);

            }
        }

    }
    private void FixedUpdate()
    {
        if(coinState == 1)
            WindCoin();
        else if(coinState == 2)
            WaitCoin();
        else if(coinState == 3)
            SpinCoin();
    }

    float windCoinTimer = 0;
    void WindCoin()
    {
        
        if (windCoinTimer < 1)
        {
            coin.GetComponent<Transform>().Rotate(90/1*.1f, 0, 0);
            windCoinTimer += 1 * .1f;
        }
        else
        {
            windCoinTimer = 0;
            coinState = 2;
        }
    }
    float waitCoinTimer = 0;
    void WaitCoin()
    {
        if (waitCoinTimer < 2)
        {
            waitCoinTimer += 1 * .1f;
        }
        else
        {
            waitCoinTimer = 0;
            coinState = 3;
        }
    }
    float rotateTrack = 0f;
    void SpinCoin()
    {
        coinPointer.GetComponent<Renderer>().enabled = false;
      //  print("Hello");
        coinSpinning = true;
        coin.GetComponent<Transform>().Rotate(0, 0, 30);
        int i = Random.Range(0, spinning.Length);
        if (animTimer > 1)
        {
            SetMaxSprite(spinning[i]);
            animTimer = 0;
        }
        else
            animTimer += 1 * Time.deltaTime;
        
        rotateTrack +=30;
    }   

    public SliderBehavior sliderBehavior;
    bool heads = false;
    public GameObject coinPointer;
    Transform pointerTransform;
    public float animTimer = 4f;
    public void StopCoin()
    {
        if (hasStarted)
        {
            if (!textBoxBehavior.typing)
            {
                coinState = 0;
                coinSpinning = false;
                int i = Random.Range(0, preSpin.Length);
                SetMaxSprite(preSpin[i]);

                coin.GetComponent<Transform>().Rotate(0, 0, -rotateTrack);
                coin.GetComponent<Transform>().Rotate(-90, 0, 0);

                float r = Random.Range(0, 2);
                if (r > .5f)
                {
                    heads = true;
                    print("heads");
                    pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, 1.71f, pointerTransform.localPosition.z);
                    textBoxBehavior.SetTextTypeWrite("You have gotten heads. Your life has increased in stability. And decreased in polarity.");
                }
                else
                {
                    print("tails");
                    heads = false;
                    pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, -1.71f, pointerTransform.localPosition.z);
                    textBoxBehavior.SetTextTypeWrite("You have gotten tails. The points have now polarized. Goodbye consistentency as we know it.");
                }
                coinPointer.GetComponent<Renderer>().enabled = true;

                foreach (var item in sliderBehavior.selectedCubes)
                {
                    float newY = 0;
                    if (heads)
                        newY = item.GetComponent<Transform>().localPosition.y / Mathf.Sqrt(sliderBehavior.selectedCubes.Count);
                    else
                        newY = item.GetComponent<Transform>().localPosition.y * Mathf.Sqrt(sliderBehavior.selectedCubes.Count) / 2;
                    item.GetComponent<Transform>().localPosition = new Vector2(item.GetComponent<Transform>().localPosition.x, newY);
                    item.GetComponent<Floater>().SetNewPosOffSet();
                }
            }

        }

    }

    public void SetMaxSprite(Sprite sprite)
    {
        maxObject.GetComponent<Image>().sprite = sprite;
    }
}
