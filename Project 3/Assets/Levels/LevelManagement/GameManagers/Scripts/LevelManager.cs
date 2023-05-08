using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelManager", menuName = "Managers/Level Manager")]
public class LevelManager : ScriptableObject
{
    [SerializeField] private List<Level> levels;

    private Dictionary<string, int> levelIndexes = new Dictionary<string, int>();

    private void OnEnable()
    {
        SetLevels();
    }

    private void OnDisable()
    {
        levelIndexes = new Dictionary<string, int>();
    }

    public void LoadLevel(string levelName)
    {
        if (!levelIndexes.ContainsKey(levelName))
        {
            Debug.Log("Name does not match Level Manager data");
            return;
        }
        int index = levelIndexes[levelName];
        Instantiate(levels[index].map);
    }

    private void SetLevels()
    {
        int index = 0;

        foreach (Level level in levels)
            levelIndexes.Add(level.name, index++);
    }

    #region Getters & Setters
    public Level GetLevel(string levelName)
    {
        if (!levelIndexes.ContainsKey(levelName))
        {
            Debug.Log("Name does not match Level Manager data");
            return null;
        }
        int index = levelIndexes[levelName];

        return levels[index];
    }

    public void AddLevel(Level level)
    {
        levels.Add(level);
    }
    #endregion
}
