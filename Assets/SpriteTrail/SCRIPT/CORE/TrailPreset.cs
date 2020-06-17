namespace SpriteTrail {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public enum TrailElementSpawnCondition {

        Time,
        FrameCount,
        Distance

    }


    public enum TrailElementDurationCondition {

        Time,
        ElementCount

    }


    [CreateAssetMenu(menuName = "SpriteTrails/TrailPreset")]
    public sealed class TrailPreset: ScriptableObject {

        [Tooltip("Set this if you want to use a special material for your sprites. Leave blank otherwise")]
        public Material m_SpecialMat;

        [Tooltip("Tick this if you just want to modify the alpha channel over time, and not the rgb (set in the \"Trail Color\" gradient)")]
        public bool m_UseOnlyAlpha;

        [Tooltip("Trail color over time")]
        public Gradient m_TrailColor;

        [Tooltip("Trail element dissapear condition (max time or max element count)")]
        public TrailElementDurationCondition m_TrailElementDurationCondition = TrailElementDurationCondition.Time;

        [Tooltip("Length of the trail in elements")]
        public int m_TrailMaxLength = 10;

        [Tooltip("Max duration of each trail element (in seconds) (-1 = infinite (not recommanded))")]
        public float m_TrailDuration = .5f;

        [Tooltip(
                "Condition needed for the trail element to spawn : \n- Time : interval in second between each spawn\n- FrameCount : interval in frames\n- Distance : If the object is too far from th previous, spawn an element"
                )]
        public TrailElementSpawnCondition m_TrailElementSpawnCondition = TrailElementSpawnCondition.Time;

        [Tooltip("Time between each trail element spawn")]
        public float m_TimeBetweenSpawns;

        [Tooltip("Frames between each trail element spawn")]
        public int m_FramesBetweenSpawns;

        [Tooltip("Distance between each trail element spawn")]
        public float m_DistanceBetweenSpawns;

        [Tooltip("Check this to calculate the error between each iteration and fill the gap")]
        public bool m_DistanceCorrection = true;

        [Tooltip("Modify element size over time")]
        public bool m_UseSizeModifier;

        [Tooltip("Modify element position over time")]
        public bool m_UsePositionModifier;

        [Tooltip("Trail size X over time")]
        public AnimationCurve m_TrailSizeX;

        [Tooltip("Trail size Y over time")]
        public AnimationCurve m_TrailSizeY;

        [Tooltip("Trail position X over time")]
        public AnimationCurve m_TrailPositionX;

        [Tooltip("Trail position Y over time")]
        public AnimationCurve m_TrailPositionY;

    }
}
