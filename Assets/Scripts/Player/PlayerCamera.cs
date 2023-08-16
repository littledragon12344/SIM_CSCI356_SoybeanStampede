using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // script will stop working if the player tranform is null
        if (player == null)
        {
            Debug.LogError("[" + GetType() + "] : " + "Missing player's transform");
            return;
        }

        // get the position offset between the camera and the player
        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // script will stop working if the player tranform is null
        if (player == null) return;

        // update the camera's transform
        transform.position = new Vector3(player.position.x - offset.x, transform.position.y, player.position.z - offset.z);
        //transform.LookAt(player);
    }
}
