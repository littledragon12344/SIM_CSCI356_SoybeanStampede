using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // enforces dependency of NavMeshAgent
public class Ai_Controls : MonoBehaviour
{
    private Transform goal;// Goal is Move towards player
    private NavMeshAgent agent;//Ai agent
    private GameObject player;//player
    private Transform playerTransf;//player local
    public float distanceBetweenObjects;//distance between enemy and player(for things like ranged enemies)

    //Enemies settings
    public bool Ranged;// tick to set enemy to ranged type
    public float AttackRange = 5f;//The Range enemy Attack distance/wont be use if ranged is false

    public float Speed =  2.5f;//enemy speed (change the mesh agent's speed)
    public int MaxHealth = 10;//enemy max hp
    public int CurrHeath ;//enemy current hp

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransf = player.GetComponent<Transform>();

        agent.speed = Speed;//set agents speed through this script
        CurrHeath = MaxHealth;//set the current hp to MaxHp

    }

    // Update is called once per frame
    void Update()
    {
        Move();// Move to player
    }

    void DistanceCheck()
    {
        //Check distance between player and this enemy
        distanceBetweenObjects = Vector3.Distance(transform.position, player.transform.position);
    }

    void Move()
    {
        DistanceCheck();//check distance

        if (playerTransf != null) // check if the player object has not been destroyed
        {
            if (distanceBetweenObjects <= AttackRange && Ranged == true)
            {
                //FOR Ranged Attacker              
                agent.destination = transform.position;//set destination to where its standing
                //Attack();
            }
            else
            {
                //for Meele enemies
                agent.destination = playerTransf.position;// set Ai destination to player position
                //Attack();
            }

        }
    }

    void Attack()
    {
        //Attack the player 
        if (Ranged != true)
        {
            //meele Attacks 
        }
        else
        {
            //ranged Attacks
            Vector3 direction = playerTransf.position - this.transform.position;
            this.transform.rotation = Quaternion.LookRotation(direction);//looktowards player

            //shoot something

        }
    }

    public void SetHealth(int damage)
    {
        CurrHeath -= damage;//Take dmg 

        if (CurrHeath == 0)
        {
            //KABOOOM the enemy DIES
            Death();
        }
    }

    void Death()
    {
        //Drops();
        Destroy(this.gameObject);//destroy the current enemy 
    }

    void Drops()
    {
        //Maybe? Monster / enemy drops 
    }
}
