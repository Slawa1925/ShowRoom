using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SurveillanceCamera : MonoBehaviour
{
    public enum CamState { Normal, Suspicious, Alarmed };
    public CamState camState;
    public float cooldownMax = 10.0f;
    public float cooldown;
    public float rotationSpeed;
    public GameObject player;
    public bool isPlayerInSight;
    public Vector3 deltaRotation;
    public Vector3 initialRotation;
    public float rotationProgress;
    public bool rotationDir;
    public Light stateLight;
    public UnityEvent m_MyEvent;
    public Color normalColor;
    public Transform cameraTransform;
    public Vector3 dir;
    RaycastHit hit;
    public float detectionDistance = 5.0f;
    public float angle = 75;
    public float suspiciousTime;
    public AudioSource audioSource;

    void Update()
    {
        /*switch (camState)
        {
            case CamState.Normal:
                if (cooldown > cooldownMax * suspiciousTime)
                    SwitchState(CamState.Suspicious);
                CameraRotation();
                break;
            case CamState.Suspicious:
                if (cooldown > cooldownMax)
                    SwitchState(CamState.Alarmed);
                else if (cooldown < cooldownMax * suspiciousTime)
                    SwitchState(CamState.Normal);
                if (isPlayerInSight) transform.LookAt(player.transform);
                break;
            case CamState.Alarmed:
                if (cooldown < cooldownMax)
                    SwitchState(CamState.Suspicious);
                if (isPlayerInSight) transform.LookAt(player.transform);
                break;
            default:
                break;
        }

        if (IsPlayerInSight())
        {
            if (cooldown < cooldownMax) cooldown += Time.deltaTime;
        }
        else
        {
            if (cooldown > 0) cooldown -= Time.deltaTime;
        }*/
    }

    void SwitchState(CamState state)
    {
        if (camState == state)
            return;

        switch (state)
        {
            case CamState.Normal:
                stateLight.color = normalColor;
                audioSource.Stop();
                break;
            case CamState.Suspicious:
                stateLight.color = Color.yellow;
                audioSource.Play();
                break;
            case CamState.Alarmed:
                stateLight.color = Color.red;
                m_MyEvent.Invoke();
                break;
            default:
                break;
        }

        camState = state;
    }

    public void IdleState()
    {
        stateLight.color = normalColor;
        audioSource.Stop();
    }
    public void SuspiciousState()
    {
        stateLight.color = Color.yellow;
        audioSource.Play();
    }
    public void AlarmedState()
    {
        stateLight.color = Color.red;
        m_MyEvent.Invoke();
    }

    public void CameraRotation()
    {
        Quaternion target2 = Quaternion.Euler(initialRotation.x + deltaRotation.x * rotationProgress, initialRotation.y + deltaRotation.y * rotationProgress, initialRotation.z + deltaRotation.z * rotationProgress);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target2, 1);

        if (rotationDir)
        {
            if (rotationProgress < 1)
            {
                rotationProgress += rotationSpeed * Time.deltaTime;
            }
            else
            {
                rotationDir = false;
            }
        }
        else
        {
            if (rotationProgress > -1)
            {
                rotationProgress -= rotationSpeed * Time.deltaTime;
            }
            else
            {
                rotationDir = true;
            }
        }

    }

    public bool IsPlayerInSight()
    {
        dir = player.transform.position - transform.position;
        dir.Normalize();

        if (Vector3.Angle(transform.forward, dir) < angle/2)
        {
            Debug.DrawRay(transform.position, dir*detectionDistance, Color.red);
            if (Physics.Raycast(transform.position, dir, out hit, detectionDistance))
            {
                
                if (hit.collider.tag == "Player")
                {
                    isPlayerInSight = true;
                    return true;
                }
            }
        }
        isPlayerInSight = false;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 vector = transform.forward;
        vector = Quaternion.AngleAxis(angle/2, transform.right) * vector;

        for (int i = 0; i < 16; i++)
        {
            vector = Quaternion.AngleAxis(22.5f * i, transform.forward) * vector;
            Debug.DrawRay(transform.position, vector * detectionDistance, stateLight.color);
        }
    }
}
