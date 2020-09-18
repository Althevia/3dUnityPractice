using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int tokenCount;
    public int totalTokens;
    private int potionCount;

    public Text tokenText;
    public Text blueText;

    public AudioClip[] tokenSounds;
    private AudioSource sound;

    private bool blue;
    private int countdown;
    public int visTime;

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        tokenCount = 0;
        potionCount = 0;
        blue = false;
        countdown = 0;
        sound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (potionCount > 0 && blue == false)
            {
                blueUse();
            }
        }
    }

    private void FixedUpdate()
    {
        if (countdown != 0)
        {
            countdown -= 1;
            if (countdown == 0)
            {
                blue = false;
                foreach (GameObject enemy in enemies)
                {
                    enemy.layer = LayerMask.NameToLayer("Invisible");
                }
            }
        }
    }

    private void blueUse()
    {
        Debug.Log("USE BLUE");
        //Use potion
        potionCount -= 1;
        blueText.text = potionCount + "";
        sound.clip = tokenSounds[4];
        sound.PlayOneShot(sound.clip,1);
        blue = true;
        countdown = visTime;
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer("MinimapObj");
        }
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
            sound.clip = tokenSounds[Random.Range(0, 3)];
            sound.Play();
            tokenText.text = tokenCount + "/" + totalTokens;
            Destroy(other.gameObject);

            if (tokenCount == totalTokens)
            {
                Debug.Log("Victory");
            }
        }
        else if (other.gameObject.CompareTag("BlueBottle"))
        {
            potionCount += 1;
            sound.clip = tokenSounds[3];
            sound.Play();
            blueText.text = potionCount + "";
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("Unknown collision detected");
        }
    }
}
