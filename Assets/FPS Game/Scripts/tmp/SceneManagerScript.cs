using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public Text ValueTxt;

    private void Start()
    {
        ValueTxt.text = SingletonTest.Instance.Value.ToString();
    }

    public void GoToFirstScene()
    {
        SceneManager.LoadScene("MainLevel");
        SingletonTest.Instance.Value++;
    }

    public void GoToSecondScene()
    {
        SceneManager.LoadScene("TestLevel");
        SingletonTest.Instance.Value++;
    }
}
