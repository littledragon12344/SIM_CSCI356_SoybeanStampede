using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    Dictionary<IGun, GameObject> gunObject = new Dictionary<IGun, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Collider Trigger= this.GetComponent<Collider>();
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
                // check if the gameobject has IGun component
                IGun temp = child.GetComponent<IGun>();
                if (temp != null)
                {
                    // create a key value pair for gunDictionary
                    gunObject[temp] = child.gameObject;
                    // add the gun to the list
                    guns.Add(temp);
                }
                //child.GetComponent<Renderer>().enabled = false;
                //child.GetComponent<Collider>().enabled = false;

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
                //pair.Value.GetComponent<Renderer>().enabled = true;
                //pair.Value.GetComponent<Collider>().enabled = true;

                foreach (Transform part in pair.Value.GetComponentInChildren<Transform>())
                {
                    part.gameObject.SetActive(true);
                }
            }
            else
            {
                //pair.Value.GetComponent<Renderer>().enabled = false;
                //pair.Value.GetComponent<Collider>().enabled = false;
                foreach (Transform part in pair.Value.GetComponentInChildren<Transform>())
                {
                    part.gameObject.SetActive(false);
                }
            }
        }
    }

    //public void ObtainGun(IGun gun)
    //{
    //    if (gun == null) return;

    //    guns.Add(gun);
    //}
}
