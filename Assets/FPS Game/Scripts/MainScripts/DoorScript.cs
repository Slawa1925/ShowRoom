using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorAction { Взаимодействие, Закрыть, Открыть, Отпереть, Ключ_карта };
public class DoorScript : MonoBehaviour
{

    public GameObject Door;
    public Mesh DoorMesh;
    public GameObject Terminal;
    public ItemData.KeyType lockType;
    public InventoryScript Player;

    public enum Type {Автоматическая, Ручная};
    public Type doorType;

    //public bool Blocked = false;
    public bool UseTerminal = false;
    public bool RequiresPower = false;
    //public bool RequiresKey = false;
    public bool Locked = false;
    public bool openOnUnlock;
    public bool isOpened = false;
    public bool button = false;
    public bool Opening = false;

    [HideInInspector]
        public float OpeningProgress;
    public float OpeningSpeed;
    [HideInInspector]
        public Vector3 StartRotation;
    [HideInInspector]
        public Vector3 StartPosition;

    public Vector3 DeltaRotation;
    public Vector3 DeltaPosition;

    float DoorT;
    float DoorT1;

    public bool isAnimated;
    public bool Rotates;
    public bool Slides;

    public AnimationClip[] Open;
    public AnimationClip[] Close;
    public AudioClip[] DoorSound;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip keyUse;

    public Animation referenceToAnimation;


    void Start()
    {
        StartRotation = Door.transform.localEulerAngles;
        StartPosition = Door.transform.position;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>();

        if (isAnimated)
            DoorT = Open[0].length;
    }

    void Update()
    {
        if (isAnimated)
        {
            if (DoorT1 <= DoorT)
                DoorT1 += Time.deltaTime;
        }

        if ( (Slides) || (Rotates) )
        {
            if (Opening)
            {
                Quaternion target2 = Quaternion.Euler(StartRotation.x + DeltaRotation.x * OpeningProgress, StartRotation.y + DeltaRotation.y * OpeningProgress, StartRotation.z + DeltaRotation.z * OpeningProgress);
                Door.transform.localRotation = Quaternion.Slerp(transform.localRotation, target2, 1);
                
                Door.transform.position = new Vector3(StartPosition.x + OpeningProgress * DeltaPosition.x, StartPosition.y + OpeningProgress * DeltaPosition.y, StartPosition.z + OpeningProgress * DeltaPosition.z);

                if (isOpened)
                {
                    if (OpeningProgress < 1)
                    {
                        OpeningProgress += OpeningSpeed * Time.deltaTime;
                        if (OpeningProgress > 1)
                            OpeningProgress = 1;
                    }
                    else
                        Opening = false;
                }
                else
                {
                    if (OpeningProgress > 0)
                    {
                        OpeningProgress -= OpeningSpeed * Time.deltaTime;
                        if (OpeningProgress < 0)
                            OpeningProgress = 0;
                    }
                    else
                        Opening = false;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (DoorMesh != null)
        {
            Gizmos.color = Color.green;

            Vector3 DoorPosition;
            Quaternion DoorRotation;

            if (Door != null)
            {
                DoorPosition = new Vector3(Door.transform.position.x + DeltaPosition.x, Door.transform.position.y + DeltaPosition.y, Door.transform.position.z + DeltaPosition.z);
                DoorRotation = Quaternion.Euler(Door.transform.eulerAngles.x + DeltaRotation.x, Door.transform.eulerAngles.y + DeltaRotation.y, Door.transform.eulerAngles.z + DeltaRotation.z);
            }
            else
            {
                DoorPosition = new Vector3(transform.position.x + DeltaPosition.x, transform.position.y + DeltaPosition.y, transform.position.z + DeltaPosition.z);
                DoorRotation = Quaternion.Euler(transform.eulerAngles.x + DeltaRotation.x, transform.eulerAngles.y + DeltaRotation.y, transform.eulerAngles.z + DeltaRotation.z);
            }

            Gizmos.DrawWireMesh(DoorMesh, DoorPosition, DoorRotation, Door.transform.lossyScale);
        }
        else if (Door != null)
        {
            if (Door.GetComponent<MeshFilter>() != null)
            {
                DoorMesh = Door.GetComponent<MeshFilter>().sharedMesh;
            }
        }
    }

    public void UseDoor()
    {
        if (!Locked)
        {
            if (isOpened)
                CloseDoor();
            else
                OpenDoor();

            if (isAnimated)
            {
                StardDoorAnimation();
            }
        }
    }

    public void OpenDoor()
    {
        if (Door.GetComponent<AudioSource>() != null)
        {
            Door.GetComponent<AudioSource>().Stop();
            //Door.GetComponent<AudioSource>().PlayOneShot(DoorSound[Random.Range(0, DoorSound.Length)], 0.5F);
            Door.GetComponent<AudioSource>().PlayOneShot(doorOpen);

        }
        isOpened = true;
        Opening = true;
    }

    public void CloseDoor()
    {
        if (Door.GetComponent<AudioSource>() != null)
        {
            Door.GetComponent<AudioSource>().Stop();
            //Door.GetComponent<AudioSource>().PlayOneShot(DoorSound[Random.Range(0, DoorSound.Length)], 0.5F);
            Door.GetComponent<AudioSource>().PlayOneShot(doorClose);
        }
        isOpened = false;
        Opening = true;
    }

    public bool CheckForKey()
    {
        if (Player != null)
        {
            if (((Player.slot[11].itemIndex == (int)ItemData.ItemName.Ключ) || (Player.slot[11].itemIndex == (int)ItemData.ItemName.Ключ_карта)) && (Player.slot[11].keyType == lockType))
            {
                return true;
            }
        }
        return false;
    }

    public void Lock()
    {
        Locked = true;
        print("door locked");
    }
    public void Unlock()
    {
        Locked = false;
        print("door unlocked");
    }

    void StardDoorAnimation()
    {
        if (isOpened)
            referenceToAnimation.clip = Open[0];
        else
            referenceToAnimation.clip = Close[0];

        referenceToAnimation.Play();
        DoorT1 = 0;
    }
}
