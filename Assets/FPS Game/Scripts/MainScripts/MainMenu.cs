using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    public GameSaveScript saveScript;
    float lastSaveTime;
    public float maxTimeWithoutSaving;
    public GameObject menuObject;
    public bool isOpened;

    public GameObject player;

    private StatsAndMovementScript playerObject;
    public StatsAndMovementScript playerStats
    {
        get
        {
            if (playerObject == null) playerObject = FindObjectOfType<StatsAndMovementScript>();
            return playerObject;
        }
    }

    public GameObject playerCamera;
    public GameObject screenshotCamera;

    public float timeScale = 1;
    readonly static float fixedTime = 0.02f;
    public float audioVolume;

    public GameObject[] menuSections;
    public GameObject saveButton;
    public GameObject backToGameButton;
    public Slider progressBar;

    public Texture2D tempScreenshot;

    public Dropdown resolutionDropdown;
    public Resolution[] resolutions;

    public float mouseSensitivity;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //fixedTime = Time.fixedDeltaTime;
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add("" + resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        saveButton.GetComponent<Button>().interactable = IsPlayerInScene();
        backToGameButton.GetComponent<Button>().interactable = IsPlayerInScene();

        if ((SceneManager.GetActiveScene().buildIndex == 0) || !IsPlayerInScene())
            OpenMenu();
        else if (IsPlayerInScene())
            CloseMenu();
    }

    bool IsPlayerInScene()
    {
        if (playerStats != null)
            return true;
        return false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ResetMenuWindows()
    {
        menuSections[0].SetActive(true);
        for (int i = 1; i < menuSections.Length; i++)
        {
            menuSections[i].SetActive(false);
        }
    }

    public void OpenMenu()
    {
        saveButton.GetComponent<Button>().interactable = IsPlayerInScene();
        backToGameButton.GetComponent<Button>().interactable = IsPlayerInScene();

        if (playerStats != null)
        {
            playerStats.NotMove(true);
            playerStats.LockCameraMovement(true);
            playerStats.CameraBlur(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            //fixedTime = Time.fixedDeltaTime;
            Time.fixedDeltaTime = fixedTime * Time.timeScale;
            AudioListener.pause = true;
        }
        else
        {
            Debug.Log("Player not found!");
        }

        menuObject.SetActive(true);
        isOpened = true;

        ResetMenuWindows();
    }

    public void CloseMenu()
    {
        if (playerStats == null)
        {
            Debug.Log("Player not found!");
            return;
        }

        menuObject.SetActive(false);
        isOpened = false;

        ResetMenuWindows();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerStats.NotMove(false);
        playerStats.LockCameraMovement(false);
        playerStats.CameraBlur(false);
        playerStats.SettingsUpdate();
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = fixedTime * Time.timeScale;
        AudioListener.pause = false;
    }

    public void MenuOpenClose()
    {
        if (isOpened == true)
            CloseMenu();
        else
            OpenMenu();
    }

    public void LoadLevel(int index)
    {
        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Scene " + index + " does't exist!");
            return;
        }
        // display scene name in ui

        for (int i = 0; i < menuSections.Length; i++)
        {
            menuSections[i].SetActive(false);
        }
        menuSections[7].SetActive(true);

        StartCoroutine(LoadAsynchronous(index));
    }

    public IEnumerator LoadAsynchronous(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = fixedTime;

        while (!operation.isDone)
        {
            Mathf.Clamp01(operation.progress / .9f);
            progressBar.value = operation.progress; 
            yield return null;
        }

        menuSections[7].SetActive(false);
        CloseMenu();
    }

    public void SetResolution(int index)
    {
        GameSettings.screenWidth = resolutions[index].width;
        GameSettings.screenHeight = resolutions[index].height;
        Screen.SetResolution(GameSettings.screenWidth, GameSettings.screenHeight, GameSettings.fullscreen);
    }

    public void WindowMode()
    {
        Debug.Log("window");
        GameSettings.fullscreen = !GameSettings.fullscreen;
        Screen.SetResolution(Screen.width, Screen.height, GameSettings.fullscreen);
    }

    public void SetTextureQulity(int index)
    {
        GameSettings.textureQulity = index;
        QualitySettings.masterTextureLimit = index;
    }

    public void SetAntialiasing(int index)
    {
        GameSettings.antialiasing = index;
        QualitySettings.antiAliasing = index;
    }

    public void SetCameraShake(float amount)
    {
        GameSettings.cameraShake = amount;
    }

    public void SetMouseSensitivity(float amount)
    {
        GameSettings.mouseSensitivity = amount;
    }

    public void SetFiltering(int index)
    {
        GameSettings.textureFiltering = index;
        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)GameSettings.textureFiltering;
    }

    public void ToggleBloom(bool toggle)
    {
        GameSettings.bloom = toggle;
    }

    public void ToggleVignette(bool toggle)
    {
        GameSettings.vignette = toggle;
    }

    public void AudioVolumeChange(float _value)
    {
        audioVolume = _value;
        AudioListener.volume = audioVolume;
    }
}
