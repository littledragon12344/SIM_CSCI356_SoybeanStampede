using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteract))]
public class PlayerStateController : MonoBehaviour
{
    public int CurrHeath;  //player current hp

    private int MaxHealth = 50;      //player max hp

    private PlayerInteract playerControls;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        CurrHeath = MaxHealth;//set the current hp to MaxHp

        playerControls = GetComponent<PlayerInteract>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // if paused disable the player's controls, enable if otherwise
        if (Time.timeScale == 0)
        {
            playerControls.enabled = false;
            playerMovement.enabled = false;
        }
        else
        {
            playerControls.enabled = true;
            playerMovement.enabled = true;
        }
    }

    public void Damage(int damage)
    {
        CurrHeath -= damage;//Take dmg 

        if (CurrHeath <= 0)
        {
            //animator.SetTrigger("Death");
            Die();
        }
        else
        {
            //take dmg animation (if have)
            //animator.SetTrigger("Damage");
        }
    }
    public void Heal(int Heal)
    {
        if (CurrHeath >= MaxHealth) return;//Does nth if Hp is full

        CurrHeath += Heal;//Heal
    }


    void Die()
    {
        //Death Screen HERE
        //End of game
        //Destroy(this.gameObject);

        GameStateContoller Death = FindObjectOfType<GameStateContoller>();
        Death.GameOver();//Total kills++
    }
}
