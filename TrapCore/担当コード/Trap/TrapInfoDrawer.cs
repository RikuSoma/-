using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TrapInfo))]
public class TrapInfoDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty kindProp = property.FindPropertyRelative("TrapKind");
        TrapKinds kind = (TrapKinds)kindProp.enumValueIndex;

        float baseHeight = EditorGUIUtility.singleLineHeight * 3; // TrapPos + TrapKind + spacing

        if (kind == TrapKinds.FakeNeedle)
        {
            SerializedProperty settingsProp = property.FindPropertyRelative("fakeNeedleSettings");
            return baseHeight + EditorGUI.GetPropertyHeight(settingsProp, true) + 4;
        }

        return baseHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty trapPosProp = property.FindPropertyRelative("TrapPos");
        SerializedProperty trapKindProp = property.FindPropertyRelative("TrapKind");
        SerializedProperty fakeNeedleSettingsProp = property.FindPropertyRelative("fakeNeedleSettings");

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(rect, trapPosProp);
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        EditorGUI.PropertyField(rect, trapKindProp);
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        TrapKinds kind = (TrapKinds)trapKindProp.enumValueIndex;

        if (kind == TrapKinds.FakeNeedle)
        {
            EditorGUI.PropertyField(rect, fakeNeedleSettingsProp, true);
        }
    }
}
#endif