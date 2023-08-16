using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private Camera cam;

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

    // Update is called once per frame
    void Update()
    {
        if (cam != null && crosshair != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                crosshair.transform.position = new Vector3(hit.point.x, -1.0f, hit.point.z);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // shooting code
        }
    }
}
