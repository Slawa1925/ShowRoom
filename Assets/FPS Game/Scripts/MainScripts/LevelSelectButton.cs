using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    static int maxIndex = 0;
    int curIndex;

    [SerializeField]
    Text levelName;

    void Start()
    {
        maxIndex++;
        curIndex = maxIndex;
        GetComponent<Button>().onClick.AddListener(delegate { MainMenu.Instance.LoadLevel(curIndex); });
        levelName.text = "[" + (curIndex) + "] " + NameFromIndex(curIndex);
    }

    private string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
}
