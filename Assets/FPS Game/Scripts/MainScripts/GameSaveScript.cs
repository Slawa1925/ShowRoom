using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class GameSaveScript : MonoBehaviour
{
    public ScreenshotTaker screenshotScript;
    public GameObject player;
    public GameObject playerCameraMain;
    public int saveNumber;
    public string saveName;

    public GameObject saveBlock;
    public string[] saveFolders;
    public GameObject[] saves;

    public string savePath = "/Resources/Saves/";

    public string imagePath;
    byte[] fileData;
    Texture2D tex;
    public GameObject load;
    public ScrollRect loadRect;

    public string date;
    public string level;
    public string playerPosition;
    public string playerRotation;
    public string cameraRotation;
    public string player_motor_velocity;

    public string player_health;
    public string player_stamina;
    public string player_Run;
    public string player_Crouch;
    public string player_isCrouched;
    public string player_CNMove;
    public string player_CanClimb;
    public string player_isClimbming;
    public string player_climbing;
    public string player_Y;
    public string player_CurLadder;
    public string plyer_inventoryOpened;
    public string player_Free;
    public string player_runProgress;


    public void WriteFile()
    {
        if (saveName == "")
        {
            saveName = "Save_" + saveNumber;
        }

        if (Directory.Exists(Application.dataPath + savePath + saveName))
        {
            Debug.Log("Save already exists!");
        }
        else
        {
            DirectoryInfo di = Directory.CreateDirectory(Application.dataPath + savePath + saveName);

            screenshotScript.TakeScreenshot(Application.dataPath + savePath + saveName);

            StreamWriter sw = new StreamWriter(Application.dataPath + savePath + saveName + "/saveData.txt");

            sw.WriteLine(date);
            sw.WriteLine(level);
            sw.WriteLine(playerPosition);
            sw.WriteLine(playerRotation);
            sw.WriteLine(cameraRotation);
            sw.WriteLine(player_motor_velocity);

            sw.Flush();
            sw.Close();
        }
    }

    public void ReadFile(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            date = sr.ReadLine();
            level = sr.ReadLine();
            playerPosition = sr.ReadLine();
            playerRotation = sr.ReadLine();
            cameraRotation = sr.ReadLine();
            player_motor_velocity = sr.ReadLine();
        }
    }

    public void SaveGame()
    {
        saveNumber++;

        date = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        level = SceneManager.GetActiveScene().name;

        playerPosition = "" + player.transform.position;
        playerRotation = "" + player.transform.rotation;
        cameraRotation = "" + playerCameraMain.transform.localRotation;
        player_motor_velocity = "" + player.GetComponent<CharacterMotor>().movement.velocity;

        WriteFile();

        saveName = "";
    }

    public void LoadGame(int index)
    {
        ReadFile(saveFolders[index] + "/saveData.txt");
    }

    public void DeleteSave(int index)
    {
        Directory.Delete(saveFolders[index], true);
        Destroy(saves[index]);
        Debug.Log("Deleteted");
    }

    public void LoadSaves()
    {
        for (int i = 0; i < saves.Length; i++)
        {
            Destroy(saves[i]);
        }

        saveFolders = Directory.GetDirectories(Application.dataPath + savePath);
        saves = new GameObject[saveFolders.Length];

        for (int i = 0; i < saveFolders.Length; i++)
        {
            saves[i] = Instantiate(saveBlock, load.transform, false);

            imagePath = saveFolders[i] + "/Preview.png";

            if (File.Exists(imagePath))
            {
                fileData = File.ReadAllBytes(imagePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);

                string tempStr = Application.dataPath + savePath;
                saveName = saveFolders[i];
                saveName = saveName.Remove(0, tempStr.Length);

                using (StreamReader sr = new StreamReader(saveFolders[i] + "/saveData.txt"))
                {
                    date = sr.ReadLine();
                    level = sr.ReadLine();
                }

                saves[i].GetComponent<SaveBlock>().SetSaveBlock(tex, saveName, date, level, i, this);
            }
            else
            {
                Debug.Log("No image file! At: " + imagePath);
            }
        }
    }

    public void SetSaveName(UnityEngine.UI.InputField _name)
    {
        saveName = _name.text;
    }
}