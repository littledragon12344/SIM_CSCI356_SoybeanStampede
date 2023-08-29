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
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ToggleCameraMode();
        //}

        // check for camera mode
        if (!isFPS)
        {
            ThirdPersonCamera();
        }
        else
        {
            FPSCamera();
        }
    }

    private void ThirdPersonCamera()
    {
        float yPos = transform.position.y;
        if (Mathf.Abs(player.position.y - transform.position.y) > Mathf.Abs(offset.y))
        {
            yPos = player.position.y - offset.y;
        }
        // update the camera's transform to follow the player
        transform.position = new Vector3(player.position.x - offset.x, yPos, player.position.z - offset.z);

        originPos = transform.position;
        originRot = transform.rotation;

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

        // check for camera mode
        if (!isFPS)
        {
            StartCoroutine(InterpolateCamera(originPos, originRot));
        }
        else
        {
            originPos = transform.position;
            originRot = transform.rotation;
            // camera transition
            // offset the fps's camera position
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y + 0.2f, player.position.z);
            StartCoroutine(InterpolateCamera(targetPosition, player.rotation, player));
        }

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

        // check for camera mode
        if (!isFPS)
        {
            StartCoroutine(InterpolateCamera(originPos, originRot));
        }
        else
        {
            // camera transition
            // offset the fps's camera position
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y + 0.2f, player.position.z);
            StartCoroutine(InterpolateCamera(targetPosition, player.rotation, player));
        }

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

    private IEnumerator InterpolateCamera(Vector3 targetPosition, Quaternion targetRotation, Transform parent = null)
    {
        float elapsedTime = 0f;
        float duration = 1.2f;

        while (elapsedTime < duration)
        {
            float t = interpolationCurve.Evaluate(elapsedTime / duration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // set parent after finished
        transform.parent = parent;
    }
}
