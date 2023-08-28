using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private float targetY;
    private Vector3 lookDirection;

    private bool isOpening = false;
    private bool isClosing = false;
    private bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        targetY = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            if (transform.localRotation.y == 0 && !isOpening)
                isOpening = true;

            if (transform.localRotation.y != 0 && !isClosing)
                isClosing = true;
        }

        if (isOpening)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, targetY, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime * 10f);

            if (transform.rotation == targetRotation) isOpening = false;
        }

        if (isClosing)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime * 10f);

            if (transform.rotation == targetRotation) isClosing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;

            lookDirection = transform.position - other.transform.position;
            lookDirection.Normalize();

            if (lookDirection.z < 0f && !isOpening) targetY = -90f;
            else if (lookDirection.z > 0f && !isOpening) targetY = 90f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }
}
