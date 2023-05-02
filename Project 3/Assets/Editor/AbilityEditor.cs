using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit)), CanEditMultipleObjects]
public class AbilityEditor : Editor
{
    private Ability.TargetType type;

    public override void OnInspectorGUI()
    {
        //Unit.UnitType prop = (Unit.UnitType)serializedObject.FindProperty("unitType");

        type = (Ability.TargetType)EditorGUILayout.EnumPopup("Display", type);
        EditorGUILayout.Space();

        if (type == Ability.TargetType.DirectDamage)
        {
            Power();
        }

        if (type == Ability.TargetType.AreaDamage)
        {
            Power();
            AreaStuff();
        }

        if (type == Ability.TargetType.DirectHealing)
        {
            Power();
        }

        if (type == Ability.TargetType.AreaHealing)
        {
            Power();
            AreaStuff();
        }

        if (type == Ability.TargetType.DirectEffect)
        {
            Effect();
        }

        if (type == Ability.TargetType.AreaEffect)
        {
            AreaStuff();
            Effect();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("minRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxRange"));


        serializedObject.ApplyModifiedProperties();
    }

    private void AreaStuff()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aoeMinRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aoeMaxRange"));
    }

    private void Power()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("power"));
    }

    private void Effect()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectDuration"));
    }
}
