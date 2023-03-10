using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDistribution : MonoBehaviour
{
    [Header("Unity References")]
    public SpawnManager spawner;

    [Header("Units in Next Combat")]
    public List<Unit> units;

    public void DistributeUnits()
    {
        spawner.SetUnitsToSpawn(units);
    }
}
