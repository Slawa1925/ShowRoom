using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour, IPooledObject
{
    public float explosionForce = 4;
    float multiplier = 1;

    public ParticleSystem[] ps;
    public AudioSource audio;

    public void OnObjectSpawn()
    {
        for (int i=0; i < ps.Length; i++)
        {
            ps[i].Clear();
            ps[i].time = 0;
            ps[i].Play();
        }

        CreateExplosionForce();
        ExplosionEffects();
    }

    private void CreateExplosionForce()
    {
        float r = 10 * multiplier;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in cols)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }

        audio.Stop();
        audio.Play();
    }

    void ExplosionEffects()
    {
        MainMenu.Instance.playerStats.ShakeCamera(gameObject.transform.position, 0.5f);
    }
}
