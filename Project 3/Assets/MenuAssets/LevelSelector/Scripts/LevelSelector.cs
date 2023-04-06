using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [Header("Unity References")]
    public LevelSelectPlayerUI playerUI;

    public void PlayLevelButton()
    {
        playerUI.SetPlayerObject();
        SceneManager.LoadScene(1);
    }
}
