using UnityEngine;

namespace EWSystem
{
    public struct EWSignalData
    {
        public EWTransmitter Transmitter;
        public Vector3 Direction;
        public float Strength;

        public EWSignalData (EWTransmitter tx, Vector3 dir, float strength = 0)
        {
            Transmitter = tx;
            Direction = dir;
            Strength = strength;
        }
    }
}
