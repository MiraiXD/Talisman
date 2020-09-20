using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComNet;
using System;

public class PlayerController : MonoBehaviour
{
    public static Dictionary<PlayerInfo, PlayerController> players { get; } = new Dictionary<PlayerInfo, PlayerController>();
    private PlayerInfo _playerInfo;
    public PlayerInfo playerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            if (players.ContainsKey(_playerInfo) || _playerInfo == null) players.Remove(_playerInfo);
            if(_playerInfo != null) players.Add(_playerInfo, this);
        }
    }
    private CharacterModel model;

    private void Start()
    {
        model = GetComponent<CharacterModel>();
    }

}
