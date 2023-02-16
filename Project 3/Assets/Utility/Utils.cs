using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetMouseWorldPosition()
    {
        //return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, LayerMask.GetMask("MouseTesting")))
            return raycastHit.point;
        else
            return Vector3.zero;
    }
    public static void CreateWorldText(Vector3 localPosition, string text, int fontSize, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        textMesh.anchor = textAnchor;
        //return textMesh;
    }
    public static void CreateWorldTextPopup(Vector3 localPosition, string text, int fontSize, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        textMesh.anchor = textAnchor;
        //return textMesh;
        
        Object.Destroy(gameObject, 1.3f);
    }

    public static void CreateWorldTextPopup(Vector3 localPosition, float duration, string text, int fontSize, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        textMesh.anchor = textAnchor;
        //return textMesh;

        Object.Destroy(gameObject, duration);
    }

    public static void CreateWorldTextPopupOnGrid(int x, int z, float duration, string text, int fontSize, Grid<Node> grid)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = grid.GetWorldPosition(x, z) +
                                   new Vector3(grid.GetCellSize() / 2, 8, grid.GetCellSize() / 2);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        textMesh.anchor = TextAnchor.MiddleCenter;        
        Object.Destroy(gameObject, duration);
    }
    public static void CreateWorldTextPopupOnGrid(Node node, float duration, string text, int fontSize, Grid<Node> grid)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = grid.GetWorldPosition(node.x, node.z) +
                                   new Vector3(grid.GetCellSize() / 2, 8, grid.GetCellSize() / 2);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        textMesh.anchor = TextAnchor.MiddleCenter;
        Object.Destroy(gameObject, duration);
    }
}
