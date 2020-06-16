using UnityEngine;

namespace DuloGames.UI
{
    [ExecuteInEditMode]
    public class UIMirrorDimensions : MonoBehaviour
    {
        [SerializeField] private RectTransform m_Target;

        protected void OnRectTransformDimensionsChange()
        {
            if (this.isActiveAndEnabled && this.m_Target != null)
            {
                RectTransform trans = this.transform as RectTransform;
                this.m_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trans.rect.width);
                this.m_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, trans.rect.height);
            }
        }
    }
}