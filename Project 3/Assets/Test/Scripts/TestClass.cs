using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public delegate int test(bool value);

    private void Start()
    {
        return;

        bool valu2 = true;
        test newEvent = new test(TestMethod2);
        newEvent += TestMethod;

        for (int i = 0; i < 30; i++)
        {
            int x = newEvent.Invoke(true);
            Debug.Log(x);
        }
    }

    private int TestMethod(bool value)
    {
        Debug.Log("From 1");
        return 3;
    }

    private int TestMethod2(bool value)
    {
        Debug.Log("From 2");
        return 7;
    }
}
