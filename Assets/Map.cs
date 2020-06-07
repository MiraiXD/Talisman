using UnityEngine;
using System.Collections.Generic;
using ComNet;
using System;

public class Map : MonoBehaviour ,ISerializationCallbackReceiver
{    
    private static MapTile[] tiles;
    public static MapInfo mapInfo;

    [SerializeField] private CatmullRomSpline _outerMapSpline;
    private static CatmullRomSpline outerMapSpline;

    private void Start()
    {
        tiles = GetComponentsInChildren<MapTile>();
    }
    public static void CreateMap()
    {
        MapTileInfo[] tileInfos = new MapTileInfo[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].tileInfo = new MapTileInfo(tiles[i].tileType);
            tileInfos[i] = tiles[i].tileInfo;
        }
        mapInfo = new MapInfo(tileInfos);
        ClientHandleNetworkData.onServerRespond_1 += CheckMapInfo;
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

        if (mapInfo == null) Debug.LogError("MapInfo is null!");
        if (mapInfo.CompareTo(serverMapInfo) != 0) Debug.LogError("Local map is different from the one received from server!");
        else
            PlayerController.GameReady();
    }

    public static MapTile GetTile(MapTileInfo.MapTiles tileType)
    {
        foreach(MapTile tile in tiles)
        {
            if (tile.tileType == tileType) return tile;
        }
        return null;
    }

    public static void CreateSplines()
    {
        outerMapSpline.Create();
    }

    public void OnBeforeSerialize()
    {        
    }

    public void OnAfterDeserialize()
    { 
        outerMapSpline = _outerMapSpline;
    }
}
