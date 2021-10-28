using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Nightmare_TextTrigger : MonoBehaviour
{
    int typeStatus = 0;
    public string[] myText;
    int textAmount;

    Nightmare_GamePlay gameManager;

    void Start(){
        textAmount = myText.Length;
        gameManager = FindObjectOfType<Nightmare_GamePlay>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!gameManager.isTyping){
            if (typeStatus < textAmount)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    gameManager.TypeStart(myText[typeStatus]);
                    typeStatus++;
                }
            }

        }

    }
}
