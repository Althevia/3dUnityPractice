using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int tokenCount;
    public int totalTokens;
    public Text tokenText;
    public AudioClip[] tokenSounds;
    private AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        tokenCount = 0;
        sound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Game over");
        }
        else if (other.gameObject.CompareTag("Token"))
        {
            tokenCount += 1;
            sound.clip = tokenSounds[Random.Range(0, tokenSounds.Length)];
            sound.Play();
            tokenText.text = tokenCount + "/" + totalTokens;
            Destroy(other.gameObject);

            if(tokenCount == totalTokens)
            {
                Debug.Log("Victory");
            }
        }else
        {
            Debug.Log("Unknown collision detected");
        }
    }
}
