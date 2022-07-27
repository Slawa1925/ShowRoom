using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "GarageSimulator/Ranged Weapon")]
public class WeaponType : ScriptableObject
{
    // weapon properties
    public float range;
    public int ammoMax;
    public float bulletVelocity;
    public float bulletMass = 1;
    public float animationSpeed;
    public float fireRate;
    public float reloadRate;
    public int damage;
    public float bulletSpread;
    public ItemData.ItemName ammoIndex;

    // weapon animation
    public AnimationClip[] shootAmimation;
    public AnimationClip noAmmoShootAmimation;
    public AnimationClip[] reloadAmimation;
    public AnimationClip startAmimation;
    public AnimationClip endAnimation;

    // weapon sounds
    public AudioClip[] shootAudio;
    public AudioClip[] reloadAudio;
    public AudioClip[] hitAudio;
    public AudioClip[] noAmmoAudio;

    // weapon object refs
    public GameObject Particles;
    public GameObject bulletHole;
}
