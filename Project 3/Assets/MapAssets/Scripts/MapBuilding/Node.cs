using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool passable = true;
    public int x;
    public int y;
    public int z;
    public int movementCost;
    public Transform blockPrefab;
    public BlockObject blockObject;
    public Transform tilePrefab;
    public TileObject tileObject;
    public ForecastTile forecastTile;
    public Unit unit;
    public Grid<Node> grid;

    // Constructor
    public Node()
    {
        // Debug Constructor
    }

    public Node(int x, int z, Grid<Node> grid)
    {
        this.x = x;
        this.y = 0;
        this.z = z;
        this.grid = grid;
    }
    public Node(int x, int y, int z, Grid<Node> grid)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.grid = grid;
    }

    public void OnUnitEnter(Unit unit)
    {
        this.unit = unit;
        passable = false;
    }

    public void OnUnitExit()
    {
        unit = null;
        passable = true;
    }
    
    public bool IsUnitWithinNode(Unit unit)
    {
        float ux = unit.transform.position.x;
        float uz = unit.transform.position.z;

        float epsilon = 2f;

        if (ux < x - epsilon && ux > x + epsilon)
            return false;
        if (uz < z - epsilon && uz > z + epsilon)
            return false;
        return true;
    }

    public void UpdatePathingValues()
    {
        if (blockObject == null)
        {
            passable = false;
            return;
        }
        
        // Passable
        if (tileObject != null)
        {
            if (!blockObject.passable || !tileObject.passable)
                passable = false;
            else
                passable = true;
        }
        else
            passable = blockObject.passable;

        // Movement Cost
        if (tileObject != null)
            movementCost = blockObject.movementCost + tileObject.movementCost;
        else
            movementCost = blockObject.movementCost;
    }

    public Vector3 GetStandingPoint()
    {
        return blockPrefab.GetComponent<Block>().standingPoint.position;
    }

    public void SetCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public void SetMoveCost(int movementCost)
    {
        this.movementCost = movementCost;
    }

    public void SetBlockObject(Transform blockPrefab, BlockObject blockObject)
    {
        this.blockPrefab = blockPrefab;
        this.blockObject = blockObject;
        UpdatePathingValues();
    }

    public void ClearBlockObject()
    {
        Object.Destroy(blockPrefab.gameObject);
        blockObject = null;
    }

    public bool CanBuild()
    {
        return blockObject == null;
    }

    public void SetTileObject(Transform tilePrefab, TileObject tileObject)
    {
        this.tilePrefab = tilePrefab;
        this.tileObject = tileObject;
        UpdatePathingValues();
    }

    public void ClearTileObject()
    {
        Object.Destroy(tilePrefab.gameObject);
        tileObject = null;
        UpdatePathingValues();
    }

    public void SetForecastTile(ForecastTile tile)
    {
        forecastTile = tile;
        tile.SetNode(this);
    }

    public void ClearForecastTile()
    {
        forecastTile.SetNode(null);
        forecastTile = null;
    }

    public void SetNodeData(Transform block, BlockObject blockObject, TileObject tileObject, ForecastTile forecastTile)
    {
        this.blockPrefab = block;
        this.blockObject = blockObject;
        this.tileObject = tileObject;
        this.forecastTile = forecastTile;
        forecastTile.SetNode(this);
        UpdatePathingValues();
    }
}
