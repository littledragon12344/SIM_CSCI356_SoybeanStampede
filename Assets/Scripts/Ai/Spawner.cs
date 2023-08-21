using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{
    public GameObject[] EnemyPrefeb;        //enemy type
    public int[] EnemyCountLimit   = { 5 }; //Spawn Limit 
    public int[] EnemyCountSpawned = { 0 }; //how many spawned
    public int[] SpawnBetweenTimer = { 10 };//timer for next spawn
    public float[] InternalTimer   = { 0 }; //internal timer for each enemy
    public float Timer = 0;
    public bool SpawnerState;

    //waves settings
    public bool Waves;
    public int  WaveCount;
    //total enemies in stage
    //randomize waves settings

    // Start is called before the first frame update
    void Start()
    {
        InternalTimer = new float[EnemyPrefeb.Length];//make new incase nvr add
    }

    public void StartSpawn()
    {
        SpawnerState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnerState == true)
        {
            Timer += Time.deltaTime;
       
            for (int i = 0; i < EnemyPrefeb.Length; i++) //repeat for each enemy 
            {
                InternalTimer[i] += Time.deltaTime;
                if (InternalTimer[i] > SpawnBetweenTimer[i])
                {
                    Spawn(i);//spawn enemy after the timer 
                    InternalTimer[i] = 0;//reset internal timer to 0 for each enemy
                }
            }    
        }
    }



    void Spawn(int i)//int to determine which enemy to spawn
    {
        if (EnemyCountSpawned[i] < EnemyCountLimit[i])//check amt of enemy spawn is less then the limit
        {
            Instantiate(EnemyPrefeb[i], transform.position, Quaternion.identity);
            EnemyCountSpawned[i]++;//+1 to spawned counter                 
        }
        else
        {
            //enemy spawn count maxxed + waves is enabled 
            if (Waves == true)
            {
                SpawnWaves();
            }
        }

    }
    void SpawnWaves()
    {
        for (int i = 0; i < EnemyPrefeb.Length; i++) //repeat for each enemy 
        {
            EnemyCountLimit[i] = UnityEngine.Random.Range(6, 10);// randomize 6-10 enemy 
            EnemyCountSpawned[i] = 0; //reset to 0
        }
    }
}
