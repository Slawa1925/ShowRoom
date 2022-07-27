using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponScript : MonoBehaviour
{
    public Animation referenceToAnimation;
    // raycasting
    RaycastHit Hit;
    Vector3 hitDir;
    public LayerMask layerMask = 1 << 12;
    // object referenses
    public InventoryScript inventoryScript;
    public GameObject Player;
    public GameObject PlayerCamera;
    // global
    public MainMenu mainMenu;
    // local variables
    public int ammo;
    public int ammoClip;
    //public bool Whit; // wtf is this name?
    public bool Starting; // starting what?
    //public float T; // no idea what that means
    public float nextTimeToFire;
    public float nextTimeToReload;    
    public MuzzleFlash flash;
    public Transform bulletSpawn;
    // ui
    public Rect GUIPos;
    // ai stuff
    public bool isPlayer = true;
    public WeaponType weapon;


    void Start()
    {
        mainMenu = GameObject.FindGameObjectWithTag("World").GetComponent<MainMenu>();
    }

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
                if (inventoryScript.FindItem((int)weapon.ammoIndex) > -1)
                    ammoClip = inventoryScript.slot[inventoryScript.FindItem((int)weapon.ammoIndex)].Count;
                else
                    ammoClip = 0;

                if (!Starting)
                {
                    if (Input.GetButtonDown("Reload"))
                    {
                        Reload();
                    }

                    if (Input.GetButton("Fire"))
                    {
                        Shoot();
                    }
                }
            }
        }
    }

    public IEnumerator WeaponPicked()
    {
        Starting = true;
        referenceToAnimation.clip = weapon.startAmimation;
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

    public void Reload()
    {
        if (ammo == weapon.ammoMax)
            return;

        if (Time.time < nextTimeToReload)
            return;

        nextTimeToReload = Time.time + 1f / weapon.reloadRate;

        if (isPlayer)
        {
            if (inventoryScript.FindItem((int)weapon.ammoIndex) == -1) { print("No ammo!"); }
            else if (inventoryScript.slot[inventoryScript.FindItem((int)weapon.ammoIndex)].Count > 0)
            {
                GetComponent<AudioSource>().PlayOneShot(weapon.reloadAudio[Random.Range(0, weapon.reloadAudio.Length - 1)]);

                referenceToAnimation.Stop();
                referenceToAnimation.clip = weapon.reloadAmimation[Random.Range(0, weapon.reloadAmimation.Length)];
                referenceToAnimation.Play();

                ammo = weapon.ammoMax;
                inventoryScript.slot[inventoryScript.FindItem((int)weapon.ammoIndex)].Count--;

                if (inventoryScript.slot[inventoryScript.FindItem((int)weapon.ammoIndex)].Count <= 0)
                {
                    inventoryScript.slot[inventoryScript.FindItem((int)weapon.ammoIndex)].Count = 0;
                    inventoryScript.slot[inventoryScript.FindItem((int)weapon.ammoIndex)].itemIndex = -1;
                }
            }
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(weapon.reloadAudio[Random.Range(0, weapon.reloadAudio.Length - 1)]);

            referenceToAnimation.Stop();
            referenceToAnimation.clip = weapon.reloadAmimation[Random.Range(0, weapon.reloadAmimation.Length)];
            referenceToAnimation.Play();

            ammo = weapon.ammoMax;
        }
    }

    public void Shoot()
    {
        if (Time.time < nextTimeToReload || Time.time < nextTimeToFire)
            return;

        nextTimeToFire = Time.time + 1f / weapon.fireRate;

        if (ammo > 0)
        {
            ammo--;
            GetComponent<AudioSource>().PlayOneShot(weapon.shootAudio[Random.Range(0, weapon.shootAudio.Length - 1)]);
            GameObject objectToSpawn = ObjectPooler.Instance.SpawnFromPool("Bullet", bulletSpawn.transform.position, bulletSpawn.transform.rotation);

            if (objectToSpawn == null)
            {
                Debug.Log("Nulll");
            }

            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

            if (pooledObj != null)
            {
                pooledObj.OnObjectSpawn();
            }

            referenceToAnimation[weapon.shootAmimation[0].name].speed = weapon.animationSpeed;
            referenceToAnimation.clip = weapon.shootAmimation[Random.Range(0, weapon.shootAmimation.Length)];
            referenceToAnimation.Play();

            StartCoroutine(flash.RestartFlash());

            Vector3 spread = new Vector3(Random.Range(weapon.bulletSpread, 1), Random.Range(weapon.bulletSpread, 1), Random.Range(weapon.bulletSpread, 1)); //bulletSpread
            hitDir = Vector3.Scale(Player.GetComponent<MouseEnterScript>().RayHitDirrection, spread);

            if (Physics.Raycast(PlayerCamera.transform.position, hitDir, out Hit, weapon.range, layerMask))
            {
                if (Hit.collider.tag == "Null") { }
                else
                {
                    StartCoroutine(BulletFlight(Hit));
                }
            }

            objectToSpawn.GetComponent<Rigidbody>().velocity = (Hit.point - bulletSpawn.transform.position) * weapon.bulletVelocity;
            objectToSpawn.GetComponent<TrailRenderer>().Clear();
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(weapon.noAmmoAudio[Random.Range(0, weapon.noAmmoAudio.Length - 1)]);
            referenceToAnimation.clip = weapon.noAmmoShootAmimation;
            referenceToAnimation.Play();
        }
    }
    IEnumerator BulletFlight(RaycastHit hit)
    {
        float travelTime = hit.distance / weapon.bulletVelocity;
        float t = 0;

        while (t < travelTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        CreateBulletHole(hit.point, hit.normal, hit.transform);
        
        if (hit.transform.GetComponent<Rigidbody>() != null)
        {
            hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(hitDir * weapon.bulletMass * weapon.bulletVelocity, hit.point);
        }

        EventManager.DamageObject(hit.transform.gameObject.GetInstanceID(), weapon.damage);
    }

    void CreateBulletHole(Vector3 _position, Vector3 _normal, Transform _parent)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.transform.position = _position;
        sphere.transform.SetParent(_parent);
    }

    void OnGUI()
    {
        if (isPlayer)
            if ((!MainMenu.Instance.isOpened) && (!Starting))
                GUI.Box(new Rect(Screen.width * 0.95f + GUIPos.x, Screen.height * 0.95f + GUIPos.y, Screen.width / 24, Screen.height / 24), "" + ammo + " / " + ammoClip);
    }
}
