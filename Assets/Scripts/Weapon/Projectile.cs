using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
        if (collision.transform.tag == "Enemy")
        {
            // apply damage
            
            // get the GameObject that was hit
            GameObject hitObject = collision.transform.gameObject;

            // get Shootable component
            Ai_Controls target = hitObject.GetComponent<Ai_Controls>();
            // if the object has a Shootable component
         
            target.SetHealth(1);// Deal dmg 
            
        }
    }
}
