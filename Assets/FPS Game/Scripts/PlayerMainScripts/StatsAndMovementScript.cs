using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class StatsAndMovementScript : MonoBehaviour
{
    public enum PlayerMainState { free, inventory };
    public enum PlayerState {crouch, run, climb};
    public PlayerState playerState;
    public PlayerMainState playerMainState;
    public bool Run = false;
    public bool Crouch = false; // присядание 
    public bool isCrouched = false; // игрок присел
    public bool CNMove = false; // 
    public bool CanClimb = false;
    public bool isClimbming = false; // карабканье по лестнице
    float climbing; // вверх || вниз
    public float Y; //
    public float ClimbSpeed = 5;
    Ladder CurLadder;
    public GameObject DeathCam;
    //bool Fly = false; // полет
    //public bool inventoryOpened = false; // открытый инвентарь
    public CharacterController controller;
    Vector3 preCrouchSize;
    public float MinHeight = 1.01f;
    public float Height = 1.65f;
    public RaycastHit Hit;
    bool Free = true;
    float MouseSensitivity; // чувствительность мыши
    [SerializeField]
    GameObject playerModel;

    public float runProgress;

    public MainMenu mainMenu;

    public float minVelocity = 1;
    public float maxVelocity = 30;
    public int dRunFOV = 5;
    public float fovSpeed = 5;
    public bool runEffect;

    public CharacterMotor MotorScript; // стандартный скрипт передвижения
    public MouseEnterScript MEnterScript; // скрипт наыедение курсора на объект
    public InventoryScript inventoryScript; // скрипт инвенторя
    public CameraShake cameraShakeScript;
    public GameObject PCamera; // 2 камеры игрока
    public GameObject Camera1;
    public GameObject Camera2;

    public float cameraFieldOfView = 70;

    public int Health = 100;

    Rect HealthGUI;
    Rect HealthBarGUI;
    Rect HealthScaleGUI;

    public float Stamina = 100;

    Rect StaminaGUI;
    Rect StaminaBarGUI;
    Rect StaminaScaleGUI;

    public int Drug;
    public Texture[] Textures;
    public AudioClip[] FallPain;
    public AudioClip[] ButtonPressAudio;

    public bool willGetFallDamage;
    public int fallVelocity = 0;
    public int damageVelocity = 10;
    public bool inventoryTesting;

    void Start()
    {
        runProgress = cameraFieldOfView;

        HealthGUI = new Rect(Screen.width / 48, Screen.height / 1.3f, 60, 60);

        HealthBarGUI = new Rect(HealthGUI.x + 60, Screen.height / 1.3f, 168, 60);
        HealthScaleGUI = new Rect(HealthBarGUI);

        StaminaGUI = new Rect(HealthGUI);
        StaminaGUI.y += 60;
        StaminaBarGUI = new Rect(HealthBarGUI);
        StaminaBarGUI.y += 60;
        StaminaScaleGUI = new Rect(StaminaBarGUI);
    }

    void Update()
    {
        HealthScaleGUI.width = HealthBarGUI.width * Health * 0.01f;
        StaminaScaleGUI.width = StaminaBarGUI.width * Stamina * 0.01f;

        #region Input
        if (Input.GetButtonDown("Cancel"))
        {
            if (!inventoryScript.Opened)
            {
                MainMenu.Instance.MenuOpenClose();
            }
            else
            {
                inventoryScript.Inventory(false);
            }
        }

        if (!MainMenu.Instance.isOpened)
        {
            if (inventoryTesting)
            {
                inventoryScript.Inventory(!inventoryScript.Opened);
                inventoryScript.Opened = false;
            }
            else if (Input.GetButtonDown("Inventory"))
            {
                //OpenInterface();
            }
            if (Input.GetButtonDown("Run"))
            {
                if ((Stamina > 10) && (!isCrouched) && (!inventoryScript.Opened))
                {
                    Run = true;
                    FRun();
                }
            }
            if (Input.GetButtonUp("Run"))
            {
                Run = false;
                FRun();
            }

            if (Input.GetButtonDown("Crouch") == true)
            {
                if ((!inventoryScript.Opened) && (!isClimbming))
                {
                    if (isCrouched)
                        FCrouch(false);
                    else
                        FCrouch(true);
                }
            }
        }
        #endregion

        if (!MainMenu.Instance.isOpened)
        {
            if (Health <= 0)
            {
                Health = 0;
                Death();
            }

            if (Run)
            {
                if (controller.velocity.magnitude < 1)
                {
                    Run = false;
                    FRun();
                }
                if (Stamina > 0)
                {
                    //Stamina -= 15 * Time.deltaTime; // пока бежит тратится выносливость
                }
                else
                {
                    Run = false;
                    FRun();
                }
            }
            else
            {
                if (Stamina < 100)
                    Stamina += 4 * Time.deltaTime;
            }

            if (isCrouched)
            {
                Ray RayUp = new Ray(transform.position, Vector3.up);
                Debug.DrawRay(transform.position, Vector3.up * MinHeight, Color.green);

                if (Physics.Raycast(RayUp, out Hit, MinHeight + 0.01f))
                {
                    if ((Hit.collider.tag == "Null") || (Hit.collider.tag == "Player"))
                    {
                        Free = true;
                    }
                    else
                    {
                        Free = false;
                    }
                }
                else
                {
                    Free = true;
                }

                if (!Crouch)
                {
                    if (Free)
                    {
                        if (controller.height != Height)
                        {
                            playerModel.transform.localScale = preCrouchSize;
                            playerModel.transform.localPosition = new Vector3(0, 0, 0);

                            controller.height = Height;
                            controller.center = new Vector3(0, 0, 0);
                            PCamera.transform.localPosition = new Vector3(0, 0.825f, 0);
                            isCrouched = false;
                        }
                    }
                }
            }

            if (isClimbming)
            {
                var Ladtrans1 = CurLadder.GetComponent<Ladder>().bottomPoint.transform.position;
                var Ladtrans2 = CurLadder.GetComponent<Ladder>().topPoint.transform.position;

                transform.position = new Vector3(Ladtrans1.x, Ladtrans1.y + (Ladtrans2.y - Ladtrans1.y) * Y, Ladtrans1.z);
                climbing = Input.GetAxis("Vertical");
                Y += climbing * ClimbSpeed * Time.deltaTime;

                if (Y > 1)
                {
                    transform.position = CurLadder.GetComponent<Ladder>().exit1.transform.position;
                    FClimb(false);
                }
                else if (Y < 0)
                {
                    transform.position = CurLadder.GetComponent<Ladder>().exit0.transform.position;
                    FClimb(false);
                }
            }

            if (runEffect)
            {
                RunEffect(Run);
            }
        }
    }

    public void UpdateMovement()
    {

    }

    public void SettingsUpdate()
    {
        Camera1.GetComponent<BloomAndFlares>().enabled = GameSettings.bloom;
        Camera1.GetComponent<FilmicVignette>().enabled = GameSettings.vignette;
    }

    public void ChangeGameSpeed(float speed)
    {
        Time.timeScale = speed;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource aSource in audioSources)
            aSource.pitch = speed;
    }

    public void Death()
    {

    }

    public void OpenInterface()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Run = false;
        FRun();
        MotorScript.canControl = false;
        LockCameraMovement(true);
        //ChangeGameSpeed(0.1f);
    }
    public void CloseInterface()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MotorScript.canControl = true;
        LockCameraMovement(false);
        ChangeGameSpeed(1);
    }

    void FRun()
    {
        if (Run)
        {
            MotorScript.movement.maxForwardSpeed = 10;
            MotorScript.movement.maxSidewaysSpeed = 8;
            MotorScript.movement.maxBackwardsSpeed = 7;
            runEffect = true;
        }
        else
        {
            MotorScript.movement.maxForwardSpeed = 6;
            MotorScript.movement.maxSidewaysSpeed = 6;
            MotorScript.movement.maxBackwardsSpeed = 6;
        }
    }

    void RunEffect(bool positive)
    {
        if (positive)
        {
            if (runProgress < cameraFieldOfView + dRunFOV)
            {
                runProgress += Time.deltaTime * fovSpeed;
                Camera1.GetComponent<Camera>().fieldOfView = runProgress;
            }
        }
        else
        {
            if (runProgress > cameraFieldOfView)
            {
                runProgress -= Time.deltaTime * fovSpeed * 2;
                Camera1.GetComponent<Camera>().fieldOfView = runProgress;
            }
            else
            {
                runEffect = false;
            }
        }
    }

    public void FCrouch(bool crouch)
    {
        if (crouch)
        {
            Crouch = true;
            isCrouched = true;
            preCrouchSize = playerModel.transform.localScale;
            playerModel.transform.localScale = new Vector3(preCrouchSize.x, preCrouchSize.y * MinHeight / Height, preCrouchSize.z);
            playerModel.transform.localPosition = new Vector3(0, (MinHeight - Height) / 2, 0);

            Run = false;
            FRun();

            controller.height = MinHeight;
            controller.center = new Vector3(0, (MinHeight - Height) / 2, 0);
            PCamera.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            Crouch = false;
            //isCrouched = false;
        }
    }

    public void FClimb(bool climb)
    {
        if (climb)
        {
            MotorScript.Work = false;
            CurLadder = MEnterScript.CurrentObject.GetComponent<Ladder>();
            MouseSensitivity = GetComponent<MouseLook>().sensitivityX;
            GetComponent<MouseLook>().sensitivityX = 0;

            FCrouch(false);
            Y = (transform.position.y - CurLadder.bottomPoint.transform.position.y) / CurLadder.climbDistance;

            if (Y < 0)
                Y = 0;
            if (Y > 1)
                Y = 1;

            transform.position = new Vector3(CurLadder.bottomPoint.transform.position.x, CurLadder.bottomPoint.transform.position.y + (CurLadder.topPoint.transform.position.y - CurLadder.bottomPoint.transform.position.y) * Y, CurLadder.bottomPoint.transform.position.z);
            transform.LookAt(new Vector3(CurLadder.transform.position.x, transform.position.y, CurLadder.transform.position.z));

            isClimbming = true;
            inventoryScript.HandHide();
        }
        else
        {
            MotorScript.Work = true;
            GetComponent<MouseLook>().sensitivityX = MouseSensitivity;

            isClimbming = false;
            inventoryScript.HandUnHide();
        }
    }

    public void NotMove(bool nmove)
    {
        if (nmove)
        {
            MotorScript.Work = false;
            CNMove = true;
        }
        else
        {
            MotorScript.Work = true;
            CNMove = false;
        }
    }

    public void LockCameraMovement(bool locked)
    {
        PCamera.GetComponent<MouseLook>().Work = !locked;
        GetComponent<MouseLook>().Work = !locked;
    }

    public void CameraBlur(bool enabled)
    {
        if (Camera1.GetComponent<Blur>() != null)
            Camera1.GetComponent<Blur>().enabled = enabled;
        if (Camera1.GetComponent<ColorCorrectionCurves>() != null)
            Camera1.GetComponent<ColorCorrectionCurves>().enabled = enabled;
        if (Camera2.GetComponent<Blur>() != null)
            Camera2.GetComponent<Blur>().enabled = enabled;
    }

    public void ShakeCamera(Vector3 pos, float power)
    {
        cameraShakeScript.StartShaking(Vector3.Distance(pos, transform.position), power);
    }

    IEnumerator Tilt(float angle, float duration, Vector3 direction, bool secondCam)
    {
        float timer = 0;
        while (timer < duration)
        {
            PCamera.transform.Rotate(angle * direction.x, angle * direction.y, angle * direction.z, Space.Self);
            timer += Time.deltaTime;
            yield return null;
        }
        PCamera.transform.Rotate(-angle * direction.x, -angle * direction.y, -angle * direction.z, Space.Self);
    }

    void ChangeMouseSensitivity()
    {
        GetComponent<MouseLook>().sensitivityX = MouseSensitivity;
        PCamera.GetComponent<MouseLook>().sensitivityY = MouseSensitivity;
    }
}
