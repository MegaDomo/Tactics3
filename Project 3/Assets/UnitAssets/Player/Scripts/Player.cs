using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Debugging")]
    public UnitStats stats;

    private void Start()
    {
        PlayerStats playerStats = new PlayerStats(stats);
        GetComponent<Unit>().SetAsPlayer(playerStats);
    }

    public void SetupPlayer(PlayerStats playerStats)
    {
        GetComponent<Unit>().SetAsPlayer(playerStats);
    }
}
