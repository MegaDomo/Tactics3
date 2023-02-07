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
    public Transform blockVFX;
    public BlockObject blockObject;
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

    public void OnUnitEnter()
    {
        passable = false;
    }

    public void OnUnitExit()
    {
        passable = true;
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
        return blockVFX.GetComponent<Block>().standingPoint.position;
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

    public void SetBlockObject(Transform block, BlockObject blockObject)
    {
        this.blockVFX = block;
        this.blockObject = blockObject;
        UpdatePathingValues();
    }

    public void ClearBlockObject()
    {
        blockObject = null;
    }

    public bool CanBuild()
    {
        return blockObject == null;
    }

    public void SetTileObject(TileObject tileObject)
    {
        this.tileObject = tileObject;
        UpdatePathingValues();
    }

    public void ClearTileObject()
    {
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
        this.blockVFX = block;
        this.blockObject = blockObject;
        this.tileObject = tileObject;
        this.forecastTile = forecastTile;
        forecastTile.SetNode(this);
        UpdatePathingValues();
    }
}
