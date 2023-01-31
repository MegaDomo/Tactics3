using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Unit)), CanEditMultipleObjects]
public class UnitEditor : Editor
{/*
    public enum DisplayCategory { Basic, Combat, Hidden }

    public DisplayCategory categoryToDisplay;

    public override void OnInspectorGUI()
    {
        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        EditorGUILayout.Space();

        switch (categoryToDisplay)
        {
            case DisplayCategory.Basic:
                BasicUI();
                break;
            case DisplayCategory.Combat:
                CombatUI();
                break;
            case DisplayCategory.Hidden:
                HiddenUI();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void BasicUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHealth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("curHealth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("movement"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vision"));
    }

    void CombatUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attack"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spAttack"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defense"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spDefense"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("accuracy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("avoidance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("critChance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("critDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inflictionChance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inflictionResist"));
    }

    void HiddenUI()
    {
        
    }*/
}

