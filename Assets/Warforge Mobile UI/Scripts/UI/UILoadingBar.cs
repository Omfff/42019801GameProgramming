using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Tweens;
using UnityEngine.SceneManagement;

namespace DuloGames.UI
{
	public class UILoadingBar : MonoBehaviour {
		
		[Serializable] public class OnChangeEvent : UnityEvent<float> {}
		
		public enum Type
		{
			Normal,
			Masked,
		}
		
		[SerializeField] [Range(0f, 1f)] private float m_FillAmount = 1f;
		[SerializeField] private Type m_Type = Type.Normal;
		[SerializeField] private Image m_TargetImage;
		[SerializeField] private RectTransform m_TargetTransform;
		[SerializeField] private float m_TransformWidth = 100f;
		[SerializeField] private Text m_TextComponent;
		[SerializeField] private bool m_IsDemo = false;
		[SerializeField] private float m_Duration = 5f;
		[SerializeField] private string m_LoadScene = "";
		[SerializeField] private OnChangeEvent m_OnChange = new OnChangeEvent();
		
		// Tween controls
		[NonSerialized] private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
		
		// Called by Unity prior to deserialization, 
		// should not be called by users
		protected UILoadingBar()
		{
			if (this.m_FloatTweenRunner == null)
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			
			this.m_FloatTweenRunner.Init(this);
		}
		
		protected void OnEnable()
		{
			if (this.m_TextComponent != null)
				this.m_TextComponent.text = "0%";
			
			if (this.m_IsDemo)
				this.StartDemoTween();
		}
		
#if UNITY_EDITOR
		protected void OnValidate()
		{
			this.FillAmountChanged();
		}
#endif

		protected void SetFillAmount(float amount)
		{
			if (this.m_FillAmount != Mathf.Clamp01(amount))
			{
				this.m_FillAmount = Mathf.Clamp01(amount);
				this.FillAmountChanged();
			}
		}
		
		protected void FillAmountChanged()
		{
			if (this.m_Type == Type.Normal)
			{
				// Apply fill amount to the target image
				if (this.m_TargetImage != null)
					this.m_TargetImage.fillAmount = this.m_FillAmount;
			}
			else
			{
				// Apply width to the target transform
				if (this.m_TargetTransform != null)
				{
					this.m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (this.m_TransformWidth * this.m_FillAmount));
				}
			}
			
			if (this.m_TextComponent != null)
				this.m_TextComponent.text = (this.m_FillAmount * 100).ToString("0") + "%";
			
			if (this.m_OnChange != null)
				this.m_OnChange.Invoke(this.m_FillAmount);
		}
		
		public void StartDemoTween()
		{
			if ((this.m_Type == Type.Normal && this.m_TargetImage == null) || (this.m_Type == Type.Masked && this.m_TargetTransform == null))
				return;
			
			float targetAmount = (this.m_FillAmount > 0.5f) ? 0f : 1f;
		
			FloatTween floatTween = new FloatTween { duration = this.m_Duration, startFloat = this.m_FillAmount, targetFloat = targetAmount };
			floatTween.AddOnChangedCallback(SetFillAmount);
			floatTween.AddOnFinishCallback(OnTweenFinished);
			floatTween.ignoreTimeScale = true;
			this.m_FloatTweenRunner.StartTween(floatTween);
		}
		
		protected void OnTweenFinished()
		{
			if (!string.IsNullOrEmpty(this.m_LoadScene))
			{
				SceneManager.LoadScene(this.m_LoadScene);
			}
			else
			{
				this.StartDemoTween();
			}
		}
	}
}