using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int tokenCount;
    // Start is called before the first frame update
    void Start()
    {
        tokenCount = 0;
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
            Debug.Log("Token count: "+tokenCount);
            Destroy(other.gameObject);
        }
    }
}
