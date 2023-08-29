using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MachineGun : MonoBehaviour, IGun
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
    private string gun_name = "Stryfe Flywheel Blaster";
    [SerializeField]
    private int dmg = 1;
    [SerializeField]
    private int mag = 10;
    [SerializeField]
    private int cap = 30;
    [SerializeField]
    private float interval = 0.2f;
    [SerializeField]
    private float projectileSpeed = 30f;
    [Header("References")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Animator animator;
    private AudioSource SoundSource;    //only used as source
    public AudioClip ShootSound;
    public AudioClip ReloadSound;

    // private members
    private float fireCD = 0f;

    // Start is called before the first frame update
    void Start()
    {
        SoundSource = GetComponent<AudioSource>();
        name = gun_name;
        damage = dmg;
        magazine = mag;
        capacity = cap;
        ammo = capacity;
        fireInterval = interval;

        if (animator == null)
        {
            Debug.LogError("[ " + GetType() + " ] : Missing Animator reference!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireCD += Time.deltaTime;
        fireCD = Mathf.Clamp(fireCD, 0.0f, fireInterval);

        if (animator != null)
        {
            int layerIndex = animator.GetLayerIndex("Stryfe Reload");
            if (layerIndex != -1)
            {
                // check if the animation has finished playing
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
                if (!animator.IsInTransition(layerIndex)
                    && stateInfo.normalizedTime >= 1.0f
                    && !stateInfo.IsName("None"))
                {
                    animator.SetBool("ReloadStryfe", false);
                }
            }
        }
    }

    // interface functions implementation
    public void Fire()
    {
        if (ammo <= 0) return;
        if (fireCD < fireInterval) return;
        SoundSource.PlayOneShot(ShootSound);
        // spawn projectile
        GameObject bullet = Instantiate(projectilePrefab);
        bullet.transform.position = transform.position + transform.forward;
        Quaternion lookDir = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(90f, 0f, 0f);
        bullet.transform.rotation = lookDir;
        // apply force to projectile
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float spreadAngle = 5f;
            Vector3 direction = Vector3.zero;
            direction.x = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f);
            direction.y = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f);
            direction = Quaternion.Euler(direction.x, direction.y, 0) * transform.forward;
            rb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
            //rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        }
        // set projectile's damage
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = damage;
            projectile.isShotBy = "Player";
        }

        // reset cooldown and decreases ammo
        ammo--;
        fireCD = 0.0f;
    }

    public void Reload()
    {
        if (magazine <= 0) return;
        SoundSource.PlayOneShot(ReloadSound);
        animator.SetBool("ReloadStryfe", true);

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
