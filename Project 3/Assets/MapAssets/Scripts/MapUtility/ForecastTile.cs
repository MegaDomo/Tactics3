using UnityEngine;
using UnityEngine.EventSystems;

public class ForecastTile : MonoBehaviour
{
    public enum ForecastState { Hidden, Selected, WithinReach, OutOfReach, AbilityForecast }
    [Header("Unity References")]
    [SerializeField] private MeshRenderer mesh;

    [Header("Attributes")]
    [SerializeField] private Color def;
    [SerializeField] private Color selected;
    [SerializeField] private Color withinReach;
    [SerializeField] private Color outOfReach;
    [SerializeField] private Color ability;

    [HideInInspector] public Node node;

    private ForecastState forecastState;
    private Material mat;
    private PlayerTurn player;
    private Grid<Node> map;

    private void Awake()
    {
        mat = mesh.material;
        HideTile();
    }

    #region Color Methods
    public void SetState(ForecastState state)
    {
        switch (state)
        {
            case ForecastState.Hidden:
                HideTile();
                break;
            case ForecastState.Selected:
                TileSelected();
                break;
            case ForecastState.WithinReach:
                TileInReach();
                break;
            case ForecastState.OutOfReach:
                TileOutOfReach();
                break;
            case ForecastState.AbilityForecast:
                ForecastAbility();
                break;
        }
    }

    public void HideTile()
    {
        forecastState = ForecastState.Hidden;
        mat.color = def;
    }
    
    public void TileSelected()
    {
        forecastState = ForecastState.Selected;
        mat.color = selected;
    }

    public void TileInReach()
    {
        forecastState = ForecastState.WithinReach;
        mat.color = withinReach;
    }

    public void TileOutOfReach()
    {
        forecastState = ForecastState.OutOfReach;
        mat.color = outOfReach;
    }

    public void ForecastAbility()
    {
        forecastState = ForecastState.AbilityForecast;
        mat.color = ability;
    }
    #endregion

    #region Getters & Setters
    public Node GetNode()
    {
        return node;
    }

    public void SetNode(Node node)
    {
        this.node = node;
    }

    public ForecastState GetState()
    {
        return forecastState;
    }

    public void SetMap(Grid<Node> map)
    {
        this.map = map;
    }
    #endregion
}