using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration
{
    Grid<Node> map;

    List<BlockSet> blockSets;
    List<TileSet> tileSets;
    GameObject forecastTile;
    List<int> saturation;
    

    BlockSet blockSet;
    List<BlockObject> blockObjects;

    public MapGeneration(string whichTerrain, Grid<Node> map, List<BlockSet> blockSets, 
                         List<TileSet> tileSets, GameObject forecastTile, List<int> saturation)
    {
        this.map = map;
        this.blockSets = blockSets;
        this.tileSets = tileSets;
        this.forecastTile = forecastTile;
        this.saturation = saturation;
        DetermineTerrain(whichTerrain);
        CreateMap();
    }

    private void DetermineTerrain(string whichTerrain)
    {
        foreach (BlockSet set in blockSets)
        {
            if (whichTerrain == set.terrain)
            {
                blockSet = set;
                blockObjects = set.blockObjects;
            }
                
        }
    }

    #region Create Map
    private void CreateMap()
    {
        NodePass();
        NeutralPass();
        DifficultTerrainPass();
        ClusterPass();
        LinePass();
        ForecastTilePass();

        



        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {

            }
        }
    }

    private void NodePass()
    {
        for (int i = 0; i < map.GetSize(); i++)
            for (int j = 0; j < map.GetSize(); j++)
                map.SetGridObject(map.GetWorldPosition(i, j), new Node(i, j, map));
    }

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

        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                Transform block = Object.Instantiate(neutralBlock.prefab, map.GetWorldPosition(i, j), Quaternion.identity);
                map.GetGridObject(i, j).SetBlockObject(block, neutralBlock);
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
            Debug.Log("No Neutral Block Detected");
            return;
        }

        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                if (Random.Range(0, 100) > saturation[GetTerrainSaturation()]) continue;
                CreateDifficultTerrain(i, j, difficultBlock);
            }
        }
    }

    private void CreateDifficultTerrain(int x, int z, BlockObject difficultBlock)
    {
        Transform block = Object.Instantiate(difficultBlock.prefab, map.GetWorldPosition(x, z), Quaternion.identity);
        Node node = map.GetGridObject(x, z);
        Object.Destroy(node.blockVFX.gameObject);
        node.SetBlockObject(block, difficultBlock);
    }

    private int GetTerrainSaturation()
    {
        int level = 1;
        switch (blockSet.difficultTerrainSaturation)
        {
            case BlockSet.TerrainSaturation.Low:
                return 0;
            case BlockSet.TerrainSaturation.Medium:
                return 1;
            case BlockSet.TerrainSaturation.High:
                return 2;
        }
        return level;
    }

    private void ClusterPass()
    {
        // TODO : Figure out how to make Clusters - likely use List<Node> as target
    }

    private void LinePass()
    {
        // TODO : Figure out how to make Line - likely use List<Node> as target
    }

    private void ForecastTilePass()
    {
        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                CreateForecastTile(i, j);
            }
        }
    }

    private void CreateForecastTile(int x, int z)
    {
        Node node = map.GetGridObject(x, z);
        ForecastTile tile = Object.Instantiate(forecastTile, node.GetStandingPoint(), Quaternion.identity).GetComponent<ForecastTile>();
        node.SetForecastTile(tile);
        map.SetGridObject(x, z, node);
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

    /*
        #region Create Map
        // Methods that Picks a random size for the map
        private void CreateMap()
        {
            for (int i = 0; i < map.GetSize(); i++)
            {
                for (int j = 0; j < map.GetSize(); j++)
                {
                    BlockObject blockObject = GetRandomBlockObject();
                    Transform block = Object.Instantiate(blockObject.prefab, map.GetWorldPosition(i, j), Quaternion.identity);

                    StoreDataInGrid(i, j, block, blockObject);
                }
            }
        }

        private void StoreDataInGrid(int x, int z, Transform block, BlockObject blockObject)
        {
            Node node = new Node(x, z, map);
            node.SetBlockObject(block, blockObject);
            if (Obstacle())
            {
                Object.Instantiate(tileObject.prefab, map.GetWorldPosition(x, z) + new Vector3(0, cellSize, 0), Quaternion.identity);
                node.SetTileObject(tileObject);
            }

            ForecastTile tile = Object.Instantiate(forecastTile, node.GetStandingPoint(), Quaternion.identity).GetComponent<ForecastTile>();
            node.SetForecastTile(tile);
            map.SetGridObject(x, z, node);
        }
        private bool Obstacle()
        {
            // short hand, computer picks a number between the range of 0 and 5, 0-4, or 20% to determine true or false.
            return (Random.Range(0, 5) == 0);
        }

        private BlockObject GetRandomBlockObject()
        {
            BlockObject temp;
            int z = Random.Range(0, 5);

            // Obstacle 20%
            if (z == 0)
                temp = blockObjects[2];
            // Swamp 40%
            else if (z == 1 || z == 2)
                temp = blockObjects[0];
            // Plains 40%
            else
                temp = blockObjects[1];

            return temp;
        }
        #endregion
    */
}
