
using Bindings;
using System;
using System.Collections.Generic;
using UnityEngine;

class ClientHandleNetworkData : MonoBehaviour
{
    private delegate void Packet_(byte[] data);
    private static Dictionary<int, Packet_> packets;
    private void Awake()
    {
        InitializeNetworkPackages();
    }

    public static void InitializeNetworkPackages()
    {
        Debug.Log("Initialize Network Packages");
        packets = new Dictionary<int, Packet_>()
            {
                { (int)ServerPackets.SConnectionOK, HandleConnectionOK},
            { (int)ServerPackets.SReplyRoomsList, HandleRoomsList}
            };
    }

    private static void HandleRoomsList(byte[] data)
    {
        PacketBuffer receivedBuffer = new PacketBuffer();
        receivedBuffer.WriteBytes(data);
        receivedBuffer.ReadInteger();
        string msg = receivedBuffer.ReadString();
        receivedBuffer.Dispose();
        Debug.Log(msg);
    }

    public static void HandleNetworkInformation(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        int packetNumber = buffer.ReadInteger();
        buffer.Dispose();
        if (packets.TryGetValue(packetNumber, out Packet_ Packet))
        {
            Packet.Invoke(data);
        }
    }

    private static void HandleConnectionOK(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();

        // ADD YOUR CODE YOU WANT TO EXEC HERE
        Debug.Log("time: " + (System.DateTime.Now - ClientTCP.t).TotalSeconds.ToString());
        Debug.Log(msg);

        ClientTCP.RequestRoomsList();
        //ClientTCP.ThankYouServer();
    }
}