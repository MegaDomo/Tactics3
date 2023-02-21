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
        List<BlockObject> neutralBlocks = FindBlock(BlockObject.BlockType.Neutral);

        if (neutralBlocks == null)
        {
            Debug.Log("No Neutral Block Detected");
            return;
        }

        for (int x = 0; x < map.GetSize(); x++)
        {
            for (int z = 0; z < map.GetSize(); z++)
            {
                CreateNeutralBlock(x, z, GetRandomBlockObjectFromList(neutralBlocks));   
            }
        }
    }

    private void CreateNeutralBlock(int x, int z, BlockObject neutralBlock)
    {
        Node node = map.GetGridObject(x, z);
        Transform block = Object.Instantiate(neutralBlock.prefab, map.GetWorldPosition(x, z), Quaternion.identity);
        node.SetBlockObject(block, neutralBlock);
    }

    private void DifficultTerrainPass()
    {
        List<BlockObject> difficultBlocks = FindBlock(BlockObject.BlockType.Difficult);

        if (difficultBlocks == null)
        {
            Debug.Log("No Difficult Terrain Block Detected");
            return;
        }

        for (int x = 0; x < map.GetSize(); x++)
            for (int z = 0; z < map.GetSize(); z++)
                CreateDifficultTerrain(x, z, GetRandomBlockObjectFromList(difficultBlocks));
    }

    private void CreateDifficultTerrain(int x, int z, BlockObject difficultBlock)
    {
        if (Random.Range(0, 100) > saturation[GetTerrainSaturation(blockSet.difficultTerrainSaturation)])
            return;

        Node node = map.GetGridObject(x, z);
        Transform block = Object.Instantiate(difficultBlock.prefab, map.GetWorldPosition(x, z), Quaternion.identity);
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

    private List<BlockObject> FindBlock(BlockObject.BlockType type)
    {
        List<BlockObject> blocksToFind = new List<BlockObject>();

        foreach (BlockObject block in blockObjects)
            if (block.blockType == type)
                blocksToFind.Add(block);

        if (blocksToFind.Count == 0) return null;
        return blocksToFind;
    }

    private BlockObject GetRandomBlockObjectFromList(List<BlockObject> blocks)
    {
        int index = Random.Range(0, blocks.Count);
        return blocks[index];
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
            for (int z = 0; z < map.GetSize(); z++)
                CreateObstacle(x, z, obstacleTile);
    }

    private void CreateObstacle(int x, int z, TileObject obstacleTile)
    {
        if (Random.Range(0, 100) > saturation[GetTerrainSaturation(blockSet.obstacleSaturation)])
            return; 

        Node node = map.GetGridObject(x, z);

        if (!IsObstacleSafe(x, z, node, obstacleTile))
            return;

        Transform obstacle = Object.Instantiate(obstacleTile.prefab, GetTileObjectSpawnPosition(x, z), GetRandomZAngle());
        node.SetTileObject(obstacle, obstacleTile);
    }

    private bool IsObstacleSafe(int x, int z, Node node, TileObject tileObject)
    {
        // TEMP :
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
            for (int z = 0; z < map.GetSize(); z++)
                CreateDecor(x, z, decorTile);
    }

    private void CreateDecor(int x, int z, TileObject decorTile)
    {
        if (Random.Range(0, 100) > saturation[GetTerrainSaturation(blockSet.decorSaturation)])
            return;

        Node node = map.GetGridObject(x, z);

        if (!IsDecorSafe(x, z, node, decorTile))
            return;

        Transform decor = Object.Instantiate(decorTile.prefab, GetTileObjectSpawnPosition(x, z), Quaternion.identity);
        node.SetTileObject(decor, decorTile);
    }

    private bool IsDecorSafe(int x, int z, Node node, TileObject tileObject)
    {
        // TEMP :
        if (node.movementCost > 1)
            return false;

        List<Vector2Int> positionList = tileObject.GetGridPositionList(x, z);
        foreach (Vector2Int vec in positionList)
            if (!map.isCoordinatesSafe(vec.x, vec.y))
                return false;
        return true;
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
    private int GetTerrainSaturation(TerrainKit.TerrainSaturation saturation)
    {
        int level = 1;
        switch (saturation)
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

    private Quaternion GetRandomZAngle()
    {
        Quaternion rotation = Quaternion.identity;
        int angle = 0;
        switch (Random.Range(0, 4))
        {
            case 0:
                angle = 0;
                break;
            case 1:
                angle = 90;
                break;
            case 2:
                angle = 180;
                break;
            case 3:
                angle = 270;
                break;
        }
        rotation.eulerAngles = new Vector3(0, angle, 0);

        return rotation;
    }

    private Vector3 GetTileObjectSpawnPosition(int x, int z)
    {
        return map.GetGridObject(x, z).GetStandingPoint() + new Vector3(0, map.GetCellSize(), 0);
    }
    #endregion
    #endregion
}
