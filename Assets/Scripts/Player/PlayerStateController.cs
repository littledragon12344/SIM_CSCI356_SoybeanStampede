using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteract))]
public class PlayerStateController : MonoBehaviour
{
    public int CurrHeath;  //player current hp

    public int MaxHealth = 50;      //player max hp

    private PlayerInteract playerControls;
    private PlayerMovement playerMovement;

    private float modeToggleCD = 0.0f;

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

        modeToggleCD += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ModeChangeTrigger" && modeToggleCD >= 5f)
        {
            PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
            if(playerCamera != null ) playerCamera.ToggleCameraMode();

            modeToggleCD = 0f;
            other.enabled = false;
        }

        if(other.tag == "FPSTrigger" && modeToggleCD >= 5f)
        {
            PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
            if(playerCamera != null ) playerCamera.SetCameraMode(true);

            modeToggleCD = 0f;
            other.enabled = false;
        }

        if(other.tag == "ThirdPersonTrigger" && modeToggleCD >= 5f)
        {
            PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
            if(playerCamera != null ) playerCamera.SetCameraMode(false);

            modeToggleCD = 0f;
            other.enabled = false;
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
