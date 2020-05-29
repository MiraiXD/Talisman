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

        try
        {
            print("T " + result.GetType().ToString());
            //ServerResponds.RequestResult requestResult = (ServerResponds.RequestResult)result;
            ServerResponds.RequestResult<ClientRequests.JoinRoom> requestResult = (ServerResponds.RequestResult<ClientRequests.JoinRoom>)result;
            Debug.Log((requestResult.obj == null).ToString() + ", " + requestResult.obj.GetType().ToString());
            GameRoom room = (GameRoom)requestResult.obj;
            Debug.Log("Success: " + requestResult.success + ", m: " + requestResult.message);
            foreach (Player player in room.players)
                Debug.Log(player.name);

            ClientHandleNetworkData.onServerRespond_1 -= JoinRoomResult;
        }
        catch { return; } // exception possible by casting to adequate RequestResult
        
    }
}
