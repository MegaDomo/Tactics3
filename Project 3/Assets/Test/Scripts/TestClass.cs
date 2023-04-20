using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClass : MonoBehaviour
{
    public BattleSystem battleSystem;
    public GameObject testObject;

    public Action testEvent;
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        battleSystem.playerTurnEvent += TestMethod;
    }

    private void Start()
    {
        
    }

    private void TestMethod(Unit unit)
    {
        if (testObject == null) Debug.Log("Test Object is Null");
        else Debug.Log("Test Object is Good");
    }
}
