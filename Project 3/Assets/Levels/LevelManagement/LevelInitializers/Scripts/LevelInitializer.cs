using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager.LoadLevel();

        // TODO : Maybe this is where Dialogue can Trigger from before GameMaster Starts Combat
        // Would need GameMaster to Load Characters but not start combat

        gameMaster.LoadLevel();






        //StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(3.5f);
        gameMaster.LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
