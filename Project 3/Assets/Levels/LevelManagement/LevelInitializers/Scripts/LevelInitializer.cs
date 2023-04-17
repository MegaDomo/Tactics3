using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject UIprefab;
    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(UIprefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
