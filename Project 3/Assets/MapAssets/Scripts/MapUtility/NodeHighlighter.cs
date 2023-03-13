using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHighlighter : MonoBehaviour
{
    private List<ForecastTile> fTiles = new List<ForecastTile>();

    public void Highlight(Unit unit)
    {
        List<Node> routes = Pathfinding.GetAllRoutes(MapManager.instance.GetMap(), unit);

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
