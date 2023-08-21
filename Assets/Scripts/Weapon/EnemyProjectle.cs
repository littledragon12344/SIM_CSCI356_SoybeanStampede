using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectle : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3f;

    public int damage { private get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            // get the GameObject that was hit
            GameObject hitObject = collision.transform.gameObject;

            // get Shootable component
            PlayerInteract target = hitObject.GetComponent<PlayerInteract>();
            // if the object has a Shootable component

            //deal the damage
            target.SetHealth(damage);
        }

        if (collision.transform.tag == "Enemy")
        {
            //make enemy projectile trigger
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {

        }
    }

}
