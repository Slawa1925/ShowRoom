using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsScript : MonoBehaviour
{

    public GameObject Player;
    public CharacterController controller;

    [System.Serializable]
    public class steps
    {

        public AudioClip[] Normal;
        public AudioClip[] Run;

    }

    public steps[] StepS;

    float T;
    float T1;

    public float t_normal = 0.5f;
    public float t_run = 0.5f;

    RaycastHit Hit;
    float length = 2;

    public StatsAndMovementScript Script1;


    void Update()
    {

        if ((controller.isGrounded) && (controller.velocity.magnitude > 3))
        {

            Ray X = new Ray(transform.position, Vector3.down);

            if (Script1.Run)
                T1 = t_run;
            else
                T1 = t_normal;

            if (T <= T1)
            {
                T += Time.deltaTime;
            }
            if (T >= T1)
            {

                if ((Physics.Raycast(X, out Hit, length)) && (Hit.collider.sharedMaterial))
                {

                    if (Hit.collider.sharedMaterial == null)
                        print("!sharedMaterial");

                    if ((Hit.collider.sharedMaterial.name == "Concrete (Instance)") || (Hit.collider.sharedMaterial.name == "Concrete"))
                    {
                        StepAudio(0);
                    }
                    if ((Hit.collider.sharedMaterial.name == "Metal (Instance)") || (Hit.collider.sharedMaterial.name == "Metal"))
                    {
                        StepAudio(1);
                    }
                    if ((Hit.collider.sharedMaterial.name == "Dirt (Instance)") || (Hit.collider.sharedMaterial.name == "Dirt"))
                    {
                        StepAudio(2);
                    }
                    if ((Hit.collider.sharedMaterial.name == "Wood (Instance)") || (Hit.collider.sharedMaterial.name == "Wood"))
                    {
                        StepAudio(3);
                    }
                    if ((Hit.collider.sharedMaterial.name == "MetalGrate (Instance)") || (Hit.collider.sharedMaterial.name == "MetalGrate"))
                    {
                        StepAudio(4);
                    }

                }

            }

        }

    }

    void StepAudio(int mat)
    {

        if (Script1.Run)
        {

            GetComponent<AudioSource>().PlayOneShot(StepS[mat].Run[Random.Range(0, StepS[mat].Run.Length)]);

        }
        else
        {

            GetComponent<AudioSource>().PlayOneShot(StepS[mat].Normal[Random.Range(0, StepS[mat].Normal.Length)]);

        }

        T = 0;

    }
}
