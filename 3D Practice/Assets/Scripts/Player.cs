using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int tokenCount;
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
        Debug.Log("Collision detected");
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Game over");
        }
        else if (other.gameObject.CompareTag("Token"))
        {
            tokenCount += 1;
            Debug.Log("Token count: "+tokenCount);
            sound.clip = tokenSounds[Random.Range(0, tokenSounds.Length)];
            sound.Play();
            Destroy(other.gameObject);
        }
    }
}
