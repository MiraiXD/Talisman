using System;
using System.Collections.Generic;
using Bindings;
namespace GameServer
{
    class ServerHandleNetworkData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initialize Network Packages");
            packets = new Dictionary<int, Packet_>()
            {
                { (int)ClientPackets.CThankYou, HandleThankYou}
            };
        }

        public static void HandleNetworkInformation(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packetNumber = buffer.ReadInteger();
            buffer.Dispose();
            if (packets.TryGetValue(packetNumber, out Packet_ Packet))
            {
                Packet.Invoke(index, data);
            }
        }

        private static void HandleThankYou(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            // ADD YOUR CODE YOU WANT TO EXEC HERE
            Console.WriteLine(msg);
        }
    }
}
