using System;
using System.Collections.Generic;
using UnityEngine;

// This Script Highlights Nodes with a given Unit's Position on the map
public class NodeHighlighter : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public BattleSystem battleSystem;
    public PlayerTurn playerTurn;

    private List<ForecastTile> fTiles = new List<ForecastTile>();

    private void OnEnable()
    {
        battleSystem.playerTurnEvent += Highlight;
        playerTurn.deselectedNodeEvent += Highlight;

        playerTurn.endTurnEvent += Unhighlight;
        playerTurn.selectedNodeEvent += Unhighlight;
    }

    public void Highlight(Unit unit)
    {
        List<Node> routes = Pathfinding.GetAllRoutes(gameMaster.GetMap(), unit);
        
        foreach (Node node in routes)
        {
            ForecastTile tile = node.forecastTile;
            fTiles.Add(tile);
            tile.TileInReach();
        }
    }

    public void Unhighlight()
    {
        foreach (ForecastTile tile in fTiles)
            tile.HideTile();
        fTiles = new List<ForecastTile>();
    }
}
