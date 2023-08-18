using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour  
{
    private GameObject player;
    public Spawner SpawntriggerLink;//link spawn trigger
    public bool checkTrigg;

    // Start is called before the first frame update
    void Start()
    {
    player = GameObject.FindWithTag("Player");

    }

    public void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Player")
        {
        //sets the spawner to active
        SpawntriggerLink.StartSpawn();
        checkTrigg = true;
        }

    }
}
