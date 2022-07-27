using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public enum ButtonType { Normal, Key, KeyCard };
    public enum Axis { x, y, z };
    public ButtonType type;

    public UnityEvent m_MyEvent;
    public AudioClip PressAudio;
    public AudioClip denyAudio;

    // Button Animation Block
    [SerializeField] bool isAnimated;
    [SerializeField] Transform buttonTransform;
    [SerializeField] float animationSpeed;
    [SerializeField] Axis movementAxis;
    float axisRelativePosition = 1.0f;
    float axisPosition;
    bool unpressButton; // meaning
    [SerializeField] float DownPosition;
    [SerializeField] float UpPosition;
    bool playButtonAnimation;


    IEnumerator ButtonAnimation()
    {
        playButtonAnimation = true;
        while (playButtonAnimation)
        {
            if (unpressButton)
            {
                if (axisRelativePosition < 1)
                {
                    axisRelativePosition += Time.deltaTime * animationSpeed;
                }
                else
                {
                    unpressButton = false;
                    playButtonAnimation = false;
                }
            }
            else
            {
                if (axisRelativePosition > 0)
                    axisRelativePosition -= Time.deltaTime * animationSpeed;
                else
                {
                    unpressButton = true;
                }
            }
            axisPosition = DownPosition + ((UpPosition - DownPosition) * axisRelativePosition);

            switch (movementAxis)
            {
                case Axis.x:
                    buttonTransform.localPosition = new Vector3(buttonTransform.localPosition.x, buttonTransform.localPosition.y, axisPosition); break;
                case Axis.y:
                    buttonTransform.localPosition = new Vector3(buttonTransform.localPosition.x, axisPosition, buttonTransform.localPosition.z); break;
                case Axis.z:
                    buttonTransform.localPosition = new Vector3(axisPosition, buttonTransform.localPosition.y, buttonTransform.localPosition.z); break;
                default: break;
            }
            yield return null;
        }
    }

    public void Press()
    {
        m_MyEvent.Invoke();
        if (PressAudio != null)
            GetComponent<AudioSource>().PlayOneShot(PressAudio, 0.5f);
        if (isAnimated)
            StartCoroutine(ButtonAnimation());
    }

    /*public void Press()
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

        if (isAnimated)
            StartCoroutine(ButtonAnimation());
    }*/
}
