using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClass : MonoBehaviour
{
    public enum TestType { Test1 = 5, Test2 }

    public TestType testType;

    public BattleSystem battleSystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
            battleSystem.Victory();
    }

    private void Start()
    {
        //Debug.Log(testType.ToString());    
    }
}