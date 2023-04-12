using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    
    private void Start()
    {
        Unit unit = new Unit();
        Player player = new Player();
        IUnit interfaceUnit = new Player();

        unit.name = "Test From Unit";
        player.TestString = "Test from Player";
        interfaceUnit.TestString = "Test From interfaceUnit";

        IUnit handOff = player;
        Debug.Log(handOff.TestString);
    }

    private void TestMethod(bool value)
    {

    }

    private void TestMethod2(bool value)
    {

    }
}
