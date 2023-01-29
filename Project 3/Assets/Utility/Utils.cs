using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void CreateWorldText(Vector3 localPosition, string text, int fontSize)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        //return textMesh;
    }
}
