using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private int stopAngle;
    private Vector3 direction;
    private Vector3 lookDirection;
    private bool isOpen = false;
    private bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        stopAngle = (int)transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            isOpen = !isOpen;
        }

        if (stopAngle != (int)transform.eulerAngles.y)
        {
            transform.Rotate(direction * speed);
        }

        if (isOpen && lookDirection.z > 0 && stopAngle == 0)
        {
            stopAngle = 90;
            direction = Vector3.up;
        }
        else if (isOpen && lookDirection.z < 0 && stopAngle == 0)
        {
            stopAngle = -90;
            direction = Vector3.down;
        }
        return;
        if (!isOpen && stopAngle != 0)
        {
            stopAngle = 0;
            direction = -direction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;

            lookDirection = transform.position - other.transform.position;
            lookDirection.Normalize();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            lookDirection = Vector3.zero;
        }
    }
}
