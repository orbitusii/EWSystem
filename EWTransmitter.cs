using UnityEngine;

namespace EWSystem
{
    public abstract class EWTransmitter
    {
        public bool Transmitting;
        public float TransmissionPower = 1000f;

        public EWAntenna Antenna;

        public abstract Vector3 GetForwardDir();
        public abstract Vector64 GetPosition();
    }
}
