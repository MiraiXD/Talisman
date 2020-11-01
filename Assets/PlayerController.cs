using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComNet;
using System;

public class PlayerController : MonoBehaviour
{
    public static Dictionary<int , PlayerController> players { get; } = new Dictionary<int, PlayerController>();
    private PlayerInfo _playerInfo;
    public PlayerInfo playerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            if (players.ContainsKey(_playerInfo.inRoomID)) players.Remove(_playerInfo.inRoomID);
            if(_playerInfo != null) players.Add(_playerInfo.inRoomID, this);Debug.Log("GYH" + playerInfo);
        }
    }
    private CharacterModel model;

    private void Start()
    {
        model = GetComponent<CharacterModel>();
    }

}
