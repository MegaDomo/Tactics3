using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration
{
    private Grid<Node> map;

    private List<TerrainKit> terrains;
    private GameObject forecastTile;
    private List<int> saturation;

    private TerrainKit blockSet;
    private List<BlockObject> blockObjects;
    private List<TileObject> tileObjects;

    public MapGeneration(string whichTerrain, Grid<Node> map, List<TerrainKit> terrains, GameObject forecastTile, List<int> saturation)
    {
        this.map = map;
        this.terrains = terrains;
        this.forecastTile = forecastTile;
        this.saturation = saturation;
        DetermineTerrain(whichTerrain);
        CreateMap();
    }

    private void DetermineTerrain(string whichTerrain)
    {
        foreach (TerrainKit set in terrains)
        {
            if (whichTerrain == set.terrain)
            {
                blockSet = set;
                blockObjects = set.blockObjects;
                tileObjects = set.tileObjects;
            }
        }
    }

    #region Create Map
    private void CreateMap()
    {
        // Initialize Pass
        NodePass();

        // Block Object Pass
        NeutralPass();
        DifficultTerrainPass();
        ClusterPass();
        LinePass();

        // Tile Object Pass
        ObstaclePass();
        DecorPass();

        // Forecast Pass
        ForecastTilePass();
    }

    private void NodePass()
    {
        for (int i = 0; i < map.GetSize(); i++)
            for (int j = 0; j < map.GetSize(); j++)
                map.SetGridObject(map.GetWorldPosition(i, j), new Node(i, j, map));
    }

    #region BlockObject Passes
    private void NeutralPass()
    {
        BlockObject neutralBlock;
        // TEMP
        // Note : Assuming we have 1 Neutral Block
        neutralBlock = FindBlock(BlockObject.BlockType.Neutral);

        if (neutralBlock == null)
        {
            Debug.Log("No Neutral Block Detected");
            return;
        }

        for (int x = 0; x < map.GetSize(); x++)
        {
            for (int z = 0; z < map.GetSize(); z++)
            {
                Transform block = Object.Instantiate(neutralBlock.prefab, map.GetWorldPosition(x, z), Quaternion.identity);
                map.GetGridObject(x, z).SetBlockObject(block, neutralBlock);
            }
        }
    }

    private void DifficultTerrainPass()
    {
        BlockObject difficultBlock;
        // TEMP
        // Note : Assuming we have 1 Neutral Block
        difficultBlock = FindBlock(BlockObject.BlockType.Difficult);

        if (difficultBlock == null)
        {
            Debug.Log("No Difficult Terrain Block Detected");
            return;
        }

        for (int x = 0; x < map.GetSize(); x++)
        {
            for (int z = 0; z < map.GetSize(); z++)
            {
                if (Random.Range(0, 100) > saturation[GetTerrainSaturation()]) continue;
                CreateDifficultTerrain(z, z, difficultBlock);
            }
        }
    }

    private void CreateDifficultTerrain(int x, int z, BlockObject difficultBlock)
    {
        Transform block = Object.Instantiate(difficultBlock.prefab, map.GetWorldPosition(x, z), Quaternion.identity);
        Node node = map.GetGridObject(x, z);
        node.ClearBlockObject();
        node.SetBlockObject(block, difficultBlock);
    }

    

    private void ClusterPass()
    {
        // TODO : Figure out how to make Clusters - likely use List<Node> as target
    }

    private void LinePass()
    {
        // TODO : Figure out how to make Line - likely use List<Node> as target
    }

    private BlockObject FindBlock(BlockObject.BlockType type)
    {
        foreach (BlockObject block in blockObjects)
        {
            if (block.blockType == type)
            {
                return block;
            }
        }
        return null;
    }
    #endregion

    #region TileObject Passes
    private void ObstaclePass()
    {
        TileObject obstacleTile = FindTile(TileObject.TileType.Obstacle);

        if (obstacleTile == null)
        {
            Debug.Log("No Obstacle Tile Found");
            return;
        }

        for (int x = 0; x < map.GetSize(); x++)
        {
            for (int z = 0; z < map.GetSize(); z++)
            {
                if (Random.Range(0, 100) > saturation[GetTerrainSaturation()]) continue;
                CreateObstacle(x, z, obstacleTile);
            }
        }
    }

    private void CreateObstacle(int x, int z, TileObject obstacleTile)
    {
        Node node = map.GetGridObject(x, z);

        if (!IsTileObjectSafe(x, z, node, obstacleTile))
            return;

        Transform obstacle = Object.Instantiate(obstacleTile.prefab, map.GetGridObject(x, z).GetStandingPoint(), Quaternion.identity);
        node.SetTileObject(obstacle, obstacleTile);
    }

    private bool IsTileObjectSafe(int x, int z, Node node, TileObject tileObject)
    {
        if (node.movementCost > 1)
            return false;

        List<Vector2Int> positionList = tileObject.GetGridPositionList(x, z);
        foreach (Vector2Int vec in positionList)
            if (!map.isCoordinatesSafe(vec.x, vec.y))
                return false;
        return true;
    }

    private void DecorPass()
    {

        TileObject decorTile = FindTile(TileObject.TileType.Decor);

        if (decorTile == null)
        {
            Debug.Log("No Decor Tile Found");
            return;
        }

        for (int x = 0; x < map.GetSize(); x++)
        {
            for (int z = 0; z < map.GetSize(); z++)
            {
                Transform decor = Object.Instantiate(decorTile.prefab, map.GetGridObject(x, z).GetStandingPoint(), Quaternion.identity);
                map.GetGridObject(x, z).SetTileObject(decor, decorTile);
            }
        }
    }
    private TileObject FindTile(TileObject.TileType type)
    {
        foreach (TileObject tile in tileObjects)
        {
            if (tile.tileType == type)
            {
                return tile;
            }
        }
        return null;
    }
    #endregion

    #region Forecast Passes
    private void ForecastTilePass()
    {
        for (int i = 0; i < map.GetSize(); i++)
            for (int j = 0; j < map.GetSize(); j++)
                CreateForecastTile(i, j);
    }

    private void CreateForecastTile(int x, int z)
    {
        Node node = map.GetGridObject(x, z);
        ForecastTile tile = Object.Instantiate(forecastTile, node.GetStandingPoint(), Quaternion.identity).GetComponent<ForecastTile>();
        node.SetForecastTile(tile);
        map.SetGridObject(x, z, node);
    }
    #endregion

    #region Utility
    private int GetTerrainSaturation()
    {
        int level = 1;
        switch (blockSet.difficultTerrainSaturation)
        {
            case TerrainKit.TerrainSaturation.Low:
                return 0;
            case TerrainKit.TerrainSaturation.Medium:
                return 1;
            case TerrainKit.TerrainSaturation.High:
                return 2;
        }
        return level;
    }
    #endregion
    #endregion
}
