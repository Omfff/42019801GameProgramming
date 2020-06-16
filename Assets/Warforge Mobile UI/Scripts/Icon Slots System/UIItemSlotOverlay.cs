using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	public class UIItemSlotOverlay : MonoBehaviour {
		
		[SerializeField] private UIItemSlot m_Slot;
		[SerializeField] private Image m_Target;
		
		protected void Start()
		{
			if (this.m_Target == null) this.m_Target = this.gameObject.GetComponent<Image>();
			
			if (this.m_Slot != null)
			{
				this.m_Slot.onAssign.AddListener(OnAssign);
				this.m_Slot.onUnassign.AddListener(OnUnassign);
			}
			
			if (this.m_Slot != null && this.m_Target != null)
			{
				this.m_Target.enabled = this.m_Slot.IsAssigned();
			}
		}
		
		private void OnAssign(UIItemSlot slot)
		{
			if (this.m_Target == null)
				return;
			
			this.m_Target.enabled = true;
		}
		
		private void OnUnassign(UIItemSlot slot)
		{
			if (this.m_Target == null)
				return;
			
			this.m_Target.enabled = false;
		}
	}
}