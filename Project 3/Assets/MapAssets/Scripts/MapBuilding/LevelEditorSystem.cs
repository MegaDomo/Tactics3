using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorSystem : MonoBehaviour
{
    public BlockObject blockObject;

    private bool canEdit;
    private Grid<Node> grid;

    // Start is called before the first frame update
    public void SetUp(bool canEdit)
    {
        this.canEdit = canEdit;
        grid = MapManager.instance.map;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canEdit)
        {
            grid.GetXZ(Utils.GetMouseWorldPosition(), out int x, out int z);
            Node node = grid.GetGridObject(x, z);
            if (node.CanBuild())
            {
                Transform blockFab = Instantiate(blockObject.prefab, grid.GetWorldPosition(x, z), Quaternion.identity);
                node.SetBlockObject(blockFab, blockObject);
            }
            else
            {
                Utils.CreateWorldTextPopup(Utils.GetMouseWorldPosition(), "Can't Build Here!", 30, TextAnchor.MiddleCenter);
            }
        }
    }
}
