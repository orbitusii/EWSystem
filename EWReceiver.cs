using System.Collections.Generic;
using UnityEngine;
using DoublePreciseCoords;

namespace EWSystem
{
    [RequireComponent(typeof(DPCObject))]
    public class EWReceiver: MonoBehaviour
    {
        public enum ReceiverMode
        {
            Disabled,
            Everything,
            Strongest,
        }

        public ReceiverMode Mode = ReceiverMode.Strongest;

        public float MinimumSignal = 0.00001f;

        public EWAntenna Antenna = new EWAntenna("none", 180);

        public virtual Vector3 GetAntennaForward()
        {
            return TrnsRef.forward;
        }
        protected Transform TrnsRef;

        public virtual Vector64 GetPosition()
        {
            return DPCObjRef.Position;
        }
        protected DPCObject DPCObjRef;

        protected List<EWSignalData> ReceivedSignals = new List<EWSignalData>();
        protected bool SignalsAreFresh;

        public void Listen(List<EWTransmitter> txers)
        {
            ReceivedSignals.Clear();
            List<EWSignalData> possibleSignals = new List<EWSignalData>();

            Vector64 myPos = GetPosition();

            // Do the math stuff.
            foreach(EWTransmitter txer in txers)
            {
                Vector64 theirPos = txer.GetPosition();

                Vector64 path = theirPos - myPos;
                float RawPower = EWMath.InverseSquare(theirPos, myPos, txer.TransmissionPower);

                Vector3 dir = path.normalizedVector3;
                float incomingAngle = Vector3.Angle(GetAntennaForward(), dir);

                float myGain = Antenna.Gain.Evaluate(incomingAngle * Mathf.Deg2Rad);
                float theirGain = txer.Antenna.Gain.Evaluate(Vector3.Angle(txer.GetForwardDir(), -dir));

                Debug.Log(theirPos + " / " + myPos + " / " + incomingAngle + " / " + theirGain * myGain + " / " + RawPower);

                float endPower = RawPower * myGain * theirGain;
                if (endPower > MinimumSignal && incomingAngle < Antenna.MaxAngle)
                {
                    possibleSignals.Add(new EWSignalData(txer, dir, strength: endPower));
                }
            }

            if (possibleSignals.Count < 1) return;

            if(Mode == ReceiverMode.Strongest)
            {
                EWSignalData strongest = new EWSignalData(null, Vector3.zero, 0);

                foreach(EWSignalData dat in possibleSignals)
                {
                    if (dat.Strength > strongest.Strength) strongest = dat;
                }

                ReceivedSignals.Add(strongest);
            }
            else
            {
                ReceivedSignals = possibleSignals;
            }

            SignalsAreFresh = true;
        }

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
            Antenna.DrawDebugGizmos(transform, 1);
        }
    }
}