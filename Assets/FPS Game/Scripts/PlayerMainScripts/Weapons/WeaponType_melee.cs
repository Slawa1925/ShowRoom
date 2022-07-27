using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "GarageSimulator/Melee Weapon")]
public class WeaponType_melee : ScriptableObject
{
    public float range;
    public float swingRate;
    public int damage;
    public float hitForce;
    public float bulletSpread;

    public AnimationClip[] hitAnimation;
    public AnimationClip[] missAnimation;
    public AnimationClip startAnimation;
    public AnimationClip endAnimation;

    public AudioClip[] swingAudio;
    public AudioClip[] hitAudio;

    public AudioClip[] hitConcreteAudio;
    public AudioClip[] hitMetalAudio;
    public AudioClip[] hitEnemyAudio;

    public GameObject particles;
}
