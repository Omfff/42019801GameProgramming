using UnityEngine;
using DuloGames.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DuloGamesEditor.UI
{
	[CustomEditor(typeof(UIItemSlot), true)]
	public class UIItemSlotEditor : UISlotBaseEditor {
		
		private SerializedProperty onAssignProperty;
		private SerializedProperty onUnassignProperty;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			this.onAssignProperty = this.serializedObject.FindProperty("onAssign");
			this.onUnassignProperty = this.serializedObject.FindProperty("onUnassign");
		}
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			EditorGUILayout.Separator();
			
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.onAssignProperty, new GUIContent("On Assign"), true);
			EditorGUILayout.PropertyField(this.onUnassignProperty, new GUIContent("On Unassign"), true);
			this.serializedObject.ApplyModifiedProperties();
		}
	}
}