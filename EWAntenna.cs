using UnityEngine;

namespace EWSystem
{
    [System.Serializable]
    public struct EWAntenna
    {
        public string Frequency;
        [Tooltip("The gain of the antenna according to angle away from forward")]
        public AnimationCurve Gain;
        [Tooltip("The maximum angle that signals will be processed in")]
        public float MaxAngle;
        public Gradient GDebugColor;
        [Range(18, 36 * 4)]
        public int GainCurveIncrements;

        public EWAntenna (string freq = "none", float maxAngle = 180)
        {
            Frequency = freq;
            Gain = new AnimationCurve(new Keyframe(0, 1), new Keyframe(Mathf.PI, 0));
            MaxAngle = maxAngle;
            GDebugColor = new Gradient();
            GainCurveIncrements = 18;
        }

        public void DrawDebugGizmos (Transform transform, float SizeMultiplier)
        {
            for (float i = -Mathf.PI; i <= Mathf.PI; i += Mathf.PI / GainCurveIncrements)
            {
                Vector3 Dir = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i));

                float GdBi = Gain.Evaluate(Mathf.Abs(i));

                Gizmos.color = GDebugColor.Evaluate(GdBi);
                Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(Dir) * GdBi * SizeMultiplier);
            }
        }
    }
}