using UnityEngine;
using ComNet;
using System;

public class TalismanClient : MonoBehaviour
{
    public static TalismanPlayerInfo playerInfo;
    void Start()
    {
        if(ClientTCP.playerInfo == null) { Debug.LogError("PLayerInfo uninitialized!"); return; }

        Map.CreateMap();
        ClientHandleNetworkData.onServerRespond_1 += CheckMapInfo;

        if (ClientTCP.playerInfo.isAdmin)
        {
            Debug.Log("Sending MapInfo to server");
            ClientTCP.SendObject(ClientPackets.CAdminMapInfo, Map.mapInfo);
        }
    }
    private static void CheckMapInfo(ServerPackets packetID, object obj)
    {
        if (packetID != ServerPackets.SMapInfo) return;
        MapInfo serverMapInfo;
        try
        {
            serverMapInfo = (MapInfo)obj;
        }
        catch { return; }

        ClientHandleNetworkData.onServerRespond_1 -= CheckMapInfo;

        if (Map.mapInfo == null) Debug.LogError("MapInfo is null!");
        if (Map.mapInfo.CompareTo(serverMapInfo) != 0) Debug.LogError("Local map is different from the one received from server!");
        else
        {
            Debug.Log("Map is correct. Starting game");
            GameReady();
        }
    }
    public static void GameReady()
    {
        ClientHandleNetworkData.onServerRespond_1 += CharacterAssignment;
        ClientTCP.SendObject(ClientPackets.CGameReady);
    }
    private static void CharacterAssignment(ServerPackets packetID, object obj)
    {
        if (packetID != ServerPackets.SCharacterAssignment) return;
        TalismanPlayerInfo[] playerInfos;
        try
        {
            playerInfos= (TalismanPlayerInfo[])obj;
        }
        catch { return; }

        ClientHandleNetworkData.onServerRespond_1 -= CharacterAssignment;

        
        foreach(TalismanPlayerInfo info in playerInfos)
        {
            CharacterModel model = CharacterModelController.SpawnModel(info.characterInfo);
            model.GetComponent<PlayerController>().playerInfo = info;
            if (info.inRoomID == ClientTCP.playerInfo.inRoomID)
            {
                playerInfo = info;
            }
        }
        if(playerInfo == null)
        {
            Debug.LogError("Wrong playerID");
            return;
        }
        //show the player the character he's been given
        CharacterUIController.ShowCard(playerInfo.characterInfo);
        CharacterUIController.onOKPressed += OnCharacterAccepted;
        //CharacterUIController.onCardShownOrHidden += OnCardHidden;
        //GameUIController.ShowCharacterCard(characterInfo);        
    }

    private static void OnCharacterAccepted()
    {
        CharacterUIController.onOKPressed -= OnCharacterAccepted;

        //ClientHandleNetworkData.onServerRespond_1 += OnPlayerTurn;
        ClientTCP.SendObject(ClientPackets.CCharacterAccepted);
    }

    public static void OnPlayerTurn(TalismanPlayerInfo playerInfo)
    {
        Debug.Log("Player " + playerInfo.inRoomID + " turn ");


    }
    public static void Roll()
    {

    }
}