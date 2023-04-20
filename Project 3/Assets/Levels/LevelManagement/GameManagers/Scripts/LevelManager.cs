using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelManager", menuName = "Managers/Level Manager")]
public class LevelManager : ScriptableObject
{
    public List<Level> levels;

    public void LoadLevel()
    {
        Instantiate(levels[0].map);
    }
}
