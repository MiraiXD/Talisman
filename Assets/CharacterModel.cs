using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public ComNet.CharacterInfo.Characters character;
    private static int mapTileMask;
    public static Vector3 GetPositionOnMapTile(Vector3 position)
    {
        Vector3 origin = position + Vector3.up * 5f;
        Vector3 direction = Vector3.down;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 10f, mapTileMask))
        {
            return hit.point;
        }
        else return Vector3.zero;
    }
    public void MoveTo(PlayerSpot from, PlayerSpot to)
    {

    }
    public void Init()
    {
        //PositionOnMapTile();
        mapTileMask = LayerMask.GetMask("MapTile");
    }
}
