using System;
using System.Collections;
using System.Collections.Generic;
using Bindings;
using UnityEngine;

using UnityEngine.UI;

public class RoomsController : MonoBehaviour
{
    public GameObject roomPrefab;
    public Transform roomsContent;

    private void OnEnable()
    {
        ClientHandleNetworkData.onServerRespond_1 += OnRoomsListArrived;
        ClientTCP.RequestRoomsList();
        

    }

    private void OnRoomsListArrived(ServerPackets packetID, object obj)
    {
        if (packetID != ServerPackets.SReplyRoomsList) return;

        List<GameRoom> gameRooms = (List<GameRoom>)obj;
       foreach(GameRoom room in gameRooms)
        {            
            var r = Instantiate(roomPrefab, roomsContent);
            Text[] texts = r.GetComponentsInChildren<Text>();
            texts[0].text = room.name;
            texts[1].text = room.clients.Count + "/" + room.maxPlayers;

        }
    }
}
