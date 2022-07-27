using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariablesManager : MonoBehaviour
{
    public int testInt;
    public float testFloat;
    public Color testColor;
    public Vector2 testVector2;
    public GameObject testObject;

    public int _testInt;
    public float _testFloat;
    public Color _testColor;
    public Vector2 _testVector2;
    public GameObject _testObject;


    void Start()
    {
    }
    void AssignVariables()
    {
        GlobalVariables.testInt = testInt;
        GlobalVariables.testFloat = testFloat;
        GlobalVariables.testColor = testColor;
        GlobalVariables.testVector2 = testVector2;
        GlobalVariables.testObject = testObject;
        Debug.Log("assigned");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            AssignVariables();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            DisplayVariables();
            Debug.Log("displayed");
        }
    }

    void DisplayVariables()
    {
        _testInt = GlobalVariables.testInt;
        _testFloat = GlobalVariables.testFloat;
        _testColor = GlobalVariables.testColor;
        _testVector2 = GlobalVariables.testVector2;
        _testObject = GlobalVariables.testObject;
    }

    /*void OnValidate()
    {
        Debug.Log("validate");
        if (UpdateVariables)
        {
            
            AssignVariables();
        }
    }*/
}
