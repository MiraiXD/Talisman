using UnityEngine;
using ComNet;
using System;
using System.Collections.Generic;

public class TalismanClient : MonoBehaviour
{
    public static TalismanPlayerInfo talismanPlayerInfo;
    void Start()
    {
        if (ClientTCP.playerInfo == null) { Debug.LogError("PLayerInfo uninitialized!"); return; }

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
        //ClientHandleNetworkData.onServerRespond_1 += CharacterAssignment;
        ClientTCP.SendObject(ClientPackets.CGameReady);
    }
    public static void ChooseYourCharacter(TalismanPlayerInfo info)
    {
        talismanPlayerInfo = info;
        CharacterUIController.Enable();               
    }

    public static void CharactersAssigned(ServerResponds.CharactersAssigned charactersAssigned)
    {
        for (int i = 0; i < charactersAssigned.playerInfos.Count; i++)
        {
            CharacterModel model = CharacterModelController.SpawnModel(charactersAssigned.characterInfos[i]);
            model.GetComponent<PlayerController>().playerInfo = charactersAssigned.playerInfos[i];
        }        
    }

    public static void PlayerTurn(PlayerInfo playerInfo)
    {
        Debug.Log("Player " + playerInfo.inRoomID + " turn ");
        if (PlayerController.players.TryGetValue(playerInfo.inRoomID, out PlayerController player))
        {
            CharacterUIController.Disable();
            CameraController.LookAt(player.transform);
            PlayerUIController.Enable();
        }
        else { Debug.LogError("No such player"); }

    }
    public static void RollResult(RollInfo rollInfo)
    {
        PlayerUIController.RollResult(rollInfo);
    }
    //private static void OnRollResult(ServerPackets packetID, object obj)
    //{
    //    throw new NotImplementedException();
    //}

    //public static void Roll()
    //{
    //    PlayerUIController.EnableRoll(false);
    //    ClientTCP.SendObject(ClientPackets.CRoll);
    //}
}