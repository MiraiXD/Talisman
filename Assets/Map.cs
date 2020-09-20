using UnityEngine;
using System.Collections.Generic;
using ComNet;
using System;

public class Map : MonoBehaviour ,ISerializationCallbackReceiver
{    
    private static MapTile[] tiles;
    private static Dictionary<int, MapTile> mapTiles;
    public static MapInfo mapInfo;

    [SerializeField] private CatmullRomSpline _outerMapSpline;
    private static CatmullRomSpline outerMapSpline;

    private void Awake()
    {
        tiles = GetComponentsInChildren<MapTile>();
    }
    public static void CreateMap()
    {        
        MapTileInfo[] tileInfos = new MapTileInfo[tiles.Length];
        mapTiles = new Dictionary<int, MapTile>();
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].tileInfo = new MapTileInfo(tiles[i].tileType);
            tileInfos[i] = tiles[i].tileInfo;

            mapTiles.Add(tiles[i].tileInfo.id, tiles[i]);
        }
        mapInfo = new MapInfo(tileInfos);        
    }

    public static MapTile GetNextTile(MapTileInfo startingTile, bool counterClockwiseDirection)
    {
        int nextTileID = counterClockwiseDirection ? startingTile.id + 1 : startingTile.id - 1;
        return mapTiles[nextTileID];
    }

    public static PlayerSpot GetSpot(MapTileInfo tileInfo)
    {
        MapTileInfo currentTile = tileInfo;

        PlayerSpot playerSpot = null;
        for (int i = 0; i < 100; i++) // while loop crashes unity - stinks
        {            
            if(!mapTiles.TryGetValue(currentTile.id, out MapTile tile)) { Debug.LogError("STINKS"); return null; }
            foreach (PlayerSpot spot in tile.playerSpots)
            {
                if (spot.occupied) continue;
                else { playerSpot = spot; break; }
            }
            if (playerSpot != null) return playerSpot;            
        }
        Debug.LogError("Could not find a spot!");
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
