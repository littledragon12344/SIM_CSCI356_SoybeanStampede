using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3f;

    public int damage { private get; set; }
    public string isShotBy { private get; set; }

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
        if (collision.transform.tag == "Enemy" && isShotBy == "Player")
        {
            // get the GameObject that was hit
            GameObject hitObject = collision.transform.gameObject;

            // get Ai_Controls component
            Ai_Controls target = hitObject.GetComponent<Ai_Controls>();
            // prevent null reference
            if (target != null)
            {
                target.SetHealth(damage);
            }

        }
        else if (collision.transform.tag == "Player" && isShotBy == "Enemy")
        {
            // get the GameObject that was hit
            GameObject hitObject = collision.transform.gameObject;

            // get player state controller object
            PlayerStateController target = hitObject.GetComponent<PlayerStateController>();
            // prevent null reference
            if (target != null)
            {
                //deal the damage
                target.Damage(damage);
            }
        }

    }
}
