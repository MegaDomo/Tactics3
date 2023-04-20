using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;

    [Header("Prefab References")]
    public GameObject UIprefab;
    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        //gameMaster.LoadLevel();
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);
        gameMaster.LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
