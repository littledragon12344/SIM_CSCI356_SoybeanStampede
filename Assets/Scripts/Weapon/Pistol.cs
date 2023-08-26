using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Pistol : MonoBehaviour, IGun
{
    // interface members implementation
    public new string name { get; protected set; }
    public int damage { get; protected set; }
    public int magazine { get; protected set; }
    public int capacity { get; protected set; }
    public int ammo { get; protected set; }
    public float fireInterval { get; protected set; }

    // serializables 
    [Header("Stats")]
    [SerializeField]
    private string gun_name = "Magnus Mega Blaster";
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
        name = gun_name;
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
        Quaternion lookDir = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(90f, 0f, 0f);
        bullet.transform.rotation = lookDir;
        // apply force to projectile
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float spreadAngle = 2f;
            Vector3 direction = Vector3.zero;
            direction.x = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f);
            direction.y = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f);
            direction = Quaternion.Euler(direction.x, direction.y, 0) * transform.forward;
            rb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
        }
        // set projectile's damage
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = damage;
            projectile.isShotBy = transform.root.tag;
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

    public void AddMagazine(int amount)
    {
        magazine += amount;
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
    }
}
