using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComNet;
using System;

public class PlayerController : MonoBehaviour
{
    public static Dictionary<TalismanPlayerInfo, PlayerController> players = new Dictionary<TalismanPlayerInfo, PlayerController>();
    private TalismanPlayerInfo _playerInfo;
    public TalismanPlayerInfo playerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            if (players.ContainsKey(_playerInfo)) players.Remove(_playerInfo);
            players.Add(_playerInfo, this);
        }
    }
    private CharacterModel model;

    private void Start()
    {
        model = GetComponent<CharacterModel>();
    }

}
