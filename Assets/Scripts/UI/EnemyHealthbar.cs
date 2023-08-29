using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarsprite;
    private Camera cam;
    // Start is called before the first frame update
    public void UpdateHealthbar(float MaxHealth, float CurrHeath)
    {
        healthbarsprite.fillAmount = CurrHeath / MaxHealth;
    }

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
