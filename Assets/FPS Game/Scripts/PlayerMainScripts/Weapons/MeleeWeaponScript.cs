using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{
    public Animation referenceToAnimation;
    public InventoryScript inventoryScript;
    RaycastHit raycastHit;
    public GameObject Player;
    public GameObject PlayerCamera;
    public LayerMask layerMask = 1 << 12;
    int AnimI;
    public bool isPlayer;
    float nextTimeToSwing;
    public WeaponType_melee weapon;
    bool Starting; // find better name!
    Vector3 HitDir;
    public GameObject bulletHole;

    void OnEnable()
    {
        StartCoroutine(WeaponPicked());
    }

    void Update()
    {
        if (isPlayer)
        {
            if ((!inventoryScript.Opened) && (!MainMenu.Instance.isOpened))
            {
                if (!Starting)
                {
                    if (Input.GetButton("Fire"))
                    {
                        TryHit();
                    }
                }
            }
        }
    }
    public IEnumerator WeaponPicked()
    {
        Starting = true;
        referenceToAnimation.clip = weapon.startAnimation;
        referenceToAnimation.Play();

        while (referenceToAnimation.isPlaying)
        {
            yield return null;
        }
        Starting = false;
    }

    public IEnumerator WeaponHidden()
    {
        Starting = true;
        referenceToAnimation.clip = weapon.endAnimation;
        referenceToAnimation.Play();

        while (referenceToAnimation.isPlaying)
        {
            yield return null;
        }

        Starting = false;
        gameObject.SetActive(false);
    }

    void TryHit()
    {
        if (Time.time < nextTimeToSwing)
            return;

        HitDir = Player.GetComponent<MouseEnterScript>().RayHitDirrection;
        referenceToAnimation.Stop();
        GetComponent<AudioSource>().PlayOneShot(weapon.swingAudio[Random.Range(0, weapon.swingAudio.Length - 1)]);

        if (Physics.Raycast(PlayerCamera.transform.position, HitDir, out raycastHit, weapon.range, layerMask))
        {
            WeaponHit();
        }
        else
        {
            WeaponSwing();
        }
    }
    void WeaponSwing()
    {
        AnimI = Random.Range(0, weapon.missAnimation.Length);
        referenceToAnimation.clip = weapon.missAnimation[AnimI];
        nextTimeToSwing = Time.time + weapon.missAnimation[AnimI].length;
        referenceToAnimation.Play();
    }

    void WeaponHit()
    {
        AnimI = Random.Range(0, weapon.hitAnimation.Length);
        referenceToAnimation.clip = weapon.hitAnimation[AnimI];
        nextTimeToSwing = Time.time + weapon.hitAnimation[AnimI].length;
        referenceToAnimation.Play();
        TryMaterial();
        CreateBulletHole(raycastHit.point, raycastHit.normal, raycastHit.transform);
        EventManager.DamageObject(raycastHit.transform.gameObject.GetInstanceID(), weapon.damage);

        if (raycastHit.transform.GetComponent<Rigidbody>() != null)
        {
            raycastHit.transform.GetComponent<Rigidbody>().AddForceAtPosition(HitDir * weapon.hitForce, raycastHit.point);
        }
    }

    void TryMaterial()
    {
        string material = raycastHit.collider.material.name.Replace(" (Instance)", "");

        switch (material)
        {
            case "Body":
                GetComponent<AudioSource>().PlayOneShot(weapon.hitEnemyAudio[Random.Range(0, weapon.hitEnemyAudio.Length)]);
                break;
            case "Metal":
                GetComponent<AudioSource>().PlayOneShot(weapon.hitMetalAudio[Random.Range(0, weapon.hitMetalAudio.Length)]);
                break;
            default:
                GetComponent<AudioSource>().PlayOneShot(weapon.hitAudio[Random.Range(0, weapon.hitAudio.Length)]);
                break;
        }
    }

    void CreateBulletHole(Vector3 _position, Vector3 _normal, Transform _parent)
    {
        if (bulletHole != null)
        {
            var g = Instantiate(bulletHole, _position, Quaternion.LookRotation(_normal));
            g.transform.position = new Vector3(_position.x + 0.01f * _normal.x, _position.y + 0.01f * _normal.y, _position.z + 0.01f * _normal.z);
        }
        else
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            sphere.transform.position = _position;
            sphere.transform.SetParent(_parent);
        }
    }
}
