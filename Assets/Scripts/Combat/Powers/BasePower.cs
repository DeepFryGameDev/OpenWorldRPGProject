using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePower
{
    public string name;
    public Sprite icon;

    public AttackDB.powerClasses powerClass;

    public float baseDamage;
    public float attackRate;

    public Sprite crosshair;

    public GameObject projectile;
    public float projectileSpeed;
    public AudioClip shoot_sfx;
    public AudioClip impact_sfx;
    public int customMovement;

    public string description;
}
