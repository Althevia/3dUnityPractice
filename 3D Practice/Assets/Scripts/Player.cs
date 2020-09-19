using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int lives = 3;
    public Canvas deathCanvas;
    public GameObject deathScreen;
    public Enemy[] enemyScripts;

    public int tokenCount = 0;
    public int totalTokens;
    private int potionCount = 0;

    public Text tokenText;
    public Text blueText;
    public Text eventText;
    public Text deathText;
    private Color textColor;

    public AudioClip[] tokenSounds;
    private AudioSource sound;

    private bool blue = false;
    private int potionTip = 0;
    private int countdown = 0;
    public int visTime;

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        textColor = blueText.color;
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
        //Vision timer countdown
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
                blueText.color = textColor;
                sound.clip = tokenSounds[5];
                sound.PlayOneShot(sound.clip, 1);
            }
        }
        //Potion tip view
        if (potionTip != 0)
        {
            potionTip -= 1;
            if (potionTip == 0)
            {
                eventText.text = "";
            }
        }
    }

    //Use potion
    private void blueUse()
    {
        potionCount -= 1;
        blueText.text = potionCount + "";
        blueText.color = Color.blue;
        sound.clip = tokenSounds[4];
        sound.PlayOneShot(sound.clip,1);
        blue = true;
        countdown = visTime;
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer("MinimapObj");
        }
    }

    private void enemyHit()
    {
        foreach (Enemy enemy in enemyScripts)
        {
            enemy.resetMons();
        }
        deathScreen.SetActive(true);

        if (lives == 2)
        {
            deathText.text = lives + " lives left";
            Image soul = GameObject.Find("Soul (2)").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
        }
        else if (lives == 1)
        {
            deathText.text = lives + " lives left";
            Image soul = GameObject.Find("Soul (1)").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
        }
        else if (lives == 0)
        {
            deathText.text = "No lives left";
            Image soul = GameObject.Find("Soul").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
            Debug.Log("Game over");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Enemy collision detected
            lives -= 1;
            Debug.Log("Hit");
            enemyHit();
            //deathCanvas.sortingOrder = 2;   //Brings death screen to front
            //GameObject.Find("Black Panel").GetComponent<Image>().color = new Color(0, 0, 0, 255);
        }
        else if (other.gameObject.CompareTag("Token"))
        {
            //Coin collected
            tokenCount += 1;
            sound.clip = tokenSounds[Random.Range(0, 3)];
            sound.Play();
            tokenText.text = tokenCount + "/" + totalTokens;
            Destroy(other.gameObject);

            if (tokenCount == totalTokens)
            {
                Debug.Log("Collected All");
            }
        }
        else if (other.gameObject.CompareTag("BlueBottle"))
        {
            //Blue potion collected
            potionCount += 1;
            sound.clip = tokenSounds[3];
            sound.Play();
            blueText.text = potionCount + "";
            Destroy(other.gameObject);
            eventText.text = "Consume potion with space";
            potionTip = 500;
        }
        else if (other.gameObject.CompareTag("Exit"))
        {
            //Attempt to exit
            Debug.Log("EXIT");
            if (tokenCount == totalTokens)
            {
                Debug.Log("Win!");
            }
        }
        else
        {
            Debug.Log("Unknown collision detected");
        }
    }
}
