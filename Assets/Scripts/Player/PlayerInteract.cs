using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float rotationSpeed = 6f;
    [SerializeField]
    private float maxPitch = 45f;
    [SerializeField]
    private LayerMask includedLayer;

    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private GameObject gunHolder;

    // guns variables
    private List<IGun> guns = new List<IGun>();
    private int currGunIndex = 0;
    Dictionary<IGun, GameObject> gunObject = new Dictionary<IGun, GameObject>();

    // fps variables
    private float vertRotation = 0f;
    private float horRotation = 0f;

    private bool shouldWait = false;

    // Start is called before the first frame update
    void Start()
    {
        Collider Trigger = this.GetComponent<Collider>();
        Trigger.isTrigger = true; //enable player's capsule or watever coilder so the enemy can Meele attack

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

        if (gunHolder != null)
        {
            // get all children gameobjects of gunHolder
            foreach (Transform child in gunHolder.GetComponentInChildren<Transform>())
            {
                if (!child.gameObject.activeInHierarchy) continue;

                // check if the gameobject has IGun component
                IGun temp = child.GetComponent<IGun>();
                if (temp != null)
                {
                    // create a key value pair for gunDictionary
                    gunObject[temp] = child.gameObject;
                    // add the gun to the list
                    guns.Add(temp);
                }
                // disable the collider
                child.GetComponent<Collider>().enabled = false;
                // disable the children
                foreach (Transform part in child.GetComponentInChildren<Transform>())
                {
                    part.gameObject.SetActive(false);
                }
            }
            EquipGun(0);
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
        // check for null reference and check if the crosshair is active or not
        if (cam != null && crosshair != null && crosshair.activeInHierarchy)
        {
            ThirdPersonControls();
        }

        if (crosshair != null && !crosshair.activeInHierarchy)
        {
            FPSControls();
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

        // use middle mouse to change weapon
        MouseScroll();
    }

    private void ThirdPersonControls()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, includedLayer))
        {
            if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Ground")
            {
                // move the crosshair to the hit point
                crosshair.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);

                // get the crosshair direction relative to the player
                Vector3 targetDirection = hit.point - transform.position;
                // freeze the x and z rotations
                targetDirection.y = 0;
                // interpolate the rotation of the player
                Quaternion lookDirection = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);

                if (gunHolder != null)
                {
                    // get the crosshair direction relative to the player
                    targetDirection = hit.point - gunHolder.transform.position;
                    targetDirection.y += 0.75f;
                    // interpolate the rotation of the gun
                    lookDirection = Quaternion.LookRotation(targetDirection);
                    gunHolder.transform.rotation = Quaternion.Lerp(gunHolder.transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void FPSControls()
    {
        if (shouldWait) return;

        horRotation = transform.localEulerAngles.y;

        // get the mouse input and calculate the rotation
        vertRotation -= Input.GetAxis("Mouse Y") * rotationSpeed;
        horRotation += Input.GetAxis("Mouse X") * rotationSpeed;

        // clamp the vertical rotation
        vertRotation = Mathf.Clamp(vertRotation, -maxPitch, maxPitch);

        // update player's transform
        transform.rotation = Quaternion.Euler(new Vector3(0, horRotation, 0));

        // update camera's transform
        if (cam != null)
        {
            cam.transform.rotation = Quaternion.Euler(new Vector3(vertRotation, horRotation, 0));
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        }

        // update gunholder's transform
        if (gunHolder != null && cam != null)
        {
            Vector3 FPScrosshair = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
            Vector3 crosshairDir = cam.ScreenToWorldPoint(FPScrosshair) - gunHolder.transform.position;
            Quaternion lookDir = Quaternion.LookRotation(crosshairDir + cam.transform.forward * 10f);
            gunHolder.transform.rotation = lookDir;
        }
    }

    private void MouseScroll()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        int weaponIndex = currGunIndex;

        if (scrollInput < 0)
        {
            weaponIndex = Mathf.Clamp(weaponIndex + 1, 0, guns.Count - 1);
        }
        if (scrollInput > 0)
        {
            weaponIndex = Mathf.Clamp(weaponIndex - 1, 0, guns.Count - 1);
        }

        if (weaponIndex != currGunIndex)
        {
            EquipGun(weaponIndex);
        }
    }

    private void EquipGun(int index)
    {
        if (index < 0 || index >= guns.Count) return;

        currGunIndex = index;

        foreach (KeyValuePair<IGun, GameObject> pair in gunObject)
        {
            if (pair.Key == guns[currGunIndex])
            {
                // enable the collider
                pair.Value.GetComponent<Collider>().enabled = true;
                // enable the children
                foreach (Transform part in pair.Value.GetComponentInChildren<Transform>())
                {
                    part.gameObject.SetActive(true);
                }
            }
            else
            {
                // disable the collider
                pair.Value.GetComponent<Collider>().enabled = false;
                // disable the children
                foreach (Transform part in pair.Value.GetComponentInChildren<Transform>())
                {
                    part.gameObject.SetActive(false);
                }
            }
        }
    }

    public void AddMagazine(int amount)
    {
        if (guns.Count <= 0) return;

        if (guns[currGunIndex] != null)
        {
            guns[currGunIndex].AddMagazine(amount);
        }
    }

    public void AddAmmo(int amount)
    {
        if (guns.Count <= 0) return;

        if (guns[currGunIndex] != null)
        {
            guns[currGunIndex].AddAmmo(amount);
        }
    }

    public void ToggleFPSControls(bool isFPS)
    {
        if (crosshair != null)
        {
            crosshair.SetActive(!isFPS);
        }

        shouldWait = true;
        StartCoroutine(Wait(1f));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        shouldWait = false;
    }
}
