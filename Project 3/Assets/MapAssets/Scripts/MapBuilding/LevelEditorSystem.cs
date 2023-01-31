using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorSystem : MonoBehaviour
{
    public TileObject block;

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
            Debug.Log(x + " : " + z);
            Node node = grid.GetGridObject(x, z);
            if (node.CanBuild())
            {
                Transform blockFab = Instantiate(block.prefab, grid.GetWorldPosition(x, z), Quaternion.identity);
                node.SetBlock(blockFab);
            }
            else
            {
                Utils.CreateWorldTextPopup(Utils.GetMouseWorldPosition(), "Can't Build Here!", 30, TextAnchor.MiddleCenter);
            }
        }
    }
}
