using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    private PlayerTurn player;

    private void Start()
    {
        player = PlayerTurn.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (CombatState.state != BattleState.PLAYERTURN || PlayerTurn.instance.actionState == ActionState.CannotChoose)
            return;

        RaycastHit hit = GetClickData();
        if (hit.transform == null)
            return;

        if (hit.transform.gameObject.tag != "ForecastTile")
            return;

        ForecastTile fTile= hit.transform.GetComponent<ForecastTile>();
        Node node = fTile.GetNode();
        player.ChooseNode(node);
        Debug.Log("Hit Forecast tile : " + hit.transform.gameObject.name);
    }

    private RaycastHit GetClickData()
    {
        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f);
            if (hit.transform == null)
                Debug.Log("No Hit on Click");
        }
        return hit;
    }
}
