using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private AnimationCurve interpolationCurve;
    [SerializeField]
    private LayerMask includedLayer;

    // offset from the player to the camera
    private Vector3 offset;

    // original position and rotation of the camera
    private Vector3 originPos;
    private Quaternion originRot;

    private bool isFPS = false;

    private GameObject prevHit;

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

        // set the prevHit gameobject
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f, includedLayer))
        {
            string layer = LayerMask.LayerToName(hit.collider.gameObject.layer);
            if (layer == "Ceiling")
            {
                Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = false;
            }
            prevHit = hit.collider.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // script will stop working if the player tranform is null
        if (player == null) return;

        // debug 
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleCameraMode();
        }

        float t = interpolationCurve.Evaluate(Time.deltaTime * 10f);
        // check for camera mode
        if (!isFPS)
        {
            // camera transition
            if (transform.position != originPos && transform.rotation != originRot)
            {
                // Lerp the camera to its original transform
                Vector3 newPosition = Vector3.Lerp(transform.position, originPos, t);
                Quaternion newRotation = Quaternion.Lerp(transform.rotation, originRot, t);
                transform.position = newPosition;
                transform.rotation = newRotation;
            }
            ThirdPersonCamera();
        }
        else
        {
            // camera transition
            // offset the fps's camera position
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y + 0.2f, player.position.z);
            // Lerp the camera to its FPS transform
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, t);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, player.rotation, t);
            transform.position = newPosition;
            transform.rotation = newRotation;

            FPSCamera();
        }
    }

    private void ThirdPersonCamera()
    {
        // update the camera's transform to follow the player
        transform.position = new Vector3(player.position.x - offset.x, transform.position.y, player.position.z - offset.z);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, includedLayer))
        {
            Renderer renderer;

            // enable prevHit renderer and collider
            if (prevHit != hit.collider.gameObject)
            {
                renderer = prevHit.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = true;
            }

            string layer = LayerMask.LayerToName(hit.collider.gameObject.layer);
            if (layer == "Ceiling")
            {
                // disable currHit renderer and collider
                renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = false;

                // set the prevHit gameobject
                prevHit = hit.collider.gameObject;
            }
        }
    }

    private void FPSCamera()
    {
        Renderer renderer = prevHit.GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = true;
    }

    public void ToggleCameraMode()
    {
        // toggle the camera mode
        isFPS = !isFPS;

        PlayerInteract playerControls = player.gameObject.GetComponent<PlayerInteract>();
        if (playerControls != null)
        {
            playerControls.ToggleFPSControls(isFPS);
        }

        PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ToggleFPSControls(isFPS);
        }
    }

    public void SetCameraMode(bool changeToFPS)
    {
        // toggle the camera mode
        isFPS = changeToFPS;

        PlayerInteract playerControls = player.gameObject.GetComponent<PlayerInteract>();
        if (playerControls != null)
        {
            playerControls.ToggleFPSControls(isFPS);
        }

        PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ToggleFPSControls(isFPS);
        }
    }
}
