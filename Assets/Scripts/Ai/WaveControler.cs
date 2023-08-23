using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControler : MonoBehaviour
{
    public Spawner[] spawners; //add/link all spawners

    //waves settings
   
    public int PlusWaves = 1;// Amount of Waves + Wave 1
    public int CurrentWave = 1;

    public bool WaveOngoing;
    public int[] FromEachSpawn; 
  
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWave > PlusWaves) return; // game ends 

        foreach (Spawner Spawn in spawners)
        {
            if (Spawn.SpawnDone() != true)  //Means that The Spawner is done Spawning
                WaveOngoing = true; //Ongoing Wave
            else 
                WaveOngoing = false;
        }

        if (WaveOngoing == true) return; // dont go to next wave
        SpawnWaves();
       
    }
    void SpawnWaves()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)// if all enemys are dead
        {
            CurrentWave++;//to next Wave    
        }
        else
        {
            return;
        }

        foreach (Spawner Spawn in spawners)
        {
            for (int i = 0; i < Spawn.EnemyPrefab.Length; i++) //repeat for each enemy 
            {
                Spawn.EnemyCountSpawned[i] = 0; //reset to 0 For next Wave
                Spawn.InternalTimer[i] = 0;
                Spawn.Timer = 0;
                Spawn.SpawnerDoneCounter = 0;

                //EnemyCount per wave multiply by the wave count (e.g wave 1 - 5 enemy, Wave 2 - 10 Enemy)
                Spawn.EnemyCountLimit[i] = FromEachSpawn[i] * CurrentWave;
            }
        }
    }
 }
