namespace SpriteTrail {
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public sealed class LayerAttributeEditor: PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.Integer) {
                Debug.LogError("Layer property should be an integer (the layer id)");
            } else {
                property.intValue = EditorGUI.LayerField(position, label, property.intValue);
            }
        }

    }
}
