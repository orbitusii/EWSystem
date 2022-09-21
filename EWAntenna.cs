using UnityEngine;

namespace EWSystem
{
    [System.Serializable]
    public struct EWAntenna
    {
        public string Frequency;
        [Tooltip("The gain of the antenna according to angle away from forward")]
        public AnimationCurve Gain;
        public float MaxAngle;

        public EWAntenna (string freq = "none", float maxAngle = 180)
        {
            Frequency = freq;
            Gain = new AnimationCurve(new Keyframe(0, 1));
            MaxAngle = maxAngle;
        }
    }
}