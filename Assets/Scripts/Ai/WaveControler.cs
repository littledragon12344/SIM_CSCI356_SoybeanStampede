using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class WaveControler : MonoBehaviour
{
    public Spawner[] spawners; //add/link all spawners

    //waves settings
    public int PlusWaves = 1;// Amount of Waves + Wave 1
    public int CurrentWave = 1;
    public bool WaveOngoing;


    //overide spawners
    public GameObject[] EnemyPrefab;        //enemy type
    public int[] EnemyCountLimit = { 1 }; //Spawn Limit 
    public int[] SpawnBetweenTimer ;//timer for next spawn

    // Start is called before the first frame update
    void Start()
    {

        if (spawners == null) return;//Dont overide Enemy
        if (EnemyPrefab == null) return;//Dont overide Enemy

        foreach (Spawner Spawn in spawners)
        {
            //Overrides ALl Spawner Linked to this to the WaveController Value
            Array.Copy(this.EnemyPrefab, Spawn.EnemyPrefab,this.EnemyPrefab.Length);
            Array.Copy(this.EnemyCountLimit, Spawn.EnemyCountLimit, this.EnemyCountLimit.Length);
            Array.Copy(this.SpawnBetweenTimer, Spawn.SpawnBetweenTimer, this.SpawnBetweenTimer.Length);       
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawners == null) return;// ignore
        if (CurrentWave > PlusWaves) return; // game ends 

        foreach (Spawner Spawn in spawners) // for all spawner check if the spawner is done
        {
            if (Spawn.SpawnDone() != true)  //Means that The Spawner is done Spawning
                WaveOngoing = true;         //Ongoing Wave
            else 
                WaveOngoing = false;        //Wave is Done
        }

        if (WaveOngoing == true) return;    //IF wave is ongoing 
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
                Spawn.EnemyCountLimit[i] = EnemyCountLimit[i] * CurrentWave;
            }
        }
    }
 }
