using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject mainCamera;
    private int lives = 3;
    public Canvas deathCanvas;
    public GameObject deathScreen;
    public bool freeze = false;
    private bool dead = false;

    public Enemy[] enemyScripts;
    public MinimapCamera mmCamera;
    public Light mainLight;
    public Light endLight;

    public int tokenCount = 0;
    public int totalTokens;
    private int potionCount = 0;

    public Text tokenText;
    public Text blueText;
    public Text eventText;
    public Text[] deathTexts;
    private Color textColor;

    public AudioClip[] tokenSounds;
    private AudioSource sound;
    public AudioSource[] bgmSounds;
    private int currSong;
    public bool[] enemyNear;
    private int enemiesNear = 0;

    private bool blue = false;
    private int tipTimer = 0;
    private int countdown = 0;
    public int visTime;
    private bool mapHint = true;

    public GameObject[] enemies;
    private int newClosest = -1;
    private float closestDist = 1000;
    private float enemySpeed = 7.1f;


    // Start is called before the first frame update
    void Start()
    {
        deathScreen.SetActive(false);
        textColor = blueText.color;
        sound = gameObject.GetComponent<AudioSource>();
        currSong = 0;
        tipTimer = 700;
        foreach (AudioSource source in bgmSounds)
        {
            source.mute = true;
        }

        bgmSounds[0].mute = false;
        

        /*
        foreach (Enemy enemy in enemyScripts)
        {
            //enemy.move = true;
            enemy.startMons();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        keyControl();
        if (tokenCount == totalTokens)
        {
            playBGM(1); //Perma intense music for end
        }
        else
        {
            //Check if nearby enemies to play sound
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
        
    }

    private void FixedUpdate()
    {
        //Vision timer countdown
        if (countdown != 0 && freeze == false)
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
        if (tipTimer != 0 && freeze == false)
        {
            tipTimer -= 1;
            if (tipTimer == 0)
            {
                Debug.Log("Tip over");
                eventText.text = "";
            }
        }


        //Closest spider gets a speed boost
        foreach (Enemy enemy in enemyScripts)
        {
            if (enemy.distToPlayer() < closestDist)
            {
                closestDist = enemy.distToPlayer();
                newClosest = enemy.ID;
            }
        }
        //Debug.Log(newClosest + "  " + closestDist);
        closestDist = 1000; //Reset
        /*
        if (newClosest != closest)
        {
            //New closest enemy
            enemyScripts[newClosest].closestBuff();
            enemyScripts[closest].notClosestAnymore();
            closest = newClosest;
        }*/


        foreach (Enemy enemy in enemyScripts)
        {
            if (enemy.ID == newClosest)
            {
                enemy.setSpeed(enemySpeed + 0.2f);
            }
            else
            {
                enemy.setSpeed(enemySpeed);
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
            deathTexts[1].text = lives + " lives left";
            Image soul = GameObject.Find("Soul (2)").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
        }
        else if (lives == 1)
        {
            deathTexts[1].text = lives + " lives left";
            Destroy(GameObject.Find("Soul (2)"));
            Image soul = GameObject.Find("Soul (1)").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
        }
        else if (lives == 0)
        {
            deathTexts[1].text = "No lives left\n" + tokenCount + " coins collected";
            deathTexts[0].text = "You died!";
            Destroy(GameObject.Find("Soul (1)"));
            Image soul = GameObject.Find("Soul").GetComponent<Image>();
            soul.CrossFadeAlpha(0, 2, false);
            dead = true;
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
            blue = false;
            foreach (GameObject enemy in enemies)
            {
                enemy.layer = LayerMask.NameToLayer("Invisible");
            }
            blueText.color = textColor;
            countdown = 0;
            tipTimer = 0;
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
                eventText.text = "Escape!";
                enemySpeed = 7.8f;
                mainLight.gameObject.SetActive(false);
                endLight.gameObject.SetActive(true);
            }
            else if (tokenCount == totalTokens / 2)
            {
                enemySpeed = 7.3f;
            }
            else if (tokenCount == totalTokens * 0.9)
            {
                enemySpeed = 7.5f;
            }

            if (mapHint == true && tokenCount >= 2 * totalTokens / 3)
            {
                eventText.text = "Expand map with M";
                tipTimer = 700;
                mapHint = false;
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
            eventText.text = "Consume potion with SPACE";
            tipTimer = 500;
        }
        else if (other.gameObject.CompareTag("Exit"))
        {
            //Attempt to exit
            if (tokenCount == totalTokens)
            {
                SceneManager.LoadScene("End");
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

    //Check keypresses
    private void keyControl()
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
            if (dead == false)
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
            else
            {
                SceneManager.LoadScene("Title");
            }
            
        }
        if (Input.GetKeyDown(KeyCode.M) && freeze == false)
        {
            //Toggle zoom on map
            mmCamera.toggleZoom();
        }
    }
}
