using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private int MaxHealth = 50;      //player max hp
    private int CurrHeath;  //player current hp

    // Start is called before the first frame update
    void Start()
    {
        CurrHeath = MaxHealth;//set the current hp to MaxHp
    }

    // Update is called once per frame
    void Update()
    {

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
    }
}
