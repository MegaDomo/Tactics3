using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClass : MonoBehaviour
{
    public BattleSystem battleSystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
            battleSystem.Victory();
    }
}