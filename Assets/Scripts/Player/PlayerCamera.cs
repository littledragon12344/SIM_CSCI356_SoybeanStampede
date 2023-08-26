using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private AnimationCurve interpolationCurve;

    // offset from the player to the camera
    private Vector3 offset;

    // original position and rotation of the camera
    private Vector3 originPos;
    private Quaternion originRot;

    // for testing only, delete line 18 when build is ready
    [SerializeField]
    private bool isFPS = false;

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
        // get the original transform information
        originPos = transform.position;
        originRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // script will stop working if the player tranform is null
        if (player == null) return;

        float t = interpolationCurve.Evaluate(Time.deltaTime * 10f);

        // check for camera mode
        if (!isFPS)
        {
            if(transform.position != originPos && transform.rotation != originRot)
            {
                // Lerp the camera to its original transform
                Vector3 newPosition = Vector3.Lerp(transform.position, originPos, t);
                Quaternion newRotation = Quaternion.Lerp(transform.rotation, originRot, t);
                transform.position = newPosition;
                transform.rotation = newRotation;
            }
            // update the camera's transform to follow the player
            transform.position = new Vector3(player.position.x - offset.x, transform.position.y, player.position.z - offset.z);
        }
        else
        {
            // offset the fps's camera position
            Vector3 targetPosition = player.position;
            targetPosition.y += 0.2f;
            // Lerp the camera to its FPS transform
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, t);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, player.rotation, t);
            transform.position = newPosition;
            transform.rotation = newRotation;
        }
    }

    public void ToggleCameraMode()
    {
        // toggle the camera mode
        isFPS = !isFPS;

        PlayerInteract playerControls = player.gameObject.GetComponent<PlayerInteract>();
        if(playerControls != null)
        {
            playerControls.ToggleFPSControls(isFPS);
        }
    }
}
