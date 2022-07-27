using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveBlock : MonoBehaviour
{
    public RawImage previewImage;
    public Text saveName;
    public Text saveDate;
    public Text saveLevel;
    public int index;

    public Button loadButton;
    public Button deleteButton;

    public void SetSaveBlock(Texture2D _previewImage, string _saveName, string _saveDate, string _saveLevel, int _index, GameSaveScript saveScript)
    {
        previewImage.texture = _previewImage;
        saveName.text = _saveName;
        saveDate.text = _saveDate;
        saveLevel.text = _saveLevel;
        index = _index;
        loadButton.onClick.AddListener(() => saveScript.LoadGame(index));
        deleteButton.onClick.AddListener(() => saveScript.DeleteSave(index));
    }
}
