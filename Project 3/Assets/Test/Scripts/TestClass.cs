using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClass : MonoBehaviour
{
    public GameObject testObject;

    private void Awake()
    {
        if (testObject == null) Debug.Log("Awake");
    }

    private void OnEnable()
    {
        if (testObject == null) Debug.Log("OnEnable");
    }

    private void Start()
    {
        if (testObject == null) Debug.Log("Start");
    }
}
