namespace SpriteTrail {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;


    [CustomEditor(typeof(TrailPreset))]
    [CanEditMultipleObjects]
    public sealed class TrailPresetEditor: Editor {

        SerializedProperty m_UseOnlyAlpha;
        SerializedProperty m_TrailColor;
        SerializedProperty m_TrailElementDurationCondition;
        SerializedProperty m_TrailMaxLength;
        SerializedProperty m_TrailDuration;
        SerializedProperty m_TrailElementSpawnCondition;
        SerializedProperty m_TimeBetweenSpawns;
        SerializedProperty m_FramesBetweenSpawns;
        SerializedProperty m_DistanceBetweenSpawns;
        SerializedProperty m_DistanceCorrection;
        SerializedProperty m_UseSizeModifier;
        SerializedProperty m_UsePositionModifier;
        SerializedProperty m_SpecialMat;
        SerializedProperty m_TrailSizeX;
        SerializedProperty m_TrailSizeY;
        SerializedProperty m_TrailPositionX;
        SerializedProperty m_TrailPositionY;


        void OnEnable() {
            m_UseOnlyAlpha = serializedObject.FindProperty("m_UseOnlyAlpha");
            m_TrailColor = serializedObject.FindProperty("m_TrailColor");
            m_TrailElementDurationCondition = serializedObject.FindProperty("m_TrailElementDurationCondition");
            m_TrailMaxLength = serializedObject.FindProperty("m_TrailMaxLength");
            m_TrailDuration = serializedObject.FindProperty("m_TrailDuration");
            m_TrailElementSpawnCondition = serializedObject.FindProperty("m_TrailElementSpawnCondition");
            m_TimeBetweenSpawns = serializedObject.FindProperty("m_TimeBetweenSpawns");
            m_FramesBetweenSpawns = serializedObject.FindProperty("m_FramesBetweenSpawns");
            m_DistanceBetweenSpawns = serializedObject.FindProperty("m_DistanceBetweenSpawns");
            m_DistanceCorrection = serializedObject.FindProperty("m_DistanceCorrection");
            m_UseSizeModifier = serializedObject.FindProperty("m_UseSizeModifier");
            m_UsePositionModifier = serializedObject.FindProperty("m_UsePositionModifier");
            m_TrailSizeX = serializedObject.FindProperty("m_TrailSizeX");
            m_TrailSizeY = serializedObject.FindProperty("m_TrailSizeY");
            m_TrailPositionX = serializedObject.FindProperty("m_TrailPositionX");
            m_TrailPositionY = serializedObject.FindProperty("m_TrailPositionY");
            m_SpecialMat = serializedObject.FindProperty("m_SpecialMat");
        }


        public override void OnInspectorGUI() {
            serializedObject.Update();
            TrailPreset TrailSettingsScript = target as TrailPreset;


            EditorGUILayout.PropertyField(m_TrailColor);
            EditorGUILayout.PropertyField(m_UseOnlyAlpha);
            EditorGUILayout.PropertyField(m_SpecialMat);



            GUILayout.Space(15);
            EditorGUILayout.PropertyField(m_TrailElementDurationCondition);
            switch (TrailSettingsScript.m_TrailElementDurationCondition) {
                case TrailElementDurationCondition.Time:
                    EditorGUILayout.PropertyField(m_TrailDuration);
                    break;
                case TrailElementDurationCondition.ElementCount:
                    EditorGUILayout.PropertyField(m_TrailMaxLength);
                    EditorGUILayout.PropertyField(m_TrailDuration);
                    break;
            }

            GUILayout.Space(15);
            EditorGUILayout.PropertyField(m_TrailElementSpawnCondition);
            switch (TrailSettingsScript.m_TrailElementSpawnCondition) {
                case TrailElementSpawnCondition.Time:
                    EditorGUILayout.PropertyField(m_TimeBetweenSpawns);
                    break;
                case TrailElementSpawnCondition.FrameCount:
                    EditorGUILayout.PropertyField(m_FramesBetweenSpawns);
                    break;
                case TrailElementSpawnCondition.Distance:
                    EditorGUILayout.PropertyField(m_DistanceBetweenSpawns);
                    EditorGUILayout.PropertyField(m_DistanceCorrection);
                    break;
            }

            GUILayout.Space(15);
            EditorGUILayout.PropertyField(m_UseSizeModifier);
            if (TrailSettingsScript.m_UseSizeModifier) {
                EditorGUILayout.PropertyField(m_TrailSizeX);
                EditorGUILayout.PropertyField(m_TrailSizeY);
            }

            GUILayout.Space(15);
            EditorGUILayout.PropertyField(m_UsePositionModifier);
            if (TrailSettingsScript.m_UsePositionModifier) {
                EditorGUILayout.PropertyField(m_TrailPositionX);
                EditorGUILayout.PropertyField(m_TrailPositionY);
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
