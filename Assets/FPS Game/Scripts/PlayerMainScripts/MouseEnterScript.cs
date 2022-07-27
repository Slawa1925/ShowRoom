using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnterScript : MonoBehaviour
{
    public GameObject CurrentObject; // объект на который смотрит игрок
    public GameObject PlayerCamera;
    public RaycastHit Hit;
    public Vector3 RayHitDirrection;
    public float RayMaxDistance = 100;
    public LayerMask layerMask = 1 << 12;

    void Update()
    {
        RayHitDirrection = PlayerCamera.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(PlayerCamera.transform.position, RayHitDirrection, out Hit, RayMaxDistance, layerMask))
        {
            if ((Hit.collider.tag == "Null") || (Hit.collider == false)) { CurrentObject = null; }
            else
                CurrentObject = Hit.transform.gameObject;
        }
        else
        {
            CurrentObject = null;
        }
    }
}
