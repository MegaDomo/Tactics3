using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHighlighter : MonoBehaviour
{
    private List<ForecastTile> fTiles;

    private void Start()
    {
        fTiles = new List<ForecastTile>();
    }
    public void Highlight(Unit unit)
    {
        Node node = unit.node;

        List<Node> routes = MapManager.instance.pathing.GetAllRoutes(unit);

        foreach (Node item in routes)
            item.forecastTile.TileInReach();
    }

    public void Unhighlight()
    {
        foreach (ForecastTile tile in fTiles)
            tile.HideTile();
        fTiles = new List<ForecastTile>();
    }
}
