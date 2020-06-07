using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public ComNet.MapTileInfo.MapTiles tileType;
    public ComNet.MapTileInfo tileInfo;
    private PlayerSpot[] playerSpots;
    private void Start()
    {
        playerSpots = GetComponentsInChildren<PlayerSpot>();
    }
}
