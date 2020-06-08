using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public ComNet.MapTileInfo.MapTiles tileType;
    public ComNet.MapTileInfo tileInfo;
    [HideInInspector] public PlayerSpot[] playerSpots;
    private void Start()
    {
        playerSpots = GetComponentsInChildren<PlayerSpot>();
        // sort by entering order
        System.Array.Sort<PlayerSpot>(playerSpots, delegate (PlayerSpot a, PlayerSpot b) { return a.enteringOrder.CompareTo(b.enteringOrder); });
    }
}
