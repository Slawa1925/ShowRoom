using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    // Graphics
    public static int screenWidth;
    public static int screenHeight;
    public static bool fullscreen = true;
    public static int textureQulity = 0;
    public static int textureFiltering = 1;
    public static int antialiasing = 3;
    public static bool bloom = true;
    public static bool vignette = true;
    public static float cameraShake = 1;
    public static bool runFOV;

    // Audio
    public static bool globalVolume;
    public static bool ambientVolume;
    public static bool musicVolume;
    public static bool voiceVolume;
    public static bool effectsVolume;

    // Controlls
    public static float mouseSensitivity = 1;

    // Game
    public static bool cheats;
}
