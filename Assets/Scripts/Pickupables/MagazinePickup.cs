using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinePickup : MonoBehaviour
{
    [SerializeField]
    private int amount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInteract player = other.gameObject.GetComponent<PlayerInteract>();
            if (player != null)
            {
                player.AddMagazine(amount);
                Destroy(gameObject);
            }
        }
    }
}
