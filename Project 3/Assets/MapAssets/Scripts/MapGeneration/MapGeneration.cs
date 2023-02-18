using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration
{
    Grid<Node> map;

    List<BlockSet> blockSets;
    List<TileSet> tileSets;
    GameObject forecastTile;

    public MapGeneration(Grid<Node> map, List<BlockSet> blockSets, List<TileSet> tileSets, GameObject forecastTile)
    {
        this.map = map;
        this.blockSets = blockSets;
        this.tileSets = tileSets;
        this.forecastTile = forecastTile;
    }

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
