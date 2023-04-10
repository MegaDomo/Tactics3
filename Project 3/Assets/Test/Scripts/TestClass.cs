using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public delegate int test(bool value);

    private void Start()
    {
        bool valu2 = true;
        test newEvent = new test(TestMethod);
        newEvent += TestMethod2;
        int x = newEvent.Invoke(true);
        Debug.Log(x);
    }

    private int TestMethod(bool value)
    {
        return 3;
    }

    private int TestMethod2(bool value)
    {
        return 7;
    }
}
