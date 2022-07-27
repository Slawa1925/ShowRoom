using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragScript : MonoBehaviour {

    public GameObject obj; // 
    public GameObject PlayerCamera; // камера игрока
    public GameObject ObjPoint; // точка перед игроком в которой должен находиться предмет
    public int CurType;
    public int CurEnterType;
    public float Speed;
    public float spring = 50.0f;
    public float damper = 5.0f;
    public float distance = 0.2f;
    public Texture[] Textures;
    public float interactionDistance;
    public MouseEnterScript PME;
    public InventoryScript InventoryScr;
    public StatsAndMovementScript statAndMove;
    bool isFree = true;
    bool canInteract;
    public float maxMass = 10;
    public float maxPlayerForce;
    public AudioClip[] itemPickUp;
    public string itemName;

    void Start()
    {
        CurType = -1;
    }

    void Update()
    {
        canInteract = false;

        if (isFree)
        {
            if (PME.CurrentObject != null)
            {
                if (Vector3.Distance(PME.Hit.point, transform.position) < interactionDistance)
                {
                    if (PME.CurrentObject.tag == "Interactable")
                    {
                        canInteract = true;
                        if (PME.CurrentObject.GetComponent<ItemPicUpScript>() != null)
                            itemName = ItemData.items[(int)PME.CurrentObject.GetComponent<ItemPicUpScript>().itemPropertis].name;
                        else
                            itemName = "";
                    }
                }
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            if ((InventoryScr.Opened == false) && (statAndMove.isClimbming == false))
            {
                GetComponent<AudioSource>().PlayOneShot(statAndMove.ButtonPressAudio[Random.Range(0, statAndMove.ButtonPressAudio.Length - 1)], 0.1f);

                if (isFree)
                {
                    if (canInteract)
                    {
                        if (PME.CurrentObject.GetComponent<ButtonScript>() != null)
                        {
                            PressButton();
                        }
                        else if (PME.CurrentObject.GetComponent<Ladder>() != null)
                        {
                            statAndMove.FClimb(true);
                        }
                        else if (PME.CurrentObject.GetComponent<ItemPicUpScript>() != null)
                        {
                            if (PME.CurrentObject.GetComponent<ItemPicUpScript>().TakeItem())
                            {
                                GetComponent<AudioSource>().PlayOneShot(itemPickUp[Random.Range(0, itemPickUp.Length - 1)], 0.1f);
                            }
                        }
                        else if (PME.CurrentObject.GetComponent<Rigidbody>() != null)
                        {
                            if (PME.CurrentObject.GetComponent<Rigidbody>().mass < maxMass)
                            {
                                PickObject();
                            }
                        }
                    }
                }
                else
                {
                    if (CurType == 0)
                    {
                        Drop();
                    }
                }
            }
            else if (statAndMove.isClimbming)
            {
                statAndMove.FClimb(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isFree)
            {
                if (CurType == 0)
                {
                    Throw();
                }
            }
        }
    }

    public void PickObject()
    {
        isFree = false;
        CurType = 0;
        InventoryScr.HandHide();

        obj = PME.CurrentObject;
        obj.transform.parent = PlayerCamera.transform;
        obj.transform.position = ObjPoint.transform.position;
        obj.transform.rotation = ObjPoint.transform.rotation;

        SpringJoint springJoint = obj.AddComponent<SpringJoint>();
        springJoint.connectedBody = PlayerCamera.GetComponent<Rigidbody>();
        springJoint.spring = spring;
        springJoint.damper = damper;
        springJoint.maxDistance = distance;
        obj.GetComponent<Rigidbody>().drag = 10;
        obj.GetComponent<Rigidbody>().angularDrag = 1;
    }

    public void PushObject()
    {
        PME.CurrentObject.GetComponent<Rigidbody>().AddForce(PME.RayHitDirrection * maxPlayerForce);
    }

    public void PressButton()
    {
        PME.CurrentObject.GetComponent<ButtonScript>().Press();
    }

    public void Drop()
    {
        if (isFree == false)
        {
            CurType = -1;
            isFree = true;
            InventoryScr.HandUnHide();

            obj.transform.parent = null;
            Destroy(obj.GetComponent<SpringJoint>());
            obj.GetComponent<Rigidbody>().drag = 0;
            obj.GetComponent<Rigidbody>().angularDrag = 0.05f;
        }
    }

    public void Throw()
    {
        CurType = -1;
        obj.transform.parent = null;
        Destroy(obj.GetComponent<SpringJoint>());
        obj.GetComponent<Rigidbody>().drag = 0;
        obj.GetComponent<Rigidbody>().angularDrag = 0.05f;
        obj.GetComponent<Rigidbody>().AddForce(PME.RayHitDirrection * maxPlayerForce);
        isFree = true;
        InventoryScr.HandUnHide();
    }

    void OnGUI()
    {
        if (canInteract)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50), Textures[0]);
            GUI.Label(new Rect(Screen.width / 2 - 25, Screen.height / 2 + 25, 100, 50), itemName);
        }
    }
}
