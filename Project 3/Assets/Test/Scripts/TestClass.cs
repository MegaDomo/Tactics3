using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TestClass : MonoBehaviour
{
    public NodeHighlighter highlighter;
    public BattleSystem battleSystem;
    public PlayerTurn playerTurn;

    public int height;
    public int spread;

    private bool isTesting;
    private Vector3 direction;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
            battleSystem.Victory();

        if (Input.GetKeyDown(KeyCode.F1))
            isTesting = !isTesting;

        if (isTesting)
            TestMethod();
    }

    private void TestMethod()
    {
        Unit player = playerTurn.GetSelected();

        RaycastHit hoverHit = GetMouseHoverData(LayerMask.GetMask("ForecastTile"));
        if (hoverHit.transform == null)
            return;

        Node targetNode = hoverHit.transform.GetComponent<ForecastTile>().node;

        Node node = GetDirectionalNode(player.node, targetNode);

        List<Node> nodes = Pathfinding.GetTriangle(GameMaster.map, node, height, direction);
        nodes.Add(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityRange);
    }

    private RaycastHit GetMouseHoverData(LayerMask mask)
    {
        RaycastHit hit = new RaycastHit();
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, mask);
        }
        return hit;
    }

    private Node GetDirectionalNode(Node player, Node targetNode)
    {
        direction = Pathfinding.GetDirection(player, targetNode);

        int dirx = Mathf.RoundToInt(direction.x);
        int dirz = Mathf.RoundToInt(direction.z);
        int x = player.x + dirx;
        int z = player.z + dirz;

        return GameMaster.map.GetGridObject(x, z);
    }
}