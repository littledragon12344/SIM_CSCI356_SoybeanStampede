using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;


[RequireComponent(typeof(NavMeshAgent))] // enforces dependency of NavMeshAgent
public class Ai_Controls : MonoBehaviour
{
    //stuff
    private NavMeshAgent agent;//Ai agent
    private GameObject player;//player
    private Transform playerTransf;//player local
    public float distanceBetweenObjects;//distance between enemy and player(for things like ranged enemies)

    //Enemies settings
    public float Speed = 2.5f;//enemy speed (change the mesh agent's speed)
    private float FarSpeed;//enemy speed (change the mesh agent's speed)
    public int MaxHealth = 10;//enemy max hp
    public int CurrHeath;//enemy current hp
    public float fireCD = 0f;
    public float Attackinterval = 3.0f;
    public float AttackRange = 5;//The Range enemy Attack distance
    public int dmg = 1;
    public bool IsAttacking;
 
    //ranged settings
    public bool Ranged;// tick to set enemy to ranged type
    private float projectileSpeed = 20f;
    public GameObject projectilePrefab;

    //Enemy Animator/sound settings
    public Animator animator;
    private AudioSource SoundSource;    //only used as source
    public AudioClip AttackSound;       //atk sound
    public AudioClip DamagedSound;      //when enemy takes dmg
    public AudioClip DeathSound;        //when it dies sound
    public AudioClip WalkSound;         //walk sound


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SoundSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");//player
        playerTransf = player.GetComponent<Transform>();    //player loca
        animator = GetComponent<Animator>();
        //animator = GetComponentInParent<Animator>();


        agent.speed = Speed;
        FarSpeed = Speed + 10.0f;
        CurrHeath = MaxHealth;  //set the current hp to MaxHp
        SetRigidBody(true);     //set the enemy to not move

        if (Ranged == true)
        {
            agent.stoppingDistance = AttackRange;    //set agents speed through this script
        }
    }
    

    // Update is called once per frame
    void Update()
    {
       
        Move();// Move to player        
    }

    void DistanceCheck()//Check distance between player and this enemy
    {
        distanceBetweenObjects = Vector3.Distance(transform.position, player.transform.position);
    }


    void Move()
    {
        if (CurrHeath < 1)// stop moving and stuff if dead
        {
            animator.enabled = false;
            return;
        }

        DistanceCheck();//check distance
        if (agent.enabled == false) return;// cant do anything if dead

        if (playerTransf == null) return;// check if the player object has not been destroyed      


        if(distanceBetweenObjects < agent.stoppingDistance)
        {

            //FOR Ranged Attacker
            //SoundSource.PlayOneShot(AttackSound);
            //animator.SetTrigger("RangedAttack");
            Attack();
        }
        else if (distanceBetweenObjects <= AttackRange && Ranged != true)
        {
            //for Meele enemies  
            //SoundSource.PlayOneShot(AttackSound);          
            Attack();
            
        }
        else//move
        {
            if (IsAttacking == true) return;// Doesnt move if its attacking  

            if (distanceBetweenObjects > 20)
            {
                agent.speed = FarSpeed;// if Far make them Fast AF
                SetRigidBody(true);// Make enemy not kinamatic if range is far far
            }
            else
            {
                agent.speed = Speed;//if near slower
            }



            agent.nextPosition = transform.position;
            agent.SetDestination(playerTransf.position);
           
            animator.SetTrigger("Walking");
            //WalkSound.Play();
            //SoundSource.PlayOneShot(WalkSound);           

        }
    }


    void Attack()
    {
        //Attack the player 
        if (Ranged != true) //meele Attacks 
        {
            fireCD += Time.deltaTime;
            animator.SetTrigger("Attack");
            if (fireCD < Attackinterval) return;// wont attack if attack in cd
            IsAttacking = true;// enemy is attacking
            //animator.SetTrigger("Attack");

            StartCoroutine(MeeleTimer(2.5f));// attack duration 1.5 sec      
        }
        else
        {
           

            fireCD += Time.deltaTime;

            if (fireCD < Attackinterval)  return;// wont attack if attack in cd
            
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
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.damage = dmg;
                projectile.isShotBy = this.tag;
            }
            fireCD = 0; // reset
            IsAttacking = false;
        }
    }

    public void Damage(int damage)
    {
        SetRigidBody(false);        //Make enemy not kinamatic 
        animator.enabled = false; //disable
        CurrHeath -= damage;//Take dmg 

        if (CurrHeath < 1)
        {
            //KABOOOM the enemy DIES   
            //animator.SetTrigger("Death");
            //SoundSource.PlayOneShot(DeathSound);
            Death();
            //Drops();  //if have
        }
        else
        {
            //animator.SetTrigger("Damage");    //if have
            //SoundSource.PlayOneShot(Damaged); // when it takes damage
        }
        animator.enabled = true; //disable
        StartCoroutine(GettingHit(0.5f));// attack duration 1.5 sec      

    }

    void Death()
    {
        animator.enabled = false; //disable
        SetRigidBody(false);        //Make enemy kinamatic 
        agent.enabled = false;      //stops the ai system
                                    
        GameStateContoller Score = FindObjectOfType<GameStateContoller>();
        Score.ScoreUpdate();//Total kills++

        Destroy(this.gameObject, 5.0f);//destroy the current enemy 
    }

    void SetRigidBody(bool State)
    {
        Rigidbody Rb = GetComponent<Rigidbody>();
        Rb.isKinematic = State;
    }

    void OnTriggerEnter(Collider collision)
    {
        //enable Damage only when attacking and if they are melee
        if (IsAttacking == true && Ranged != true)
        {
            GameObject hitObject = collision.transform.gameObject;
            PlayerStateController Hit = hitObject.GetComponent<PlayerStateController>();

            if (Hit != null)
            {
                Hit.Damage(dmg);
            }
        }
    }
    
         private IEnumerator GettingHit(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        animator.enabled = true; //disable
        animator.SetTrigger("Attack");
    }

    private IEnumerator MeeleTimer(float waitTime)
    {
      
        yield return new WaitForSeconds(waitTime);
        IsAttacking = false;
        fireCD = 0; // reset
        animator.SetTrigger("Walking");
    }

    void Drops()
    {
        //Maybe? Monster / enemy drops 
    }
}
