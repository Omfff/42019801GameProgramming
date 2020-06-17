namespace SpriteTrail {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;


    [CustomEditor(typeof(SpriteTrail))]
    [CanEditMultipleObjects]
    public sealed class SpriteTrailEditor: Editor {

        SerializedProperty m_CurrentTrailPreset;
        SerializedProperty m_TrailParent;
        SerializedProperty m_SpriteToDuplicate;
        SerializedProperty m_Layer;
        SerializedProperty m_ZMoveStep;
        SerializedProperty m_ZMoveMax;
        SerializedProperty m_HideTrailOnDisabled;
        SerializedProperty m_TrailActivationCondition;
        SerializedProperty m_TrailDeactivationCondition;
        SerializedProperty m_StartIfUnderVelocity;
        SerializedProperty m_VelocityStartIsLocalSpace;
        SerializedProperty m_VelocityNeededToStart;
        SerializedProperty m_StopIfOverVelocity;
        SerializedProperty m_VelocityStopIsLocalSpace;
        SerializedProperty m_VelocityNeededToStop;
        SerializedProperty m_TrailActivationDuration;
        SerializedProperty m_TrailName;
        SerializedProperty m_SortingLayerID;
        SerializedProperty m_OrderInSortingLayer;


        void OnEnable() {
            m_CurrentTrailPreset = serializedObject.FindProperty("m_CurrentTrailPreset");
            m_TrailParent = serializedObject.FindProperty("m_TrailParent");
            m_SpriteToDuplicate = serializedObject.FindProperty("m_SpriteToDuplicate");
            m_Layer = serializedObject.FindProperty("m_Layer");
            m_ZMoveStep = serializedObject.FindProperty("m_ZMoveStep");
            m_ZMoveMax = serializedObject.FindProperty("m_ZMoveMax");
            m_HideTrailOnDisabled = serializedObject.FindProperty("m_HideTrailOnDisabled");
            m_TrailActivationCondition = serializedObject.FindProperty("m_TrailActivationCondition");
            m_TrailDeactivationCondition = serializedObject.FindProperty("m_TrailDeactivationCondition");
            m_StartIfUnderVelocity = serializedObject.FindProperty("m_StartIfUnderVelocity");
            m_VelocityStartIsLocalSpace = serializedObject.FindProperty("m_VelocityStartIsLocalSpace");
            m_VelocityNeededToStart = serializedObject.FindProperty("m_VelocityNeededToStart");
            m_StopIfOverVelocity = serializedObject.FindProperty("m_StopIfOverVelocity");
            m_VelocityStopIsLocalSpace = serializedObject.FindProperty("m_VelocityStopIsLocalSpace");
            m_VelocityNeededToStop = serializedObject.FindProperty("m_VelocityNeededToStop");
            m_TrailActivationDuration = serializedObject.FindProperty("m_TrailActivationDuration");
            m_TrailName = serializedObject.FindProperty("m_TrailName");
            m_SortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
            m_OrderInSortingLayer = serializedObject.FindProperty("m_OrderInSortingLayer");
        }


        public override void OnInspectorGUI() {
            serializedObject.Update();
            SpriteTrail TrailSettingsScript = target as SpriteTrail;

            EditorGUILayout.PropertyField(m_TrailName);
            GUILayout.Space(15);

            EditorGUILayout.PropertyField(m_CurrentTrailPreset);
            if (TrailSettingsScript.m_CurrentTrailPreset == null) {
                EditorGUILayout.HelpBox(
                        "You need to assign a preset (Current trail preset).\n You can create one, or use one of the preset In the folder : \nSpriteTrail/PREFAB/TRAIL_PRESETS",
                        MessageType.Warning,
                        true);
                GUILayout.Space(15);
            }

            EditorGUILayout.PropertyField(m_HideTrailOnDisabled);

            //GUILayout.Space(15);
            EditorGUILayout.PropertyField(m_TrailActivationCondition);
            switch (TrailSettingsScript.m_TrailActivationCondition) {
                case TrailActivationCondition.AlwaysEnabled:
                    break;
                case TrailActivationCondition.Manual:
                    break;
                case TrailActivationCondition.VelocityMagnitude:
                    EditorGUILayout.PropertyField(m_VelocityNeededToStart);
                    EditorGUILayout.PropertyField(m_StartIfUnderVelocity);
                    EditorGUILayout.PropertyField(m_VelocityStartIsLocalSpace);
                    GUILayout.Space(15);
                    break;
            }


            EditorGUILayout.PropertyField(m_TrailDeactivationCondition);
            switch (TrailSettingsScript.m_TrailDeactivationCondition) {
                case TrailDeactivationCondition.Manual:
                    break;
                case TrailDeactivationCondition.Time:
                    EditorGUILayout.PropertyField(m_TrailActivationDuration);
                    GUILayout.Space(15);
                    break;
                case TrailDeactivationCondition.VelocityMagnitude:
                    EditorGUILayout.PropertyField(m_VelocityNeededToStop);
                    EditorGUILayout.PropertyField(m_StopIfOverVelocity);
                    EditorGUILayout.PropertyField(m_VelocityStopIsLocalSpace);
                    GUILayout.Space(15);
                    break;
            }



            EditorGUILayout.PropertyField(m_TrailParent);
            EditorGUILayout.PropertyField(m_SpriteToDuplicate);
            EditorGUILayout.PropertyField(m_Layer);
            EditorGUILayout.PropertyField(m_ZMoveStep);
            EditorGUILayout.PropertyField(m_ZMoveMax);
            EditorGUILayout.PropertyField(m_SortingLayerID, new GUIContent("Sorting Layer", m_OrderInSortingLayer.tooltip));
            EditorGUILayout.PropertyField(m_OrderInSortingLayer, new GUIContent("Order in Layer", m_OrderInSortingLayer.tooltip));

            serializedObject.ApplyModifiedProperties();
        }

    }
}
