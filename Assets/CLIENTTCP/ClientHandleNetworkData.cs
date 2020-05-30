
using Bindings;
using System;
using System.Collections.Generic;
using UnityEngine;

class ClientHandleNetworkData : MonoBehaviour
{
    private delegate void Packet_(byte[] data);
    private static Dictionary<int, Packet_> packets;
    
    public static Action<ServerPackets> onServerRespond_0;
    public static Action<ServerPackets, object> onServerRespond_1;    
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
            { (int)ServerPackets.SReplyRoomsList, HandleRoomsList},
            { (int)ServerPackets.SRequestResult, HandleRequestResult},
            };
    }

    private static void HandleRequestResult(byte[] data)
    {
        ServerResponds.RequestResult result = ClientTCP.GetData<ServerResponds.RequestResult>(data);//, out ServerPackets packetID);
        //what type is room
        ThreadSynchronizer.SyncTask(() => { onServerRespond_1?.Invoke(ServerPackets.SRequestResult, result); });
        
    }

    private static void HandleRoomsList(byte[] data)
    {        
        List<GameRoom> gameRooms = ClientTCP.GetData<List<GameRoom>>(data);
        ThreadSynchronizer.SyncTask(() => 
        {
            onServerRespond_1?.Invoke(ServerPackets.SReplyRoomsList, gameRooms);

        });
        
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
        //PacketBuffer buffer = new PacketBuffer();
        //buffer.WriteBytes(data);
        //buffer.ReadInteger();
        //string msg = buffer.ReadString();
        //buffer.Dispose();
        string msg = ClientTCP.GetString(data);
        // ADD YOUR CODE YOU WANT TO EXEC HERE
        //Debug.Log("time: " + (System.DateTime.Now - ClientTCP.t).TotalSeconds.ToString());
        Debug.Log(msg);

        ClientTCP.RequestRoomsList();
        //ClientTCP.ThankYouServer();
    }
}