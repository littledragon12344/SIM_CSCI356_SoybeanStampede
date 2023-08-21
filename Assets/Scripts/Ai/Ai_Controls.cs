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

    //Enemy Animator/sound settings
    //public Animator animator;
    public AudioSource SoundSource;
    public AudioClip AttackSound;
    public AudioClip DamagedSound;
    public AudioClip DeathSound;
    public AudioClip WalkSound;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SoundSource=GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransf = player.GetComponent<Transform>();
        //rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        agent.speed = Speed;//set agents speed through this script
        CurrHeath = MaxHealth;//set the current hp to MaxHp
        SetRigidBody(true);
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
        if (agent.enabled == false) return;// cant do anything if dead

        if (playerTransf != null) // check if the player object has not been destroyed
        {
            if (distanceBetweenObjects <= AttackRange && Ranged == true)
            {
                //FOR Ranged Attacker              
                agent.destination = transform.position;//set destination to where its standing     
                //SoundSource.PlayOneShot(AttackSound);
                //animator.SetTrigger("RangedAttack");
                Attack();
            }
            else if (distanceBetweenObjects <= AttackRange)
            {
                //for Meele enemies  
                //SoundSource.PlayOneShot(AttackSound);
                //animator.SetTrigger("Attack");
                Attack();

            }
            else//move
            {
                if (IsAttacking==true) return;// Doesnt move if its attacking

                agent.destination = playerTransf.position;// set Ai destination to player position
                //WalkSound.Play();
                //SoundSource.PlayOneShot(WalkSound);           
        }
    }
 }


    void Attack()
    {
        //Attack the player 
        if (Ranged != true)
        {
            if (fireCD < Attackinterval) return;// wont attack if attack in cd
            IsAttacking = true;// enemy is attacking

            //animator.SetTrigger("Attack");

            //meele Attacks 
            //Not sure how to implement this

            //dmg
            IsAttacking = false;
        }
        else
        {
            fireCD += Time.deltaTime;

            if (fireCD < Attackinterval) return;// wont attack if attack in cd

            //ranged Attacks
            //animator.SetTrigger("RangedAttack");
            IsAttacking = true;// enemy is attacking

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
            EnemyProjectle projectile = bullet.GetComponent<EnemyProjectle>();
            if (projectile != null)
            {
                projectile.damage = dmg;
            }
            fireCD = 0; // reset
            IsAttacking = false;
        }
    }

    public void SetHealth(int damage)
    {
        CurrHeath -= damage;//Take dmg 

        if (CurrHeath == 0)
        {
           
            //KABOOOM the enemy DIES        
            //Invoke("Death", 10.0f);//delay it for 10f
            Death();
            //Drops();
        }
        else
        {
            //take dmg animation (if have)
            //animator.SetTrigger("Damage");
            //SoundSource.PlayOneShot(Damaged);
        }
    }

    void Death()
    {
        //animator.SetTrigger("Death");
        //SoundSource.PlayOneShot(DeathSound);
        //animator.enabled = false;
        SetRigidBody(false);//Make enemy kinamatic 
        agent.enabled = false;//stops the ai system
        //Total kills++
        //Drops();
        Destroy(this.gameObject,5.0f);//destroy the current enemy 
    }

    void SetRigidBody(bool State)
    {
        Rigidbody Rb = GetComponent<Rigidbody>();
        Rb.isKinematic = State;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsAttacking == true)//enable Damage only when attacking
        {
            PlayerInteract Hit = player.GetComponent<PlayerInteract>();
            Hit.SetHealth(dmg);
        }
    }

    void Drops()
    {
        //Maybe? Monster / enemy drops 
    }
}
