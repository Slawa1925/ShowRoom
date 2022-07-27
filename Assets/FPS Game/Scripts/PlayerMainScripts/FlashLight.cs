using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject lightSource;
    public AudioSource audioSource;
    public AudioClip switchSound;
    public AudioClip lightSound;
    public InventoryScript inventoryScript;
    public MainMenu mainMenu;
    public float power;
    public float maxPower;
    public float powerDrain;
    public bool flashLightState;
    public Rect rect;


    void Start ()
    {
        mainMenu = GameObject.FindGameObjectWithTag("World").GetComponent<MainMenu>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Flashlight"))
        {
            if ((inventoryScript.Opened == false) && (!MainMenu.Instance.isOpened))
                SwitchLight(!flashLightState);
        }

        if (flashLightState)
        {
            if (power > 0)
            {
                power -= powerDrain * Time.deltaTime;
            }
            else
            {
                int batterySlotIndex = inventoryScript.FindItem((int)ItemData.ItemName.Батарейка);
                if (inventoryScript.FindItem((int)ItemData.ItemName.Батарейка) != -1)
                {
                    inventoryScript.slot[batterySlotIndex].Count--;

                    if (inventoryScript.slot[batterySlotIndex].Count <= 0)
                    {
                        inventoryScript.slot[batterySlotIndex].Count = 0;
                        inventoryScript.slot[batterySlotIndex].itemIndex = -1;
                    }

                    power = maxPower;
                }
                else
                {
                    power = 0;
                    SwitchLight(false);
                }
            }
        }
    }

    public void SwitchLight(bool state)
    {
        flashLightState = state;
        lightSource.SetActive(state);
        audioSource.Play();
    }

    void OnGUI()
    {
        if (!MainMenu.Instance.isOpened)
            GUI.Box(rect, "Батарейка: " + power);
    }
}
