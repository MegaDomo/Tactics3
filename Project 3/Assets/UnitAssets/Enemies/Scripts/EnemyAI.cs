using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In Control of the whole enemy team
public class EnemyAI : MonoBehaviour
{
    #region Singleton
    public static EnemyAI instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private Unit selected;
    public IEnumerator StartTurn()
    {
        selected.behavior.FindTarget();

        selected.behavior.FindNodeToTarget();

        selected.behavior.Move();

        // TODO : NEEDS to be a variable based on length of movement occuring
        yield return new WaitForSeconds(1f);

        EndTurn();
        //selected.behavior.FindTarget();
    }

    public void EndTurn()
    {
        BattleSystem.instance.GetNextInitiative();
    }

    #region Setters
    public void SetSelected(Unit _selected)
    {
        selected = _selected;
    }
    #endregion
}
