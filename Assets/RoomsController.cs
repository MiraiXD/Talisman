using System;
using System.Collections;
using System.Collections.Generic;
using Bindings;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class RoomsController : MonoBehaviour
{
    public GameObject roomPrefab;
    public Transform roomsContent;
    private MenuController menuController;

    private void OnEnable()
    {
        ClientHandleNetworkData.onServerRespond_1 += OnRoomsListArrived;
        ClientTCP.RequestRoomsList();
        menuController = FindObjectOfType<MenuController>();
    }

    private void OnRoomsListArrived(ServerPackets packetID, object obj)
    {
        if (packetID != ServerPackets.SReplyRoomsList) return;

        List<GameRoom> gameRooms = (List<GameRoom>)obj;
        foreach (GameRoom room in gameRooms)
        {
            var r = Instantiate(roomPrefab, roomsContent);
            Text[] texts = r.GetComponentsInChildren<Text>();
            texts[0].text = room.name;
            texts[1].text = room.players.Count + "/" + room.maxPlayers;
            r.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (string.IsNullOrWhiteSpace(menuController.playerNameInputField.text))
                    Debug.LogError("error, puste! Imię");
                else
                {
                    ClientHandleNetworkData.onServerRespond_1 += JoinRoomResult;
                    ClientTCP.JoinRoom(new ClientRequests.JoinRoom(room, menuController.playerNameInputField.text));
                }
            });
        }
    }

    private void JoinRoomResult(ServerPackets packetID, object result)
    {
        if (packetID != ServerPackets.SRequestResult) return;
        ServerResponds.RequestResult<ClientRequests.JoinRoom> requestResult;
        try
        {
            requestResult = (ServerResponds.RequestResult<ClientRequests.JoinRoom>)result;
        }
        catch { return; }

        GameRoom room = (GameRoom)requestResult.obj;
        print(room.players[0].name);

        ClientHandleNetworkData.onServerRespond_1 -= JoinRoomResult;
    }
}
