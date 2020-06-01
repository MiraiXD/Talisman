using System;
using System.Collections;
using System.Collections.Generic;
using ComNet;
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
        ClientHandleNetworkData.onServerRespond_1 += RoomsListResult;
        //ClientTCP.RequestRoomsList();
        ClientHandleNetworkData.RequestRoomsList();
        menuController = FindObjectOfType<MenuController>();
    }
    private void OnDisable()
    {
        ClientHandleNetworkData.onServerRespond_1 -= RoomsListResult;
    }
    private void RoomsListResult(ServerPackets packetID, object result)
    {
        if (packetID != ServerPackets.SRequestResult) return;
        ServerResponds.RequestResult<ClientRequests.RoomsList> requestResult;
        try
        {
            requestResult = (ServerResponds.RequestResult<ClientRequests.RoomsList>)result;
        }
        catch { return; }

        GameRoomInfo[] gameRoomInfos = (GameRoomInfo[])requestResult.obj;
        foreach (GameRoomInfo info in gameRoomInfos)
        {
            var r = Instantiate(roomPrefab, roomsContent);
            Text[] texts = r.GetComponentsInChildren<Text>();
            texts[0].text = info.name;
            texts[1].text = info.clientInfos.Count + "/" + info.maxPlayers;
            r.GetComponent<Button>().onClick.AddListener(() =>
            {               
                    ClientHandleNetworkData.onServerRespond_1 += JoinRoomResult;
                    ClientTCP.SendObject(ClientPackets.CJoinRoom, new ClientRequests.JoinRoom(info));                
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

        GameRoomInfo info = (GameRoomInfo)requestResult.obj;      

        ClientHandleNetworkData.onServerRespond_1 -= JoinRoomResult;

        if (requestResult.success) { UnityEngine.SceneManagement.SceneManager.LoadScene("Game"); }

    }
}
