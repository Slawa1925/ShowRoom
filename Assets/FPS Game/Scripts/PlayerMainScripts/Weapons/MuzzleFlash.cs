using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public Light flashLight;
    public ParticleSystem particles;
    public float duration;

    public IEnumerator RestartFlash()
    {
        flashLight.enabled = true;
        flashLight.intensity = 1.0f;

        particles.Clear();
        particles.time = 0;
        particles.Play();

        while (!particles.isStopped)
        {
            flashLight.intensity = Mathf.Clamp01((particles.main.duration - particles.time) / particles.main.duration);
            yield return null;
        }

        flashLight.enabled = false;
    }


}
