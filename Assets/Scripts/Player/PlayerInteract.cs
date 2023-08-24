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
    [SerializeField]
    private GameObject gunHolder;

    private List<IGun> guns = new List<IGun>();
    private int currGunIndex = 0;

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
        if (gunHolder == null)
        {
            Debug.LogError("[" + GetType() + "] : " + "Missing gunHolder's reference");
        }

        EquipGun();
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

        // shooting code
        if (Input.GetMouseButton(0))
        {
            if (currGunIndex >= 0 && currGunIndex < guns.Count)
            {
                guns[currGunIndex].Fire();
            }
        }

        // reload code
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currGunIndex >= 0 && currGunIndex < guns.Count)
            {
                guns[currGunIndex].Reload();
            }
        }
    }

    public void EquipGun()
    {
        if (gunHolder == null) return;

        //currentGun = gunHolder.GetComponentInChildren<IGun>();
    }

    public void ObtainGun(IGun gun)
    {
        if (gun == null) return;

        guns.Add(gun);
    }
}
