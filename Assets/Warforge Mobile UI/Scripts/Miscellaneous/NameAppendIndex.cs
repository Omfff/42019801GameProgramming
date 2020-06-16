using UnityEngine;

namespace DuloGames.UI
{
    [ExecuteInEditMode]
    public class NameAppendIndex : MonoBehaviour
    {
        void Start()
        {
            if (!this.gameObject.activeInHierarchy)
                return;

            int index = 0;

            foreach (Transform child in this.transform.parent)
            {
                if (child.name.Contains(this.gameObject.name))
                    index++;
            }

            this.gameObject.name = this.gameObject.name + " " + index.ToString();
            DestroyImmediate(this); // Remove this script
        }
    }
}