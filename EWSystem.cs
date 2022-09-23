using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EWSystem
{
    public static class EWSystem
    {
        public static List<EWTransmitter> Transmitters { get; private set; } = new List<EWTransmitter>();
        public static List<EWReceiver> Receivers { get; private set; } = new List<EWReceiver>();

        public static void Simulate()
        {
            List<string> freqs = AssembleFreqList();

            foreach (string freq in freqs)
            {
                List<EWTransmitter> TXers = Transmitters.FindAll(x => x.Antenna.Frequency == freq && x.Transmitting);
                if (TXers.Count < 1) continue;

                List<EWReceiver> RXers = Receivers.FindAll(x => x.Antenna.Frequency == freq);

                EvaluateReceivers(RXers, TXers);
            }
        }

        /// <summary>
        /// NONFUNCTIONAL: Unity is really fussy about stupid thread safety, how rude.
        /// </summary>
        public static async void SimulateAsync ()
        {
            List<string> freqs = AssembleFreqList();
            List<Task> freqTasks = new List<Task>();

            foreach (string freq in freqs)
            {
                List<EWTransmitter> TXers = Transmitters.FindAll(x => x.Antenna.Frequency == freq
                && x.Transmitting);
                if (TXers.Count < 1) continue;

                List<EWReceiver> RXers = Receivers.FindAll(x => x.Antenna.Frequency == freq
                && x.Mode != EWReceiver.ReceiverMode.Disabled);
                if (RXers.Count < 1) continue;

                freqTasks.Add(Task.Run(() => EvaluateReceiversAsync(RXers, TXers)));
            }

            await Task.WhenAll(freqTasks);
        }

        private static List<string> AssembleFreqList ()
        {
            List<string> freqs = new List<string> { "none" };

            foreach (var rxer in Receivers)
            {
                if (freqs.Contains(rxer.Antenna.Frequency)) continue;

                freqs.Add(rxer.Antenna.Frequency);
            }

            return freqs;
        }

        private static void EvaluateReceivers (List<EWReceiver> RXers, List<EWTransmitter> TXers)
        {
            foreach(var RXer in RXers)
            {
                RXer.Listen(TXers);
            }
        }

        private static async void EvaluateReceiversAsync (List<EWReceiver> RXers, List<EWTransmitter> TXers)
        {
            List<Task> tasks = new List<Task>();

            foreach(var RXer in RXers)
            {
                tasks.Add(Task.Run(() => RXer.Listen(TXers)));
            }

            await Task.WhenAll(tasks);
        }

        public static void AddItem (EWTransmitter txer)
        {
            if (Transmitters.Contains(txer)) return;

            Transmitters.Add(txer);
        }

        public static void AddItem(EWReceiver rxer)
        {
            if (Receivers.Contains(rxer)) return;

            Receivers.Add(rxer);
        }
        public static void RemoveItem(EWTransmitter txer)
        {
            Transmitters.Remove(txer);
        }

        public static void RemoveItem(EWReceiver rxer)
        {
            Receivers.Remove(rxer);
        }

        public static void ResetLists ()
        {
            Receivers.Clear();
            Transmitters.Clear();
        }
    }
}
