using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject mainCamera;
    private int lives = 3;
    public Canvas deathCanvas;
    public GameObject deathScreen;
    public bool freeze = false;

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
    public AudioSource[] bgmSounds;
    private int currSong;
    public bool[] enemyNear;
    private int enemiesNear = 0;

    private bool blue = false;
    private int potionTip = 0;
    private int countdown = 0;
    public int visTime;

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        deathScreen.SetActive(false);
        textColor = blueText.color;
        sound = gameObject.GetComponent<AudioSource>();
        currSong = 0;
        foreach (AudioSource source in bgmSounds)
        {
            source.mute = true;
        }
        bgmSounds[0].mute = false;

        foreach (Enemy enemy in enemyScripts)
        {
            //enemy.move = true;
            enemy.startMons();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && freeze == false)
        {
            //Consume blue potion
            if (potionCount > 0 && blue == false)
            {
                blueUse();
            }
        }
        if (Input.GetKeyDown("return") && freeze == true)
        {
            //Exit from death screen
            foreach (Enemy enemy in enemyScripts)
            {
                enemy.startMons();
            }

            playBGM(0);
            deathScreen.SetActive(false);
            Debug.Log("exiting death");
            freeze = false;
        }

        foreach(bool near in enemyNear)
        {
            if (near == true)
            {
                enemiesNear += 1;
            }
        }
        if (enemiesNear == 0)
        {
            playBGM(0); //Play ambient sound;
        }
        enemiesNear = 0;
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
        freeze = true;
        deathScreen.SetActive(true);
        gameObject.transform.position = new Vector3(-57.42f,2.56f,3.94f);
        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        foreach (Enemy enemy in enemyScripts)
        {
            enemy.resetMons();
        }
        playBGM(2);

        sound.clip = tokenSounds[6]; //Flame sound
        sound.PlayOneShot(sound.clip, 1);

        if (lives == 2)
        {
            deathText.text = lives + " lives left";
            Image soul = GameObject.Find("Soul (2)").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
        }
        else if (lives == 1)
        {
            deathText.text = lives + " lives left";
            Destroy(GameObject.Find("Soul (2)"));
            Image soul = GameObject.Find("Soul (1)").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
        }
        else if (lives == 0)
        {
            deathText.text = "No lives left";
            Destroy(GameObject.Find("Soul (1)"));
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
            deathCanvas.sortingOrder = 2;   //Brings death screen to front
            GameObject.Find("Black Panel").GetComponent<Image>().color = new Color(0, 0, 0, 255);
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

    public void playBGM(int song)
    {
        if (currSong != song)
        {
            foreach(AudioSource source in bgmSounds)
            {
                source.mute = true;
            }
            currSong = song;
            bgmSounds[song].mute = false;
        }
    }
}
