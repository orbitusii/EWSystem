using System.Collections.Generic;
using UnityEngine;

namespace EWSystem
{
    public abstract class EWReceiver
    {
        public enum ReceiverMode
        {
            Disabled,
            Everything,
            Strongest,
        }

        public ReceiverMode Mode = ReceiverMode.Strongest;

        public float MinimumSignal = 0.00001f;

        public EWAntenna Antenna;

        public abstract Vector3 GetForwardDir();
        public abstract Vector64 GetPosition();

        protected Dictionary<EWTransmitter, float> HeardTransmitters;

        public void Listen(List<EWTransmitter> txers)
        {
            HeardTransmitters.Clear();

            Vector64 myPos = GetPosition();

            // Do the math stuff.
            foreach(EWTransmitter txer in txers)
            {
                Vector64 theirPos = txer.GetPosition();

                Vector64 path = theirPos - myPos;
                float RawPower = EWMath.InverseSquare(theirPos, myPos, txer.TransmissionPower);

                Vector3 dir = path.normalizedVector3;

                float myGain = Antenna.Gain.Evaluate(Vector3.Angle(GetForwardDir(), dir));
                float theirGain = txer.Antenna.Gain.Evaluate(Vector3.Angle(txer.GetForwardDir(), -dir));

                float endPower = RawPower * myGain * theirGain;
                if (endPower > MinimumSignal) HeardTransmitters.Add(txer, endPower);
            }

            if(Mode == ReceiverMode.Strongest)
            {
                KeyValuePair<EWTransmitter, float> strongest = new KeyValuePair<EWTransmitter, float>(null, -1);

                foreach(KeyValuePair<EWTransmitter, float> pair in HeardTransmitters)
                {
                    if (pair.Value > strongest.Value) strongest = pair;
                }
            }
        }


    }
}