using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	[ExecuteInEditMode, RequireComponent(typeof(UnityEngine.UI.Toggle)), AddComponentMenu("UI/Toggle OnOff")]
	public class UIToggle_OnOff : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler {
		
		[SerializeField] private Image m_Target;
		[SerializeField] private Sprite m_ActiveSprite;
		[SerializeField] private Image m_PressOverlay;
		[SerializeField] private Vector2 m_PressActivePos = Vector2.zero;
		[SerializeField] private Vector2 m_PressInactivePos = Vector2.zero;
		[SerializeField] private bool m_InstaOut = true;
		
		public Toggle toggle {
			get { return this.gameObject.GetComponent<Toggle>(); }
		}
		
		protected void OnEnable()
		{
			this.toggle.onValueChanged.AddListener(OnValueChanged);
			this.DoTransition(false, true);
			this.OnValueChanged(this.toggle.isOn);
		}
		
		protected void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(OnValueChanged);
		}
		
#if UNITY_EDITOR
		protected void OnValidate()
		{
			this.DoTransition(false, true);
		}
#endif
		
		public void OnValueChanged(bool state)
		{
			if (this.m_Target == null || !this.isActiveAndEnabled)
				return;
			
			this.m_Target.overrideSprite = (state) ? this.m_ActiveSprite : null;
			
			// Reposition the press overlay
			if (this.m_PressOverlay != null)
			{
				// Active
				if (state)
				{
					this.m_PressOverlay.rectTransform.anchoredPosition = this.m_PressActivePos;
				}
				else
				{
					this.m_PressOverlay.rectTransform.anchoredPosition = this.m_PressInactivePos;
				}
			}
		}
		
		private void DoTransition(bool pressed, bool instant)
		{
			if (this.m_PressOverlay == null || !this.isActiveAndEnabled)
				return;
			
			if (instant || (!pressed && this.m_InstaOut))
			{
				this.m_PressOverlay.CrossFadeAlpha((pressed ? 1f : 0f), 0f, true);
			}
			else
			{
				this.m_PressOverlay.CrossFadeAlpha((pressed ? 1f : 0f), 0.1f, true);
			}
		}
		
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;
			
			this.DoTransition(true, false);
		}
		
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;
			
			this.DoTransition(false, false);
		}
	}
}