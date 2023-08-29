using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarsprite;
    // Start is called before the first frame update
    public void UpdateHealthbar(float MaxHealth, float CurrHeath)
    {
        healthbarsprite.fillAmount = CurrHeath / MaxHealth;
    }
}
