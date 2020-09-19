using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent monster;
    public GameObject player;

    //public float monsDistanceRun = 15.0f;    //When the mob should start chasing
    private float distance;
    private Vector3 dirToPlayer;
    private Vector3 newPos;

    public Player playerScript;
    public bool move = false;


    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        //Run towards player
        //if (distance < monsDistanceRun)
        if (move == true)
        {
            dirToPlayer = transform.position - player.transform.position;
            newPos = transform.position - dirToPlayer;
            monster.SetDestination(newPos);
        }
    }

    public void resetMons()
    {
        move = false;
    }
}
