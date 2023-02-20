using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainKit
{
    public enum TerrainSaturation { Low, Medium, High }

    public string terrain;
    public TerrainSaturation difficultTerrainSaturation;
    public List<BlockObject> blockObjects;
    public TerrainSaturation obstacleSaturation;
    public List<TileObject> tileObjects;


}
