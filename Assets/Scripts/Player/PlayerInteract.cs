using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float rotationSpeed = 6f;

    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("[" + GetType() + "] : " + "Missing camera's reference");
        }
        if (crosshair == null)
        {
            Debug.LogError("[" + GetType() + "] : " + "Missing croshair's reference");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null && crosshair != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Ground")
                {
                    // move the crosshair to the hit point
                    crosshair.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);

                    // get the crosshair direction relative to the player
                    Vector3 targetDirection = hit.point - transform.position;
                    // freeze the x and z rotations
                    targetDirection.y = 0;
                    // interpolate the rotation
                    Quaternion lookDirection = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // shooting code
        }
    }
}
