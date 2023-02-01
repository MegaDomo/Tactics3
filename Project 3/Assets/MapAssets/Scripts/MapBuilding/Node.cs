using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool passable = true;
    public int x;
    public int z;
    public int movementCost;
    public Transform vfx;
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
        passable = tileObject.passable;
        movementCost = tileObject.movementCost;
    }

    public Vector3 GetStandingPoint()
    {
        return vfx.gameObject.GetComponent<Block>().standingPoint.position;
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

    public void SetTileObject(Transform block, TileObject tileObject)
    {
        this.vfx = block;
        this.tileObject = tileObject;
        UpdatePathingValues();
    }

    public void ClearTileObject()
    {
        tileObject = null;
    }

    public bool CanBuild()
    {
        return tileObject == null;
    }

    public void SetForecastTile(ForecastTile tile)
    {
        forecastTile = tile;
        tile.SetNode(this);
    }
}
