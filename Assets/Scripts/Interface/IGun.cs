using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    string name { get; }
    int damage { get; }
    int magazine { get; }
    int capacity { get; }
    int ammo { get; }
    float fireInterval { get; }

    public abstract void Fire();
    public abstract void Reload();
    public abstract void AddMagazine(int amount);
    public abstract void AddAmmo(int amount);
}