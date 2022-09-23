using UnityEngine;
using DoublePreciseCoords;

namespace EWSystem
{
    [RequireComponent(typeof(DPCObject))]
    public class EWTransmitter: MonoBehaviour
    {
        public bool Transmitting = true;
        public float TransmissionPower = 1000f;

        public EWAntenna Antenna = new EWAntenna("none", 180);

        public virtual Vector3 GetForwardDir()
        {
            return TrnsRef.forward;
        }
        protected Transform TrnsRef;

        public virtual Vector64 GetPosition()
        {
            return DPCObjRef.Position;
        }
        protected DPCObject DPCObjRef;

        public virtual void Start()
        {
            EWSystem.AddItem(this);
            DPCObjRef = GetComponent(typeof(DPCObject)) as DPCObject;
            TrnsRef = transform;
        }

        public virtual void OnDestroy()
        {
            EWSystem.RemoveItem(this);
        }

        void OnDrawGizmosSelected()
        {
            //if (GDebugStep < 0.01) return;
            Antenna.DrawDebugGizmos(transform, TransmissionPower);
        }
    }
}
