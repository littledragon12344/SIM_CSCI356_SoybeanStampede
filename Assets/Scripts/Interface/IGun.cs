using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    int damage { get; }
    int magazine { get; }
    int capacity { get; }
    int ammo { get; }
    float fireInterval { get; }

    public abstract void Fire();
    public abstract void Reload();
}