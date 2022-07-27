/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class Menu_1Script : MonoBehaviour {

    [System.Serializable]
    public class config
    {

        var Path : String; // 
    var Resolution : Vector2; // 
    var AntiAliasing : int; // 0 - нет;
    var Windowed : boolean; // 
    var Bloom : boolean; // 
    var Shadows : int; // 0 - нет;
    var ShadowQuality : int; // 
    var Texture : int; // 
    var Filtration : int; // 

    var Mousesensitivity : float; //

    var AudioVolume : float; // 

}
    var Config : config; // 

public class button
    {

        var GUIinfo : Rect; // 
    var Name : String; // 
    var Press : boolean; // 

}
    var Button : button[]; // 

public class settings
    {

        var GUIinfo : Rect; // 
    var GUIinfo2 : Rect; //
    var Name : String; // 
    var Type : int; // 0 - кнопка; 1 - слайдер; 2 - разрешение;
    var Press : boolean; // 

}
    var Settings : settings[]; // 

var LoadTexture : Texture;

var SavePath : String; // путь к файлу сохранения

var ResolList : boolean; // 

var StandResol : Vector2[]; // стандартные разрешения экрана

var MenuType : int; // 0 - главное; 1 - графика; 2 - загрузки;

var N : int;
var LoadTimer = false;
    var OnPlayer = false;
    var CurLoadingLevel : int;
var CurCamera : GameObject[];

var Lines : String[];
var SavingScript : Saving;

var ButtonsCount : int = 5;

var ScreenResolution : Vector3;


function Start()
    {

        SavePath = Application.dataPath + "/Saves/InventorySave.txt";
        Config.Path = Application.dataPath + "/MainConfig.txt";

        StandResol[StandResol.length - 1] = Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

        print(Screen.currentResolution.width);
        print(Screen.width);

        if (File.Exists(Config.Path))
            Load();
        else
        {
            Config.Resolution = Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            Screen.SetResolution(Config.Resolution.x, Config.Resolution.y, false);
            Save();
        }

        //var ButtonsCount : int = 5;
        ButtonsCount = 5;

        if (OnPlayer == false)
        {

            Time.timeScale = 1;
            ButtonsCount = 4;
            AudioListener.pause = false;
            Screen.lockCursor = false;

            OptionsUpdate();

            var ScreenFull = false;

            if (Config.Windowed == 0)
                ScreenFull = false;
            else
                ScreenFull = true;

            Screen.SetResolution(Config.Resolution.x, Config.Resolution.y, ScreenFull);

        }
        else
        {

            SavingScript = GetComponent(Saving);

        }

        LoadTexture = Resources.Load("Save01") as Texture;

        Config.Resolution = Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

        //StandResol[StandResol.length - 1] = Vector2( Screen.currentResolution.width, Screen.currentResolution.height );

        //for ( StandResol)
        //StandResol = Vector2( Screen.resolutions.width, Screen.resolutions.height );

        if (File.Exists(Config.Path))
            print("Yes");
        else
            print("No");

        for (var i:int; i < ButtonsCount; i++ ) {

            Button[i].GUIinfo.x = Screen.width / 2 - Button[i].GUIinfo.width / 2;
            Button[i].GUIinfo.y = Screen.height / 5 + Button[i].GUIinfo.height * i * 1.2;

        }

        for (var j:int; j < 9; j++ ) {

            Settings[j].GUIinfo.width = Button[0].GUIinfo.width;
            Settings[j].GUIinfo.height = Button[0].GUIinfo.height;

            Settings[j].GUIinfo2.width = Settings[j].GUIinfo.width;
            Settings[j].GUIinfo2.height = Settings[j].GUIinfo.height;

            Settings[j].GUIinfo.x = Screen.width / 4 - Settings[j].GUIinfo.width / 2;
            Settings[j].GUIinfo.y = Screen.height / 5 + Settings[j].GUIinfo.height * j * 1.1;

            Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Screen.width / 15.3;//110;
            Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

            if (j == 0)
            {

                Settings[j].GUIinfo2.height = Screen.height / 30;//35

            }

            if (j == 1)
            {

                Settings[j].GUIinfo.x += Settings[j].GUIinfo.width + Screen.width / 10;
                Settings[j].GUIinfo.y = Screen.height / 5 + Settings[j].GUIinfo.height * (j - 1) * 1.1;

                Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Screen.width / 28;//90;
                Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

                Settings[j].GUIinfo2.width = Screen.width / 56;//20;
                Settings[j].GUIinfo2.height = Screen.width / 56;//20;

            }

            if (j == 7)
            {

                Settings[j].GUIinfo.width = Settings[7].GUIinfo.width;
                Settings[j].GUIinfo.height = Settings[7].GUIinfo.height;

                Settings[j].GUIinfo.x = Settings[2].GUIinfo.x + Settings[2].GUIinfo.width + Settings[2].GUIinfo2.width;
                Settings[j].GUIinfo.y = Settings[2].GUIinfo.y;

                Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Settings[j].GUIinfo.width;
                Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

            }
            if (j == 8)
            {

                Settings[j].GUIinfo.width = Settings[7].GUIinfo.width;
                Settings[j].GUIinfo.height = Settings[7].GUIinfo.height;

                Settings[j].GUIinfo.x = Settings[7].GUIinfo.x;
                Settings[j].GUIinfo.y = Settings[3].GUIinfo.y;

                Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Settings[j].GUIinfo.width;
                Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

            }

        }

    }

    function ResolutionUpdate()
    {

        for (var i:int; i < ButtonsCount; i++ ) {

            Button[i].GUIinfo.x = Screen.width / 2 - Button[i].GUIinfo.width / 2;
            Button[i].GUIinfo.y = Screen.height / 5 + Button[i].GUIinfo.height * i * 1.2;

        }

        for (var j:int; j < 9; j++ ) {

            Settings[j].GUIinfo.width = Button[0].GUIinfo.width;
            Settings[j].GUIinfo.height = Button[0].GUIinfo.height;

            Settings[j].GUIinfo2.width = Settings[j].GUIinfo.width;
            Settings[j].GUIinfo2.height = Settings[j].GUIinfo.height;

            Settings[j].GUIinfo.x = Screen.width / 4 - Settings[j].GUIinfo.width / 2;
            Settings[j].GUIinfo.y = Screen.height / 5 + Settings[j].GUIinfo.height * j * 1.1;

            Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Screen.width / 15.3;//110;
            Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

            if (j == 0)
            {

                Settings[j].GUIinfo2.height = Screen.height / 30;//35

            }

            if (j == 1)
            {

                Settings[j].GUIinfo.x += Settings[j].GUIinfo.width + Screen.width / 10;
                Settings[j].GUIinfo.y = Screen.height / 5 + Settings[j].GUIinfo.height * (j - 1) * 1.1;

                Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Screen.width / 28;//90;
                Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

                Settings[j].GUIinfo2.width = Screen.width / 56;//20;
                Settings[j].GUIinfo2.height = Screen.width / 56;//20;

            }

            if (j == 7)
            {

                Settings[j].GUIinfo.width = Settings[7].GUIinfo.width;
                Settings[j].GUIinfo.height = Settings[7].GUIinfo.height;

                Settings[j].GUIinfo.x = Settings[2].GUIinfo.x + Settings[2].GUIinfo.width + Settings[2].GUIinfo2.width;
                Settings[j].GUIinfo.y = Settings[2].GUIinfo.y;

                Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Settings[j].GUIinfo.width;
                Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

            }
            if (j == 8)
            {

                Settings[j].GUIinfo.width = Settings[7].GUIinfo.width;
                Settings[j].GUIinfo.height = Settings[7].GUIinfo.height;

                Settings[j].GUIinfo.x = Settings[7].GUIinfo.x;
                Settings[j].GUIinfo.y = Settings[3].GUIinfo.y;

                Settings[j].GUIinfo2.x = Settings[j].GUIinfo.x + Settings[j].GUIinfo.width;
                Settings[j].GUIinfo2.y = Settings[j].GUIinfo.y;

            }

        }

    }

    function Update()
    {

        ScreenResolution.x = Screen.currentResolution.width;
        ScreenResolution.y = Screen.width;

    }


    function OnGUI()
    {

        GUI.Box(Rect(0, 0, 200, 50), "" + ScreenResolution);

        if (MenuType == 0)
        {

            if (OnPlayer == false)
            {

                for (var i:int; i < 4; i++ ) {

                    Button[i].Press = GUI.Button(Button[i].GUIinfo, Button[i].Name);

                }

                if (Button[0].Press)
                {

                    MenuType = 2;

                }

                if (Button[1].Press)
                {

                    MenuType = 3;

                }

                if (Button[2].Press)
                {

                    MenuType = 1;

                }

                if (Button[3].Press)
                {

                    Application.Quit();

                }

            }
            else if (SavingScript.Menu == true)
            {

                for (var i1:int; i1 < 5; i1++ ) {

                    Button[i1].Press = GUI.Button(Button[i1].GUIinfo, Button[i1].Name);

                }

                if (Button[0].Press)
                { // продолжить

                    SavingScript.Menu = false;
                    BlurCamera(false);
                    SavingScript.MenuClose();

                }

                if (Button[1].Press)
                { // сохранить

                    MenuType = 5;

                }

                if (Button[2].Press)
                { // загрузить

                    MenuType = 3;

                }

                if (Button[3].Press)
                { // настройки

                    MenuType = 1;

                }

                if (Button[4].Press)
                { // меню

                    LoadinLevel(0);

                }

            }

        }
        if (MenuType == 1)
        {

            for (var j:int; j < 9; j++ ) {

                GUI.Label(Settings[j].GUIinfo, Settings[j].Name);

                if (j == 0)
                {

                    if (GUI.Button(Settings[j].GUIinfo2, Config.Resolution.x + " x " + Config.Resolution.y))
                    {

                        ResolList = !ResolList;

                    }

                }
                if (j == 1)
                {

                    if (Config.Windowed == true)
                    {

                        if (GUI.Button(Settings[j].GUIinfo2, "X"))
                            Config.Windowed = false;

                    }
                    else
                    {

                        if (GUI.Button(Settings[j].GUIinfo2, ""))
                            Config.Windowed = true;

                    }

                }
                if (j == 2)
                    Config.AntiAliasing = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.AntiAliasing, 0, 4);
                if (j == 3)
                    Config.Filtration = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.Filtration, 0, 4);
                if (j == 4)
                    Config.Shadows = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.Shadows, 0, 3);
                if (j == 5)
                    Config.ShadowQuality = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.ShadowQuality, 0, 4);
                if (j == 6)
                    Config.Texture = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.Texture, 0, 6);
                if (j == 7)
                    Config.Mousesensitivity = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.Mousesensitivity, 0, 100);
                if (j == 8)
                {
                    Config.AudioVolume = GUI.HorizontalSlider(Settings[j].GUIinfo2, Config.AudioVolume, 0, 100);
                    AudioListener.volume = Config.AudioVolume / 100;
                }

            }

            if (GUI.Button(Rect(Settings[2].GUIinfo2.x - Settings[2].GUIinfo2.width / 2 - (Screen.width / 16.8), Settings[1].GUIinfo2.y, Screen.width / 16.8, Screen.height / 35), "Назад"))
            { // 100 30

                MenuType = 0;

                OptionsUpdate();
                Save();

            }

            if (ResolList)
            {

                for (var r:int; r < StandResol.length; r++ ) {

                    if (GUI.Button(Rect(Settings[0].GUIinfo2.x, Settings[0].GUIinfo2.y + Settings[0].GUIinfo2.height * (1 + r), Settings[0].GUIinfo2.width, Settings[0].GUIinfo2.height), StandResol[r].x + " x " + StandResol[r].y))
                    {

                        ResolList = false;
                        Config.Resolution = StandResol[r];

                    }

                }

            }

        }

        if (MenuType == 2)
        {

            if (GUI.Button(Rect(Settings[2].GUIinfo2.x - Settings[2].GUIinfo2.width / 2 - (Screen.width / 16.8), Settings[1].GUIinfo2.y, Screen.width / 16.8, Screen.height / 35), "Назад")) // 100 30
                MenuType = 0;

            if (GUI.Button(Rect(Screen.width / 2 - (Screen.width / 33.6), Settings[1].GUIinfo2.y, Screen.width / 7.3, Screen.height / 10.5), "Тестовый уровень"))
            { // 230 100
              //Application.LoadLevel(1);
                LoadinLevel(1);
            }
            if (GUI.Button(Rect(Screen.width / 2 - (Screen.width / 33.6), Settings[1].GUIinfo2.y + (Screen.width / 14), Screen.width / 7.3, Screen.height / 10.5), "Основной уровень"))
            { // 230 100
              //Application.LoadLevel(2);
                LoadinLevel(2);
            }
        }
        if (MenuType == 3)
        {

            if (GUI.Button(Rect(Settings[2].GUIinfo2.x - Settings[2].GUIinfo2.width / 2 - (Screen.width / 16.8), Settings[1].GUIinfo2.y, Screen.width / 16.8, Screen.height / 35), "Назад")) // 100 30
                MenuType = 0;

            GUI.DrawTexture(Rect(Settings[2].GUIinfo2.x, Settings[1].GUIinfo2.y, LoadTexture.width / 2, LoadTexture.height / 2), LoadTexture);

            if (GUI.Button(Rect(Settings[2].GUIinfo2.x, Settings[1].GUIinfo2.y + LoadTexture.height / 2, Screen.width / 11.2, Screen.height / 21), "Загрузить"))
            { // 150 50

                sr = new File.OpenText(SavePath);

                sr.ReadLine();

                for (var f:int = 0; f < Lines.length; f++ ) {

                    Lines[f] = sr.ReadLine();

                }

                var LoadLevelN : int = int.Parse(Lines[0]);

                sr.Close();

                var sw : StreamWriter = new StreamWriter(SavePath);

                sw.WriteLine("1");

                for (var f1:int = 0; f1 < Lines.length; f1++ ) {

                    sw.WriteLine(Lines[f1]);


                }

                sw.Flush();
                sw.Close();

                //Application.LoadLevel(LoadLevelN);
                LoadinLevel(LoadLevelN);

            }

        }

        if (MenuType == 4)
        {

            GUI.Box(Rect(Screen.width / 2 - (Screen.width / 33.6), Screen.height / 2 - 25, Screen.width / 16.8, Screen.height / 21), "Загрузка"); // 100 50
            if (LoadTimer)
            {

                LoadTimer = false;
                Application.LoadLevel(CurLoadingLevel);

            }

        }

        if (MenuType == 5)
        {

            if (GUI.Button(Rect(Settings[2].GUIinfo2.x - Settings[2].GUIinfo2.width / 2 - (Screen.width / 16.8), Settings[1].GUIinfo2.y, Screen.width / 16.8, Screen.height / 35), "Назад")) // 100 30
                MenuType = 0;

        }

    }

    function LoadinLevel(CurLevel : int )
    {

        MenuType = 4;
        BlurCamera(true);
        LoadTimer = true;
        CurLoadingLevel = CurLevel;

    }

    function BlurCamera(UnBlur : boolean )
    {

        for (var i:int; i < CurCamera.length; i++ )
        CurCamera[i].GetComponent("Blur").enabled = UnBlur;

    }

    function Save()
    {

        var sw : StreamWriter = new StreamWriter(Config.Path);

        sw.WriteLine(Config.Resolution.x);
        sw.WriteLine(Config.Resolution.y);
        sw.WriteLine(Config.AntiAliasing);

        if (Config.Windowed)
            sw.WriteLine("1");
        else
            sw.WriteLine("0");
        if (Config.Bloom)
            sw.WriteLine("1");
        else
            sw.WriteLine("0");

        sw.WriteLine(Config.Shadows);
        sw.WriteLine(Config.ShadowQuality);
        sw.WriteLine(Config.Texture);
        sw.WriteLine(Config.Filtration);
        sw.WriteLine(Config.Mousesensitivity);
        sw.WriteLine(Config.AudioVolume);

        sw.Flush();
        sw.Close();

    }

    function Load()
    {

        sr = new File.OpenText(Config.Path);

        Config.Resolution.x = float.Parse(sr.ReadLine());
        Config.Resolution.y = float.Parse(sr.ReadLine());
        Config.AntiAliasing = int.Parse(sr.ReadLine());

        if (sr.ReadLine() == "1")
            Config.Windowed = true;
        else
            Config.Windowed = false;

        if (sr.ReadLine() == "1")
            Config.Bloom = true;
        else
            Config.Bloom = false;

        Config.Shadows = int.Parse(sr.ReadLine());
        Config.ShadowQuality = int.Parse(sr.ReadLine());
        Config.Texture = int.Parse(sr.ReadLine());
        Config.Filtration = int.Parse(sr.ReadLine());
        Config.Mousesensitivity = float.Parse(sr.ReadLine());
        Config.AudioVolume = float.Parse(sr.ReadLine());


        sr.Close();

    }

    function OptionsUpdate()
    {

        QualitySettings.antiAliasing = Config.AntiAliasing;
        QualitySettings.masterTextureLimit = Mathf.Abs(Config.Texture - 6);
        QualitySettings.anisotropicFiltering = Config.Filtration;
        AudioListener.volume = Config.AudioVolume / 100;
        //QualitySettings.shadowResolution = Config.ShadowQuality;
        Screen.SetResolution(Config.Resolution.x, Config.Resolution.y, !Config.Windowed);
        ResolutionUpdate();
        ScreenResolution.z = Config.Resolution.x;

    }
}*/
