using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitTyping)), CanEditMultipleObjects]
public class UnitTypingEditor : Editor
{
    public Unit.UnitType type;

    public override void OnInspectorGUI()
    {
        //Unit.UnitType prop = (Unit.UnitType)serializedObject.FindProperty("unitType");

        type = (Unit.UnitType)EditorGUILayout.EnumPopup("Display", type);
        EditorGUILayout.Space();

        if (type == Unit.UnitType.Enemy)
            Enemy();

        serializedObject.ApplyModifiedProperties();
    }

    private void Enemy()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyObject"));
    }
}
