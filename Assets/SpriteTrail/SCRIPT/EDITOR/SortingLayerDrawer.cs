namespace SpriteTrail {
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;


    public sealed class SortingLayerDrawer {

        [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
        public sealed class SortingLayerPropertyDrawer: PropertyDrawer {

            private static readonly MethodInfo _SortingLayerFieldMethodInfo =
                    typeof(EditorGUI).GetMethod(
                            "SortingLayerField",
                            BindingFlags.Static | BindingFlags.NonPublic,
                            null,
                            new[]
                            {
                                typeof(Rect),
                                typeof(GUIContent),
                                typeof(SerializedProperty),
                                typeof(GUIStyle),
                                typeof(GUIStyle)
                            },
                            null);


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                if (property.propertyType != SerializedPropertyType.Integer) {
                    Debug.LogError("SortingLayer property should be an integer (the layer id)");
                } else {
                    SortingLayerField(position, label, property, EditorStyles.popup, EditorStyles.label);
                }
            }


            public static void SortingLayerField(Rect position,
                                                 GUIContent label,
                                                 SerializedProperty layerID,
                                                 GUIStyle style,
                                                 GUIStyle labelStyle) {

                if (_SortingLayerFieldMethodInfo != null) {
                    object[] parameters = new object[]
                    {
                        position,
                        label,
                        layerID,
                        style,
                        labelStyle
                    };
                    _SortingLayerFieldMethodInfo.Invoke(null, parameters);
                }
            }
        }

    }
}
