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

        show info about character
    }

    void Update()
    {
        
    }
}
