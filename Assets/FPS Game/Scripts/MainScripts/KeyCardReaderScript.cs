using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class KeyCardReaderScript : MonoBehaviour
{
    public enum ButtonType { Normal, Key, KeyCard };
    public ButtonType type;

    public GameObject ButtonModel;
    public float Speed;
    float Position = 1.0f;
    bool ButtonP;
    public float DownPosition;
    public float UpPosition;
    public int MovementAxis;

    public bool doorButton = true;

    [HideInInspector]
    public bool ButtonMove;
    [HideInInspector]
    public bool Ready;
    [HideInInspector]
    public bool ButtonPress = false;
    public bool Animated = false;
    public AudioClip PressAudio;
    public AudioClip denyAudio;

    public DoorScript door; // remove dependency on door
    public UnityEvent m_MyEvent;


    void Update()
    {
        if (ButtonMove)
        {
            if (ButtonP)
            {
                if (Position < 1)
                {
                    Position += Time.deltaTime * Speed;
                }
                else
                {
                    ButtonP = false;
                    ButtonMove = false;
                }
            }
            else
            {
                if (Position > 0)
                    Position -= Time.deltaTime * Speed;
                else
                {
                    ButtonP = true;
                }
            }

            float lP = DownPosition + ((UpPosition - DownPosition) * Position);

            if (MovementAxis == 0)
                ButtonModel.transform.localPosition = new Vector3(ButtonModel.transform.localPosition.x, ButtonModel.transform.localPosition.y, lP);
            else if (MovementAxis == 1)
                ButtonModel.transform.localPosition = new Vector3(ButtonModel.transform.localPosition.x, lP, ButtonModel.transform.localPosition.z);
            else if (MovementAxis == 2)
                ButtonModel.transform.localPosition = new Vector3(lP, ButtonModel.transform.localPosition.y, ButtonModel.transform.localPosition.z);
        }
    }

    public void Press()
    {
        if (doorButton && door.Locked)
        {
            if (door.CheckForKey())
            {
                door.Unlock();

                m_MyEvent.Invoke();
                if (PressAudio != null)
                    GetComponent<AudioSource>().PlayOneShot(PressAudio, 0.5f);

                if (type == ButtonType.KeyCard)
                {
                    door.Lock();
                }
            }
            else
            {
                if (denyAudio != null)
                    GetComponent<AudioSource>().PlayOneShot(denyAudio, 0.5f);
            }
        }
        else
        {
            m_MyEvent.Invoke();
            if (PressAudio != null)
                GetComponent<AudioSource>().PlayOneShot(PressAudio, 0.5f);
        }

        if (Animated)
            ButtonMove = true;
    }
}

