using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject Camera;
    public GameObject secondCamera;
    public float maxInfluenceDistance = 300;
    public bool allowCameraShake;
    public float shakeSpeed;

    public void StartShaking(float distance, float power)
    {
        if ((allowCameraShake) && (distance < maxInfluenceDistance))
        {
            StartCoroutine(Shake((1 - (distance / maxInfluenceDistance)) * GameSettings.cameraShake, power, false));
            if (secondCamera != null)
                StartCoroutine(Shake((1 - (distance / maxInfluenceDistance)) / 4 * GameSettings.cameraShake, power, true));
        }
    }

    IEnumerator Shake(float magnitude, float duration, bool secondCam)
    {
        Vector3 originalPos = (secondCam) ? secondCamera.transform.localPosition : Camera.transform.localPosition;
        float dx, dy, dz, elapsed = 0.0f;

        while (elapsed < duration)
        {
            dx = Random.Range(-1f, 1f) * magnitude * (1 - elapsed / duration);
            dy = Random.Range(-1f, 1f) * magnitude * (1 - elapsed / duration);
            dz = Random.Range(-1f, 1f) * magnitude * (1 - elapsed / duration);

            if (secondCam)
                secondCamera.transform.localPosition = Vector3.MoveTowards(secondCamera.transform.localPosition, new Vector3(dx, dy, dz), shakeSpeed * Time.deltaTime);
            else
                Camera.transform.localPosition = Vector3.MoveTowards(Camera.transform.localPosition, new Vector3(dx, dy, dz), shakeSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (secondCam)
            secondCamera.transform.localPosition = originalPos;
        else
            Camera.transform.localPosition = originalPos;
    }

    IEnumerator Tilt(float magnitude, float duration, bool secondCam)
    {
        while (true)
        {
            yield return null;
        }
    }
}
