namespace SpriteTrail {
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    public sealed class TrailElement: MonoBehaviour {

        public static Stack<TrailElement> m_FreeElements = new Stack<TrailElement>();

        TrailPreset m_TrailSettings;
        float m_TimeSinceCreation;
        SpriteRenderer m_SpRenderer;
        public SpriteRenderer SpriteRenderer {
            get { return m_SpRenderer; }
            set { m_SpRenderer = value; }
        }

        bool m_Init = false;
        Vector3 m_InitSize;
        Vector3 m_InitPos;
        SpriteTrail m_MotherTrail;
        public bool m_NeedLateUpdate = false;
        public int m_TrailPos = -1;
        bool m_NeedDequeue = false;
        public Transform m_Transform;
        private TrailElement m_TrailElement;
		public GameObject m_myGameObject;


        private TrailElement GetTrailElement() {
            if (m_TrailElement != null) {
                return m_TrailElement;
            }

            return m_TrailElement = GetComponent<TrailElement>();
        }


        /// <summary>
        /// Auxiliary list to hold the current valid free elements.
        /// Used with <see cref="RefreshFreeElements"/> to avoid
        /// allocating a new list to iterate the stack.
        /// </summary>
        private static readonly List<TrailElement> m_ValidFreeElements = new List<TrailElement>();


        public static void RefreshFreeElements() {
            m_ValidFreeElements.Clear();

            //foreach to avoid allocating an array/list to iterate the stack
            foreach (var e in m_FreeElements) {
                //objects in unloaded or invalid scenes are not useful for the cache
                if (e != null && e.m_myGameObject.scene.IsValid() && e.m_myGameObject.scene.isLoaded) {
                    m_ValidFreeElements.Add(e);
                }
            }

            m_FreeElements.Clear();
            for (var i = 0; i < m_ValidFreeElements.Count; i++) {
                m_FreeElements.Push(m_ValidFreeElements[i]);
            }
        }


        public void Initialise(SpriteTrail trail) {
            m_NeedDequeue = false;
            m_TrailPos = -1;
            m_NeedLateUpdate = false;
            m_TimeSinceCreation = 0;
            m_MotherTrail = trail;
            this.m_myGameObject.SetActive(true);
            m_TrailSettings = trail.m_CurrentTrailPreset;
            if (SpriteRenderer == null) {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (m_TrailSettings.m_SpecialMat != null) {
                SpriteRenderer.material = m_TrailSettings.m_SpecialMat;
            } else {
                SpriteRenderer.material = trail.m_SpriteToDuplicate.material;
            }
            SpriteRenderer.color = trail.m_SpriteToDuplicate.color;
            SpriteRenderer.sortingLayerID = trail.m_SortingLayerID;
            SpriteRenderer.sortingOrder = trail.m_OrderInSortingLayer;
            SpriteRenderer.sprite = trail.m_SpriteToDuplicate.sprite;
            SpriteRenderer.flipX = trail.m_SpriteToDuplicate.flipX;
            SpriteRenderer.flipY = trail.m_SpriteToDuplicate.flipY;
            m_InitSize = m_Transform.localScale;
            m_InitPos = m_Transform.localPosition;
            m_Init = true;
            trail.m_ElementsInTrail.Enqueue(this);


			ApplyFrameEffect();

            if (m_TrailSettings.m_TrailElementDurationCondition == TrailElementDurationCondition.ElementCount) {
                if (m_TrailSettings.m_TrailMaxLength > 0) {
                    while (trail.m_ElementsInTrail.Count > m_TrailSettings.m_TrailMaxLength) {
                        trail.m_ElementsInTrail.Dequeue().Hide();
                    }
                } else {
                    while (trail.m_ElementsInTrail.Count > 0) {
                        trail.m_ElementsInTrail.Dequeue().Hide();
                    }
                }


                int _cnt = 0;
                foreach (TrailElement _elem in trail.m_ElementsInTrail) {
                    _elem.m_TrailPos = trail.m_ElementsInTrail.Count - _cnt;
                    _elem.m_NeedLateUpdate = true;
                    _cnt++;
                }
            }
        }


        private void Awake() {
            m_Transform = transform;
			m_myGameObject = gameObject;
		}


        private void Update() {
            if (!m_Init)
                return;

            m_TimeSinceCreation += Time.deltaTime;
            ApplyFrameEffect();
        }


        private void LateUpdate() {
            if (!m_NeedLateUpdate)
                return;

            ApplyAddSpriteEffect(m_TrailPos);
            m_NeedLateUpdate = false;
        }


        void ApplyAddSpriteEffect(int index) {
            if (m_TrailSettings.m_TrailMaxLength > 0) {
                ApplyModificationFromRatio(/*1f - */((float) index / (float) m_TrailSettings.m_TrailMaxLength));
            }

        }


        void ApplyFrameEffect() {
            float _Ratio = 0;
            if (m_TrailSettings.m_TrailDuration > 0)
                _Ratio = m_TimeSinceCreation / m_TrailSettings.m_TrailDuration;

            if (_Ratio >= 1) {
                Hide();
                return;
            }

            if (m_TrailSettings.m_TrailElementDurationCondition == TrailElementDurationCondition.Time)
                ApplyModificationFromRatio(_Ratio);
        }


        void ApplyModificationFromRatio(float ratio) {
            if (m_TrailSettings.m_UseOnlyAlpha) {
                Color _tmpCol = SpriteRenderer.color;
                _tmpCol.a = m_TrailSettings.m_TrailColor.Evaluate(ratio).a;
                SpriteRenderer.color = _tmpCol;
            } else
                SpriteRenderer.color = m_TrailSettings.m_TrailColor.Evaluate(ratio);

            if (m_TrailSettings.m_UseSizeModifier) {
                Vector3 _NewSize = new Vector3();
                _NewSize.x = m_InitSize.x * m_TrailSettings.m_TrailSizeX.Evaluate(ratio);
                _NewSize.y = m_InitSize.y * m_TrailSettings.m_TrailSizeY.Evaluate(ratio);
                _NewSize.z = 1f;
                m_Transform.localScale = _NewSize;
            }
            if (m_TrailSettings.m_UsePositionModifier) {
                Vector3 _NewPos = m_InitPos;
                _NewPos.x += m_TrailSettings.m_TrailPositionX.Evaluate(ratio);
                _NewPos.y += m_TrailSettings.m_TrailPositionY.Evaluate(ratio);
                m_Transform.localPosition = _NewPos;
            }
        }


        public static TrailElement GetFreeElement() {
            if (m_FreeElements.Count > 0) {
                return m_FreeElements.Pop();
            }

            var e = Instantiate(SpriteTrail.TrailElementTemplate).GetComponent<TrailElement>();
            e.SpriteRenderer = e.GetComponent<SpriteRenderer>();
            e.m_Transform = e.transform;
            return e;
        }


        public void Hide(bool AddToFree = true) {
            if (m_myGameObject == null || this == null)
                return;

            if (m_MotherTrail != null)
                m_NeedDequeue = true;
            if (m_MotherTrail != null && m_MotherTrail.m_ElementsInTrail.Count > 0 && m_MotherTrail.m_ElementsInTrail.Peek().m_NeedDequeue)
                m_MotherTrail.m_ElementsInTrail.Dequeue();
            this.m_myGameObject.SetActive(false);
            if (AddToFree) {
                var t = this.GetTrailElement();
                m_FreeElements.Push(t);
                t.m_NeedLateUpdate = false;
                t.m_Transform.SetParent(SpriteTrail.GlobalTrailContainer, true);
            }
            m_Init = false;
        }

    }
}
