using UnityEngine;

public class SpawnPointBlock : MonoBehaviour
{
    [Header("Unit to Spawn")]
    public UnitObj unitObj;

    private void Start()
    {
        if (unitObj == null)
            Debug.Log("Unit Object not Set in one of SpawnPointBlocks");
    }
}
