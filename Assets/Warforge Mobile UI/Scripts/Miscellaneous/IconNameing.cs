using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
    [ExecuteInEditMode]
    public class IconNameing : MonoBehaviour
    {

        [SerializeField]
        private Image m_Image;

        // Use this for initialization
        void Start()
        {
            SetName();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            SetName();
        }
#endif

        private void SetName()
        {
            if (m_Image != null)
            {
                gameObject.name = "Button (" + m_Image.sprite.name + ")";
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Remove All Scripts")]
        public void RemoveAll()
        {
            IconNameing[] scripts = Object.FindObjectsOfType<IconNameing>();

            foreach (IconNameing script in scripts)
            {
                DestroyImmediate(script);
            }
        }
#endif
    }
}