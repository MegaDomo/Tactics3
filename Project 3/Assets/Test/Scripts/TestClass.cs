using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public event EventHandler OnPressSpaceBar;

    private void Start()
    {
        OnPressSpaceBar += TestMethod;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPressSpaceBar?.Invoke(this, EventArgs.Empty);
        }
    }

    private void TestMethod(object sender, EventArgs e)
    {
        Debug.Log("Hell0");
    }
}
