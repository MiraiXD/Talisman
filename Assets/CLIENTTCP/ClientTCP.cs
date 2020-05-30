using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using Bindings;
using Newtonsoft.Json;
public class ClientTCP : MonoBehaviour
{
    public string IP_Adress;
    public int port;

    public static Socket _clientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _asyncBuffer = new byte[1024];
    private static JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All};
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to server...");        
        _clientSocket.BeginConnect(IP_Adress, port, new AsyncCallback(ConnectCallback), _clientSocket);
        
        DontDestroyOnLoad(this.gameObject);
    }
    private void ConnectCallback(IAsyncResult ar)
    {
        _clientSocket.EndConnect(ar);
        while (true)
        {
            OnReceive();
        }
    }

    private void OnReceive()
    {
        byte[] _sizeInfo = new byte[4];
        byte[] _receivedBuffer = new byte[1024];

        int totalRead = 0, currentRead = 0;
        try
        {
            currentRead = totalRead = _clientSocket.Receive(_sizeInfo);
            if (totalRead <= 0)
            {
                Console.WriteLine("You are not connected to the server");
            }
            else
            {
                while (totalRead < _sizeInfo.Length && currentRead > 0)
                {
                    currentRead = _clientSocket.Receive(_sizeInfo, totalRead, _sizeInfo.Length - totalRead, SocketFlags.None);
                    totalRead += currentRead;
                }

                int messageSize = 0;
                messageSize |= _sizeInfo[0];
                messageSize |= (_sizeInfo[1] << 8);
                messageSize |= (_sizeInfo[2] << 16);
                messageSize |= (_sizeInfo[3] << 24);

                byte[] data = new byte[messageSize];

                totalRead = 0;
                currentRead = totalRead = _clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                while (totalRead < messageSize && currentRead > 0)
                {
                    currentRead = _clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                    totalRead += currentRead;
                }
                ClientHandleNetworkData.HandleNetworkInformation(data);
            }
        }
        catch
        {
            Console.WriteLine("You are not connected to the server");
        }
    }

    public static void SendData(byte[] data)
    {
        _clientSocket.Send(data);
    }
    public static void RequestRoomsList()
    {
        //PacketBuffer buffer = new PacketBuffer();
        //buffer.WriteInteger((int)ClientPackets.CRequestRoomsList);
        //SendData(buffer.ToArray());
        //buffer.Dispose();
        SendString(ClientPackets.CRequestRoomsList);
    }
    public static void CreateRoom(ClientRequests.CreateRoom request)
    {        
        SendString(ClientPackets.CCreateRoom, JsonConvert.SerializeObject(request, settings));
    }
    public static void JoinRoom(ClientRequests.JoinRoom request)
    {        
        SendString(ClientPackets.CJoinRoom, JsonConvert.SerializeObject(request, settings));
    }
    public static void SendString(ClientPackets packetID, string msg = null)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)packetID);
        if(msg != null)
            buffer.WriteString(msg);

        SendData(buffer.ToArray());
        buffer.Dispose();
    } 
    public static string GetString(byte[] data, out ServerPackets packetID)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        packetID = (ServerPackets) buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();
        return msg;
    }
    public static string GetString(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();
        return msg;
    }
    public static T GetData<T>(byte[] data, out ServerPackets packetID)
    {
        string msg = GetString(data, out packetID);        
        T obj = default;
        try
        {
            obj = JsonConvert.DeserializeObject<T>(msg, settings);
        }
        catch (Exception e) { ThreadSynchronizer.SyncTask(() => { Debug.LogException(e); }); }
        return obj;
    }
    public static T GetData<T>(byte[] data)
    {
        string msg = GetString(data);
        T obj = default;
        try
        {
            JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            obj = JsonConvert.DeserializeObject<T>(msg, settings);
        }
        catch (Exception e) { ThreadSynchronizer.SyncTask(() => { Debug.LogException(e); }); }
        return obj;
    }
}
