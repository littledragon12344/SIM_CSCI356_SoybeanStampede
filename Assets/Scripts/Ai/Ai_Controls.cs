using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    public float Speed =  2.5f;//enemy speed (change the mesh agent's speed)
    public int MaxHealth = 10;//enemy max hp
    public int CurrHeath;//enemy current hp
    private float fireCD = 0f;
    public float Attackinterval = 3.0f;
    public float AttackRange = 5f;//The Range enemy Attack distance
    public int dmg = 1;
    private bool IsAttacking;

    //ranged settings
    public bool Ranged;// tick to set enemy to ranged type
    private float projectileSpeed = 20f;
    public GameObject projectilePrefab;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransf = player.GetComponent<Transform>();
        //rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
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
                Attack();
            }
            else if (distanceBetweenObjects <= AttackRange)
            {
                //for Meele enemies
                IsAttacking = true;
                Attack();
            }
            else//move
            {
                if (IsAttacking==true) return;// Doesnt move if its attacking

                agent.destination = playerTransf.position;// set Ai destination to player position
            }

        }
    }


    void Attack()
    {
        //Attack the player 
        if (Ranged != true)
        {
            if (fireCD < Attackinterval) return;// attack interval
            //animator.SetTrigger("Attack");

            //meele Attacks 
            //Not sure how to implement this
 
            //dmg
        }
        else
        {
            fireCD += Time.deltaTime;

            if (fireCD < Attackinterval) return;

            //ranged Attacks
            //animator.SetTrigger("RangedAttack");
            Vector3 direction = playerTransf.position - this.transform.position;
            this.transform.rotation = Quaternion.LookRotation(direction);//looktowards player

            //shoot something
            GameObject bullet = Instantiate(projectilePrefab);
            bullet.transform.position = transform.position + transform.forward;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            }
            // set projectile's damage
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.damage = dmg;
            }
            fireCD =  0; // reset
        }
    }

    public void SetHealth(int damage)
    {
        CurrHeath -= damage;//Take dmg 

        if (CurrHeath == 0)
        {
            //animator.SetTrigger("Death");
            //KABOOOM the enemy DIES        
            //Invoke("Death", 10.0f);//delay it for 10f
            Death();
            //Drops();
        }
        else
        {
            //take dmg animation (if have)
            //animator.SetTrigger("Damage");
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
