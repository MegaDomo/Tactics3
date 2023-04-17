using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Action selectLevelEvent;

    public void PlayLevel1Button()
    {
        selectLevelEvent.Invoke();
        SceneManager.LoadScene(1);
    }

    public void PlayLevel2Button()
    {
        selectLevelEvent.Invoke();
        SceneManager.LoadScene(2);
    }
}
