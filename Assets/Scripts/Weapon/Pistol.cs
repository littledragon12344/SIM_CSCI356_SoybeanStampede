using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Pistol : MonoBehaviour, IGun
{
    // interface members implementation
    public int damage { get; protected set; }
    public int magazine { get; protected set; }
    public int capacity { get; protected set; }
    public int ammo { get; protected set; }
    public float fireInterval { get; protected set; }

    // serializables 
    [Header("Stats")]
    [SerializeField]
    private int dmg = 1;
    [SerializeField]
    private int mag = 10;
    [SerializeField]
    private int cap = 12;
    [SerializeField]
    private float interval = 0.5f;
    [SerializeField]
    private float projectileSpeed = 20f;
    [Header("References")]
    [SerializeField]
    private GameObject projectilePrefab;

    // private members
    private float fireCD = 0f;

    // Start is called before the first frame update
    void Start()
    {
        damage = dmg;
        magazine = mag;
        capacity = cap;
        ammo = capacity;
        fireInterval = interval;
    }

    // Update is called once per frame
    void Update()
    {
        fireCD += Time.deltaTime;
        fireCD = Mathf.Clamp(fireCD, 0.0f, fireInterval);
    }

    // interface functions implementation
    public void Fire()
    {
        if (ammo <= 0) return;
        if (fireCD < fireInterval) return;

        // spawn projectile
        GameObject bullet = Instantiate(projectilePrefab);
        bullet.transform.position = transform.position + transform.forward;
        // apply force to projectile
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        }
        // set projectile's damage
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = damage;
        }

        // reset cooldown and decreases ammo
        ammo--;
        fireCD = 0.0f;
    }

    public void Reload()
    {
        if (magazine <= 0) return;

        magazine--;
        ammo = capacity;
    }
}
