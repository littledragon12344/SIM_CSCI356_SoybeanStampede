using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerGUIController : MonoBehaviour
{
    [Header("Player references")]
    [SerializeField]
    private PlayerInteract playerInteract;
    [SerializeField]
    private PlayerStateController playerState;

    [Header("GUI references")]
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TMP_Text weaponName;
    [SerializeField]
    private TMP_Text ammoCount;
    [SerializeField]
    private TMP_Text capacity;
    [SerializeField]
    private TMP_Text magazineCount;

    // Start is called before the first frame update
    void Start()
    {
        if (playerInteract == null)
        {
            Debug.LogError("[" + GetType() + "] : " + "Missing player interact's reference");
        }
        if (playerState == null)
        {
            Debug.LogError("[" + GetType() + "] : " + "Missing player state's reference");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteract != null)
        {
            IGun gun = playerInteract.GetCurrentlyEquiped();
            weaponName.text = gun.name;
            ammoCount.text = gun.ammo.ToString();
            capacity.text = gun.capacity.ToString();
            magazineCount.text = gun.magazine.ToString();
        }

        if (playerState != null)
        {
            healthBar.value = (float)playerState.CurrHeath / (float)playerState.MaxHealth;
        }
    }
}
