using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{
    public GameObject[] EnemyPrefab;        //enemy type
    public int[] EnemyCountLimit = { 5 }; //Spawn Limit 
    public int[] EnemyCountSpawned = { 0 }; //how many spawned
    public int[] SpawnBetweenTimer = { 10 };//timer for next spawn
    public float[] InternalTimer = { 0 }; //internal timer for each enemy
    public float Timer = 0;
    public bool SpawnerState;
    public int SpawnerDoneCounter;

    //total enemies in stage
    //randomize waves settings

    // Start is called before the first frame update
    void Start()
    {
        InternalTimer = new float[EnemyPrefab.Length];//make new incase nvr add
    }

    public void StartSpawn()
    {
        SpawnerState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnerState != true) return;// only enable if the spawner is triggered

        Timer += Time.deltaTime;

        for (int i = 0; i < EnemyPrefab.Length; i++) //repeat for each enemy 
        {
            InternalTimer[i] += Time.deltaTime;
            if (InternalTimer[i] > SpawnBetweenTimer[i])
            {
                Spawn(i);//spawn enemy after the timer 
                InternalTimer[i] = 0;//reset internal timer to 0 for each enemy
            }
        }
    }



    void Spawn(int i)//int to determine which enemy to spawn
    {
        if (EnemyCountSpawned[i] < EnemyCountLimit[i])//check amt of enemy spawn is less then the limit
        {
            Instantiate(EnemyPrefab[i], transform.position, Quaternion.identity);
            EnemyCountSpawned[i]++;//+1 to spawned counter                 

            if (EnemyCountSpawned[i] == EnemyCountLimit[i])
                SpawnerDoneCounter++;
        }
    }


    public bool SpawnDone()
    {
        // if SpawnerDoneCounter is done for all the prefab Will be true
        return SpawnerDoneCounter == EnemyPrefab.Length;
    }
}
