using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComNet;
using System;

public class PlayerController : MonoBehaviour
{    

    void Start()
    {
        ClientHandleNetworkData.onServerRespond_1 += CharacterAssignment;
        ClientTCP.SendObject(ClientPackets.CGameReady);
    }

    private void CharacterAssignment(ServerPackets packetID, object info)
    {
        if (packetID != ServerPackets.SCharacterAssignment) return;
        ComNet.CharacterInfo characterInfo;
        try
        {
            characterInfo = (ComNet.CharacterInfo)info;
        }catch { return; }

        ClientHandleNetworkData.onServerRespond_1 -= CharacterAssignment;

        //show the player the character he's been given
        CharacterUIController.ShowCard(characterInfo);

        //CharacterUIController.onCardShownOrHidden += OnCardHidden;
        //GameUIController.ShowCharacterCard(characterInfo);

        //spawn players' models on the board
    }

    private void OnCardHidden(bool cardShown)
    {
        if (cardShown) return;


    }
}
