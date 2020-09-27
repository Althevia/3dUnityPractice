using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent monster;
    public int ID;
    public GameObject player;

    //public float monsDistanceRun = 15.0f;    //When the mob should start chasing
    private float distance;
    private Vector3 dirToPlayer;
    private Vector3 newPos;

    public Player playerScript;
    public bool move = false;
    private int nearDist = 26;

    private AudioSource[] audioSources;


    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<NavMeshAgent>();
        audioSources = GetComponents<AudioSource>();
        resetMons();
        startMons();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        //Triggers for suspense music
        if (distance < nearDist)
        {
            playerScript.enemyNear[ID] = true;
            playerScript.playBGM(1);
        }
        else
        {
            if (playerScript.enemyNear[ID] == true && distance > nearDist + 5)
            {
                playerScript.enemyNear[ID] = false;
            }
        }

        //Run towards player
        //if (distance < monsDistanceRun)
        if (move == true)
        {
            //dirToPlayer = transform.position - player.transform.position;
            //newPos = transform.position - dirToPlayer;
            newPos = player.transform.position;
            monster.SetDestination(newPos);
        }
    }

    public void resetMons()
    {
        move = false;
        /*
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }*/
        Vector3 resetPos;
        if (ID == 0)
        {
            resetPos = new Vector3(-38.6f, -0.94f, 70.2f);
        }else if (ID == 1)
        {
            resetPos = new Vector3(44.4f, -0.94f, -60.8f);
        }
        else
        {
            resetPos = new Vector3(36.19f, -0.94f, 74.43f);
        }
        monster.Warp(resetPos);
    }

    public void startMons()
    {
        move = true;
        /*
        foreach (AudioSource source in audioSources)
        {
            source.Play();
        }*/
    }

    //Change speed of spider
    public void setSpeed(float speed)
    {
        monster.speed = speed;
    }

    public void closestBuff()
    {
        monster.speed += 0.2f;
    }

    public void notClosestAnymore()
    {
        monster.speed -= 0.2f;
    }

    public float distToPlayer()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        return distance;
    }
}
