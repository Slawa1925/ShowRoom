using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    ObjectPooler objectPooler;

    public GameObject explosion;
    public GameObject debris;
    public int health = 1;
    public bool destroy = true;
    [SerializeField]
    int id;

    void OnEnable()
    {
        EventManager.Damage += TakeDamage;
    }

    void OnDisable()
    {
        EventManager.Damage -= TakeDamage;
    }

    private void Start()
    {
        id = gameObject.GetInstanceID();
        objectPooler = ObjectPooler.Instance;
    }

    public void TakeDamage(int id, int damage)
    {
        if (id == this.id)
        {
            health -= damage;
            if (health <= 0)
                Explode();
        }
    }

    public void Explode(Vector3 pos)
    {
        GameObject objectToSpawn = objectPooler.SpawnFromPool("Explosion", pos, transform.rotation);

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        if (destroy)
            gameObject.SetActive(false);
    }

    public void Explode()
    {
        GameObject objectToSpawn = objectPooler.SpawnFromPool("Explosion", transform.position, transform.rotation);

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        if (destroy)
            gameObject.SetActive(false);
    }
}
