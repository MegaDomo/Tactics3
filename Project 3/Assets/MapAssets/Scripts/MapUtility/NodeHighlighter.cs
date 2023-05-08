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

    [Header("Unity References")]
    public NodeClicker clicker;

    private void OnEnable()
    {
        battleSystem.playerTurnEvent += HighlightPossibleRoutes;
        playerTurn.deselectedNodeEvent += HighlightPossibleRoutes;
        playerTurn.selectedNodeEvent += HighlightPossibleRoutes;

        playerTurn.choseAbilityEvent += HighlightAreaAbilityForecast;
        playerTurn.targetChosenEvent += HighlightAreaAbilityForecast;

        playerTurn.choseAbilityEvent += HidePossibleRoutes;
        playerTurn.endTurnEvent += HidePossibleRoutes;
    }

    private void OnDisable()
    {
        battleSystem.playerTurnEvent -= HighlightPossibleRoutes;
        playerTurn.deselectedNodeEvent -= HighlightPossibleRoutes;
        playerTurn.selectedNodeEvent -= HighlightPossibleRoutes;

        playerTurn.choseAbilityEvent -= HighlightAreaAbilityForecast;
        playerTurn.choseAbilityEvent += HidePossibleRoutes;

        playerTurn.endTurnEvent -= HidePossibleRoutes;
    }

    #region Event Subscribers
    public void HighlightPossibleRoutes(Unit unit)
    {
        List<Node> routes = Pathfinding.GetAllRoutes(gameMaster.GetMap(), unit);
        HidePossibleRoutes();
        Highlight(routes, ForecastTile.ForecastState.WithinReach);
    }

    public void HidePossibleRoutes(Node node, Ability ability)
    {
        HighlightAll(ForecastTile.ForecastState.Hidden);
    }

    public void HidePossibleRoutes(Unit unit)
    {
        HighlightAll(ForecastTile.ForecastState.Hidden);
    }

    public void HidePossibleRoutes()
    {
        HighlightAll(ForecastTile.ForecastState.Hidden);
    }

    public void HighlightAreaAbilityForecast(Node node, Ability ability)
    {
        List<Node> nodes = ability.GetAreaTargeting(node);
        Highlight(nodes, ForecastTile.ForecastState.AbilityForecast);
    }
    #endregion

    public void Highlight(List<Node> nodes, ForecastTile.ForecastState state)
    {
        Grid<Node> map = gameMaster.GetMap();

        foreach (Node node in nodes)
            node.forecastTile.SetState(state);
    }

    public void HighlightAll(ForecastTile.ForecastState state)
    {
        Grid<Node> map = gameMaster.GetMap();

        for (int i = 0; i < map.GetWidth(); i++)
            for (int j = 0; j < map.GetHeight(); j++)
                map.GetGridObject(i, j).forecastTile.SetState(state);
    }
}
